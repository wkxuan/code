using System;
using System.Collections.Generic;
using System.Data;
using z.ERP.Entities;
using z.ERP.Entities.Enum;
using z.ERP.Entities.Procedures;
using z.Exceptions;
using z.Extensions;
using z.MVC5.Results;
using z.SSO.Model;

namespace z.ERP.Services
{
    public class CxglService : ServiceBase
    {
        internal CxglService()
        {

        }
        #region 促销活动主题
        public DataGridResult SearchPromotion(SearchItem item)
        {
            string sql = $@"SELECT * FROM PROMOTION WHERE 1=1 ";
            item.HasKey("NAME", a => sql += $" and NAME LIKE '%{a}%'");
            item.HasKey("YEAR", a => sql += $" and YEAR= {a}");
            item.HasKey("CONTENT", a => sql += $" and CONTENT LIKE '%{a}%'");
            item.HasDateKey("START_DATE_START", a => sql += $" and START_DATE >= {a}");
            item.HasDateKey("START_DATE_END", a => sql += $" and START_DATE <= {a}");
            item.HasDateKey("END_DATE_START", a => sql += $" and END_DATE >= {a}");
            item.HasDateKey("END_DATE_END", a => sql += $" and END_DATE <= {a}");
            item.HasKey("STATUS", a => sql += $" and STATUS= {a}");
            sql += " ORDER BY ID DESC";
            int count;
            var dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }
        public PROMOTIONEntity PromotionShowOneData(PROMOTIONEntity data)
        {
            string sql = $@"SELECT * FROM PROMOTION WHERE ID=" + data.ID;
            var res = DbHelper.ExecuteOneObject<PROMOTIONEntity>(sql);
            return res;
        }
        #endregion

        #region 促销折扣单
        public DataGridResult GetPromobill(SearchItem item)
        {
            string sql = $@"select P.*,B.NAME BRANCHNAME,T.NAME PROMOTIONNAME  
                              from PROMOBILL P,BRANCH B,PROMOTION T
                             where P.BRANCHID=B.ID and P.PROMOTIONID=T.ID ";
            item.HasKey("PROMOTIONNAME", a => sql += $" and T.NAME LIKE '%{a}%'");
            item.HasKey("REPORTER_NAME", a => sql += $" and P.REPORTER_NAME LIKE '%{a}%'");
            item.HasKey("VERIFY_NAME", a => sql += $" and P.VERIFY_NAME LIKE '%{a}%'");
            item.HasDateKey("START_DATE_START", a => sql += $" and P.START_DATE >= {a}");
            item.HasDateKey("START_DATE_END", a => sql += $" and P.START_DATE <= {a}");
            item.HasDateKey("END_DATE_START", a => sql += $" and P.END_DATE >= {a}");
            item.HasDateKey("END_DATE_END", a => sql += $" and P.END_DATE <= {a}");
            item.HasKey("STATUS", a => sql += $" and P.STATUS in ({a})");
            item.HasKey("BRANCHID", a => sql += $" and P.BRANCHID in ({a})");
            item.HasKey("PROMOTYPE", a => sql += $" and P.PROMOTYPE in ({a})");
            sql += " ORDER BY BILLID DESC";
            int count;
            var dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<促销单状态>("STATUS", "STATUSMC");
            dt.NewEnumStrColumns<星期>("WEEK", "WEEKMC");
            return new DataGridResult(dt, count);
        }

        public Tuple<dynamic, DataTable> PromobillShowOneData(PROMOBILLEntity data)
        {
            string sql = @"select P.*,T.NAME PROMOTIONNAME,T.START_DATE START_DATE_LIMIT,T.END_DATE END_DATE_LIMIT 
                             from PROMOBILL P,PROMOTION T 
                            where P.PROMOTIONID=T.ID and P.BILLID={0}";
            var dt = DbHelper.ExecuteTable(string.Format(sql, data.BILLID));
            if (dt.Rows.Count == 0)
            {
                throw new LogicException("找不到促销折扣单!");
            }
            dt.NewEnumColumns<促销单状态>("STATUS", "STATUSMC");

            string sqlitem = @"select P.*,G.GOODSDM,G.NAME GOODSNAME,B.NAME BRANDMC,F.NAME VALUE2MC 
                                 from PROMOBILL_GOODS P,GOODS G,BRAND B,FR_PLAN F  
                                where P.GOODSID=G.GOODSID and G.BRANDID=B.ID and P.VALUE2=F.ID(+) and P.BILLID={0} order by P.INX ASC";
            var itemdt = DbHelper.ExecuteTable(string.Format(sqlitem, data.BILLID));
            foreach (DataRow dr in itemdt.Rows)
            {
                if (!dr["VALUE1"].ToString().IsEmpty())
                    dr["VALUE1"] = dr["VALUE1"].ToString().ToDouble() * 100;
            }
            return new Tuple<dynamic, DataTable>(dt.ToOneLine(), itemdt);
        }
        public string SavePromobill(PROMOBILLEntity data)
        {
            var v = GetVerify(data);
            if (data.BILLID.IsEmpty())
                data.BILLID = data.BRANCHID + NewINC("PROMOBILL_" + data.BRANCHID).PadLeft(7, '0');

            data.STATUS = ((int)促销单状态.未审核).ToString();
            data.REPORTER = employee.Id;
            data.REPORTER_NAME = employee.Name;
            data.REPORTER_TIME = DateTime.Now.ToString();

            v.IsUnique(a => a.BILLID);
            v.Require(a => a.BILLID);
            v.Require(a => a.BRANCHID);
            v.Require(a => a.PROMOTYPE);
            v.Require(a => a.PROMOTIONID);
            v.Require(a => a.START_DATE);
            v.Require(a => a.END_DATE);
            v.Require(a => a.WEEK);
            v.Require(a => a.START_TIME);
            v.Require(a => a.END_TIME);
            v.Verify();

            using (var Tran = DbHelper.BeginTransaction())
            {
                data.PROMOBILL_GOODS?.ForEach(item =>
                {
                    GetVerify(item).Require(a => a.BILLID);
                    GetVerify(item).Require(a => a.INX);
                    GetVerify(item).Require(a => a.GOODSID);
                    if (!item.VALUE1.IsEmpty())
                    {
                        item.VALUE1 = (item.VALUE1.ToDouble() / 100).ToString();
                    }
                });
                DbHelper.Save(data);

                ////增加审核待办任务
                //var dcl = new BILLSTATUSEntity
                //{
                //    BILLID = data.BILLID,
                //    MENUID = "",
                //    BRABCHID = data.BRANCHID,
                //    URL = "CXGL/PROMOBILL_DIS/Promobill_DisEdit/"
                //};
                //InsertDclRw(dcl);

                Tran.Commit();
            }

            return data.BILLID;
        }
        public string ExecPromobill(PROMOBILLEntity data)
        {
            if (data.STATUS == ((int)促销单状态.审核).ToString())
            {
                throw new LogicException("单据(" + data.BILLID + ")已经审核不能再次审核!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                EXEC_PROMOBILL exec = new EXEC_PROMOBILL()
                {
                    in_BILLID = data.BILLID,
                    in_USERID = employee.Id
                };
                DbHelper.ExecuteProcedure(exec);

                ////删除审核待办任务
                //var dcl = new BILLSTATUSEntity
                //{
                //    BILLID = data.BILLID,
                //    MENUID = "",
                //    BRABCHID = data.BRANCHID
                //};
                //DelDclRw(dcl);

                Tran.Commit();
            }

            return data.BILLID;
        }
        public void DeletePromobill(List<PROMOBILLEntity> data)
        {
            foreach (var con in data)
            {
                var Data = DbHelper.Select(con);
                if (Data.STATUS != ((int)促销单状态.未审核).ToString())
                    throw new LogicException($"单据(" + Data.BILLID + ")已经不是未审核不能删除!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                foreach (var con in data)
                {
                    ////删除审核待办任务
                    //var dcl = new BILLSTATUSEntity
                    //{
                    //    BILLID = con.BILLID,
                    //    MENUID = "",
                    //    BRABCHID = con.BRANCHID
                    //};
                    //DelDclRw(dcl);

                    DbHelper.Delete(con);
                }
                Tran.Commit();
            }
        }
        public string BeginPromobill(PROMOBILLEntity data)
        {
            using (var Tran = DbHelper.BeginTransaction())
            {
                EXEC_PROMOBILL_STARTUP exec = new EXEC_PROMOBILL_STARTUP()
                {
                    in_BILLID = data.BILLID,
                    in_USERID = employee.Id
                };
                DbHelper.ExecuteProcedure(exec);
                Tran.Commit();
            }
            return data.BILLID;
        }
        public string StopPromobill(PROMOBILLEntity data)
        {
            using (var Tran = DbHelper.BeginTransaction())
            {
                EXEC_PROMOBILL_STOP exec = new EXEC_PROMOBILL_STOP()
                {
                    in_BILLID = data.BILLID,
                    in_USERID = employee.Id
                };
                DbHelper.ExecuteProcedure(exec);
                Tran.Commit();
            }
            return data.BILLID;
        }
        #endregion

        #region 促销赠品单
        public string SavePromobill_FG(PROMOBILLEntity data)
        {
            var v = GetVerify(data);
            if (data.BILLID.IsEmpty())
                data.BILLID = NewINC("PROMOBILL");

            data.STATUS = ((int)促销单状态.未审核).ToString();
            data.REPORTER = employee.Id;
            data.REPORTER_NAME = employee.Name;
            data.REPORTER_TIME = DateTime.Now.ToString();

            v.IsUnique(a => a.BILLID);
            v.Require(a => a.BILLID);
            v.Require(a => a.BRANCHID);
            v.Require(a => a.PROMOTYPE);
            v.Require(a => a.PROMOTIONID);
            v.Require(a => a.START_DATE);
            v.Require(a => a.END_DATE);
            v.Require(a => a.WEEK);
            v.Require(a => a.START_TIME);
            v.Require(a => a.END_TIME);
            v.Verify();

            using (var Tran = DbHelper.BeginTransaction())
            {
                data.PROMOBILL_FG_RULE?.ForEach(item =>
                {
                    GetVerify(item).Require(a => a.BILLID);
                    GetVerify(item).Require(a => a.INX);
                    GetVerify(item).Require(a => a.FULL);
                    GetVerify(item).Require(a => a.PRESENTID);
                });
                DbHelper.Save(data);

                Tran.Commit();
            }

            ////增加审核待办任务
            //var dcl = new BILLSTATUSEntity
            //{
            //    BILLID = data.BILLID,
            //    MENUID = "",
            //    BRABCHID = data.BRANCHID,
            //    URL = "CXGL/PROMOBILL_DIS/Promobill_DisEdit/"
            //};
            //InsertDclRw(dcl);

            return data.BILLID;
        }
        public void DeletePromobill_FG(List<PROMOBILLEntity> data)
        {
            foreach (var con in data)
            {
                var Data = DbHelper.Select(con);
                if (Data.STATUS != ((int)促销单状态.未审核).ToString())
                    throw new LogicException($"单据(" + Data.BILLID + ")已经不是未审核不能删除!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                foreach (var con in data)
                {
                    ////删除审核待办任务
                    //var dcl = new BILLSTATUSEntity
                    //{
                    //    BILLID = con.BILLID,
                    //    MENUID = "",
                    //    BRABCHID = con.BRANCHID
                    //};
                    //DelDclRw(dcl);

                    DbHelper.Delete(con);
                }
                Tran.Commit();
            }
        }
        public Tuple<dynamic, DataTable> Promobill_FGShowOneData(PROMOBILLEntity data)
        {
            string sql = @"select P.*,T.NAME PROMOTIONNAME 
                             from PROMOBILL P,PROMOTION T 
                            where P.PROMOTIONID=T.ID and P.BILLID={0}";
            var dt = DbHelper.ExecuteTable(string.Format(sql, data.BILLID));
            if (dt.Rows.Count == 0)
            {
                throw new LogicException("找不到促销赠品单!");
            }
            dt.NewEnumColumns<促销单状态>("STATUS", "STATUSMC");

            string sqlitem = @"select P.*,G.ID,G.NAME PRESENTNAME
                                 from PROMOBILL_FG_RULE P,PRESENT G
                                where P.PRESENTID=G.ID and P.BILLID={0} order by P.INX ASC";
            var itemdt = DbHelper.ExecuteTable(string.Format(sqlitem, data.BILLID));
            return new Tuple<dynamic, DataTable>(dt.ToOneLine(), itemdt);
        }
        #endregion

        #region 满减方案
        public DataGridResult GetFRPLAN(SearchItem item)
        {
            string sql = $@"SELECT * FROM FR_PLAN WHERE 1=1 ";
            item.HasKey("NAME", a => sql += $" and NAME LIKE '%{a}%'");
            item.HasKey("ID", a => sql += $" and ID= {a}");
            item.HasKey("FRTYPE", a => sql += $" and FRTYPE= {a}");
            item.HasKey("STATUS", a => sql += $" and STATUS= {a}");
            sql += " ORDER BY ID DESC";
            int count;
            var dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<使用状态>("STATUS", "STATUSMC");
            dt.NewEnumColumns<满减方式>("FRTYPE", "FRTYPEMC");
            return new DataGridResult(dt, count);
        }
        public string SaveFRPLAN(FR_PLANEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            if (DefineSave.ID.IsEmpty())
            {
                DefineSave.ID = CommonService.NewINC("FR_PLAN");
                DefineSave.STATUS = "1";
            }
            v.Require(a => a.NAME);
            v.Require(a => a.LIMIT);
            v.Require(a => a.FRTYPE);
            if (DefineSave.STATUS == "2")
            {
                throw new LogicException("数据已使用状态不能更改!");
            };
            DefineSave.FR_PLAN_ITEM?.ForEach(sdb =>
            {
                GetVerify(sdb).Require(a => a.ID);
            });
            v.Verify();
            using (var tran = DbHelper.BeginTransaction())
            {
                DbHelper.Save(DefineSave);
                tran.Commit();
            }
            return DefineSave.ID;
        }
        public Tuple<dynamic, DataTable> GetFRPLANInfo(FR_PLANEntity Data)
        {
            string sql = $@"SELECT * FROM FR_PLAN WHERE ID={Data.ID}";

            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.NewEnumColumns<使用状态>("STATUS", "STATUSMC");

            var sql1 = $@"SELECT * from FR_PLAN_ITEM WHERE ID={Data.ID} ";

            sql1 += " order by INX";
            DataTable dt1 = DbHelper.ExecuteTable(sql1);

            return new Tuple<dynamic, DataTable>(dt.ToOneLine(), dt1);
        }
        #endregion

        #region 赠品定义
        /// <summary>
        /// 赠品定义
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string PresentSql(SearchItem item)
        {
            string sql = $@"SELECT P.ID , B.NAME BRANCHNAME, B.ID BRANCHID, P.NAME, P.PRICE, P.STATUS 
                            FROM PRESENT P,BRANCH B
                            WHERE B.ID=P.BRANCHID";
            sql += "  AND B.ID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限

            item.HasKey("ID", a => sql += $" and P.ID LIKE '%{a}%'");
            item.HasKey("BRANCHID", a => sql += $" and B.ID LIKE '%{a}%'");
            item.HasKey("NAME", a => sql += $" and P.NAME LIKE '%{a}%'");
            item.HasKey("PRICE", a => sql += $" and P.PRICE LIKE '%{a}%'");
            item.HasKey("STATUS", a => sql += $" and P.STATUS LIKE '%{a}%'");
            sql += " ORDER BY B.ID DESC";
            return sql;
            //int count;
            //var dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            //dt.NewEnumColumns<促销单状态>("STATUS", "STATUSMC");
            //return new DataGridResult(dt, count);
        }
        public DataGridResult Present(SearchItem item)
        {
            string sql = PresentSql(item);
            int count;
            var dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<促销单状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }

        public DataTable PresentDetail(SearchItem item)
        {
            string sql = PresentSql(item);
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt;
        }
        public DataTable GetPresent(PresentEntity data)
        {

            string sql = $@"SELECT P.ID , B.NAME BRANCHNAME, B.ID BRANCHID, P.NAME, P.PRICE, P.STATUS 
                            FROM PRESENT P,BRANCH B
                            WHERE B.ID=P.BRANCHID";
            sql += "  AND B.ID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            sql += " and P.ID=" + data.ID;
            sql += " ORDER BY B.ID DESC";
            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.NewEnumColumns<使用状态>("STATUS", "STATUSMC");
            return dt;
        }

        public void DeletePresent(List<PresentEntity> DeleteData)
        {
            foreach (var item in DeleteData)
            {
                PresentEntity Data = DbHelper.Select(item);
                //    if (Data.STATUS == ((int)使用状态.已使用).ToString())
                //    {
                //        throw new LogicException("已经审核不能删除!");
                //    }
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                foreach (var item in DeleteData)
                {
                    DbHelper.Delete(item);
                }
                Tran.Commit();
            }
        }
        #endregion

        #region 赠品发放 
        public DataGridResult Present_SendList(SearchItem item)
        {
            string sql = $@"select P.*,B.NAME BRANCHNAME
                              from PRESENT_SEND P,BRANCH B
                             where P.BRANCHID=B.ID ";
            item.HasKey("BILLID", a => sql += $" and P.BILLID = '{a}'");
            item.HasKey("REPORTER_NAME", a => sql += $" and P.REPORTER_NAME LIKE '%{a}%'");
            item.HasKey("VERIFY_NAME", a => sql += $" and P.VERIFY_NAME LIKE '%{a}%'");
            item.HasDateKey("REPORTER_TIME_START", a => sql += $" and TRUNC(P.REPORTER_TIME)>={a}");
            item.HasDateKey("REPORTER_TIME_END", a => sql += $" and TRUNC(P.REPORTER_TIME)<={a}");
            item.HasDateKey("VERIFY_TIME_START", a => sql += $" and TRUNC(P.VERIFY_TIME)>={a}");
            item.HasDateKey("VERIFY_TIME_END", a => sql += $" and TRUNC(P.VERIFY_TIME)<={a}");
            item.HasKey("STATUS", a => sql += $" and P.STATUS in ({a})");
            item.HasKey("BRANCHID", a => sql += $" and P.BRANCHID in ({a})");
            sql += " ORDER BY P.BILLID DESC";
            int count;
            var dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<促销单状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }
        public Tuple<dynamic, int> GetSaleTicket(string BRANCHID, string POSNO, string DEALID)
        {
            int status = 0;
            string sql = $@"SELECT S.POSNO ,S.DEALID,S.SALE_AMOUNT AMOUNT,S.SALE_TIME
                    FROM ALLSALE S,STATION ST
                    WHERE S.POSNO =ST.STATIONBH AND S.ISFG=1 AND S.SALE_AMOUNT>0 AND S.POSNO='{POSNO}' AND DEALID='{DEALID}' AND ST.BRANCHID='{BRANCHID}' 
                    AND NOT EXISTS (SELECT POSNO,DEALID FROM ALLSALE WHERE POSNO_OLD='{POSNO}' AND DEALID_OLD='{DEALID}')";
            var dt = DbHelper.ExecuteTable(sql);
            if (dt.Rows.Count > 0)
            {
                var time = dt.Rows[0]["SALE_TIME"].ToString();
                string sql1 = $@" select billid from promobill
                     where promotype = 4 
                       and status = 3
                       and branchid={BRANCHID} 
                       and trunc(TO_DATE('{time}','yyyy-mm-dd hh24:mi:ss')) >= start_date
                       and trunc(TO_DATE('{time}','yyyy-mm-dd hh24:mi:ss')) <= end_date
                       and instr(week, to_char(TO_DATE('{time}','yyyy-mm-dd hh24:mi:ss')-1,'d'))>0
                       and to_number(to_char(TO_DATE('{time}','yyyy-mm-dd hh24:mi:ss'),'hh24'))*60 +to_number(to_char(TO_DATE('{time}','yyyy-mm-dd hh24:mi:ss'),'mi'))>= start_time 
                       and to_number(to_char(TO_DATE('{time}','yyyy-mm-dd hh24:mi:ss'),'hh24'))*60 +to_number(to_char(TO_DATE('{time}','yyyy-mm-dd hh24:mi:ss'),'mi'))<= end_time
                       order by billid desc";
                var dt1 = DbHelper.ExecuteTable(sql1);
                if (dt1.Rows.Count > 0)
                {
                    dt.Columns.Add("FGID", typeof(string));
                    dt.Rows[0]["FGID"] = dt1.Rows[0]["BILLID"].ToString();
                    status = 1;   //正常
                }
                else
                {
                    status = 3;  //没有参加活动
                }
            }
            else
            {
                status = 2;  //无数据
            }
            return new Tuple<dynamic, int>(dt.ToOneLine(), status);
        }
        /// <summary>
        /// 获取符合条件的赠品
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public DataTable GetPresentList(List<PROMOBILL_FG_RULEEntity> Data)
        {
            string sql = "";
            if (Data == null) { return null; }
            for (var i = 0; i < Data.Count; i++)
            {
                if (i != 0)
                {
                    sql += " union all ";
                }
                sql += $@"SELECT A.PRESENTID,P.NAME PRESENTNAME,P.PRICE
                FROM PROMOBILL_FG_RULE A,PRESENT P
                WHERE A.PRESENTID=P.ID AND A.BILLID ={Data[i].BILLID} AND  A.FULL <={Data[i].FULL}";
            }
            string sqlall = $@" SELECT DISTINCT * FROM (" + sql + ") ORDER BY PRESENTID";
            var dt = DbHelper.ExecuteTable(sqlall);
            return dt;
        }
        public string SavePresent_Send(PRESENT_SENDEntity data)
        {
            var v = GetVerify(data);
            if (data.BILLID.IsEmpty())
                data.BILLID = NewINC("PRESENT_SEND");

            data.STATUS = ((int)促销单状态.未审核).ToString();
            data.REPORTER = employee.Id;
            data.REPORTER_NAME = employee.Name;
            data.REPORTER_TIME = DateTime.Now.ToString();

            v.IsUnique(a => a.BILLID);
            v.Require(a => a.BILLID);
            v.Require(a => a.BRANCHID);
            v.Verify();

            using (var Tran = DbHelper.BeginTransaction())
            {
                data.PRESENT_SEND_TICKET?.ForEach(item =>
                {
                    item.SALE_TIME.ToDateTime().ToString();
                });
                DbHelper.Save(data);
                Tran.Commit();
            }
            ////增加审核待办任务
            //var dcl = new BILLSTATUSEntity
            //{
            //    BILLID = data.BILLID,
            //    MENUID = "",
            //    BRABCHID = data.BRANCHID,
            //    URL = "CXGL/PROMOBILL_DIS/Promobill_DisEdit/"
            //};
            //InsertDclRw(dcl);

            return data.BILLID;
        }
        public void DeletePresent_Send(List<PRESENT_SENDEntity> data)
        {
            foreach (var con in data)
            {
                var Data = DbHelper.Select(con);
                if (Data.STATUS != ((int)促销单状态.未审核).ToString())
                    throw new LogicException($"单据(" + Data.BILLID + ")已经不是未审核不能删除!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                foreach (var con in data)
                {
                    ////删除审核待办任务
                    //var dcl = new BILLSTATUSEntity
                    //{
                    //    BILLID = con.BILLID,
                    //    MENUID = "",
                    //    BRABCHID = con.BRANCHID
                    //};
                    //DelDclRw(dcl);

                    DbHelper.Delete(con);
                }
                Tran.Commit();
            }
        }
        public string ExecPresent_Send(PRESENT_SENDEntity data)
        {
            if (data.STATUS == ((int)促销单状态.审核).ToString())
            {
                throw new LogicException("单据(" + data.BILLID + ")已经审核不能再次审核!");
            }
            data.STATUS = ((int)促销单状态.审核).ToString();
            using (var Tran = DbHelper.BeginTransaction())
            {
                EXEC_PRESENT_SEND exec = new EXEC_PRESENT_SEND()
                {
                    in_BILLID = data.BILLID,
                    in_USERID = employee.Id
                };
                DbHelper.ExecuteProcedure(exec);
                ////删除审核待办任务
                //var dcl = new BILLSTATUSEntity
                //{
                //    BILLID = data.BILLID,
                //    MENUID = "",
                //    BRABCHID = data.BRANCHID
                //};
                //DelDclRw(dcl);

                Tran.Commit();
            }

            return data.BILLID;
        }
        public Tuple<dynamic, DataTable, DataTable> Present_SendShowOneData(PRESENT_SENDEntity data)
        {
            string sql = $@"SELECT * FROM PRESENT_SEND WHERE BILLID={data.BILLID} ";
            var dt = DbHelper.ExecuteTable(sql);

            string sql1 = $@"SELECT * FROM PRESENT_SEND_TICKET WHERE BILLID ={data.BILLID} ORDER BY SALE_TIME";
            var dt1 = DbHelper.ExecuteTable(string.Format(sql1));

            string sql2 = $@"SELECT A.*,B.NAME PRESENTNAME FROM PRESENT_SEND_ITEM A,PRESENT B
                    WHERE A.PRESENTID=B.ID AND A.BILLID={data.BILLID} ORDER BY A.PRESENTID";
            var dt2 = DbHelper.ExecuteTable(string.Format(sql2));

            return new Tuple<dynamic, DataTable, DataTable>(dt.ToOneLine(), dt1, dt2);
        }
        #endregion

        #region 活动小票兑奖
        public DataGridResult GetNULL(SearchItem item)
        {
            DataTable dt = new DataTable();
            int count = 0;
            return new DataGridResult(dt, count);
        }
        public DataTable GetSaleTicketInfo(string PROMOTIONID, string POSNO, string DEALID)
        {
            string sql = $@"SELECT S.POSNO ,S.DEALID,S.SALE_AMOUNT AMOUNT,S.SALE_TIME
                    FROM ALLSALE S
                    WHERE S.ISFG in (1,2) AND S.SALE_AMOUNT>0 AND S.POSNO='{POSNO}' AND DEALID='{DEALID}' 
                    AND NOT EXISTS (SELECT POSNO,DEALID FROM ALLSALE WHERE POSNO_OLD='{POSNO}' AND DEALID_OLD='{DEALID}') 
                    AND NOT EXISTS (SELECT POSNO,DEALID FROM TICKET_ACTIVITY_HISTORY WHERE POSNO='{POSNO}' AND DEALID='{DEALID}' AND PROMOTIONID='{PROMOTIONID}')";
            var dt = DbHelper.ExecuteTable(sql);
            return dt;
        }
        public bool SaveTICKET_ACTIVITY_HISTORY(List<TICKET_ACTIVITY_HISTORYEntity> DefineSave) {
            foreach (var item in DefineSave) {
                var v = GetVerify(item);
                TICKET_ACTIVITY_HISTORYEntity Data = DbHelper.Select(item);
                if (Data!=null) { throw new LogicException($@"终端号:{Data.POSNO},小票号:{Data.DEALID} 已参加过该活动!"); }
                item.REPORTER = employee.Id;
                item.REPORTER_NAME = employee.Name;
                item.REPORTER_TIME = DateTime.Now.ToString();

                v.Require(a => a.PROMOTIONID);
                v.Require(a => a.POSNO);
                v.Require(a => a.DEALID);
                using (var tran = DbHelper.BeginTransaction())
                {
                    DbHelper.Save(item);
                    tran.Commit();
                }
            }
            return true;
        }
        #endregion

        #region 随机立减单
        public string SavePromobill_RR(PROMOBILL_RREntity data)
        {
            var v = GetVerify(data);
            if (data.BILLID.IsEmpty())
                data.BILLID = data.BRANCHID + NewINC("PROMOBILL_" + data.BRANCHID).PadLeft(7, '0');

            data.STATUS = ((int)促销单状态.未审核).ToString();
            data.REPORTER = employee.Id;
            data.REPORTER_NAME = employee.Name;
            data.REPORTER_TIME = DateTime.Now.ToString();

            v.IsUnique(a => a.BILLID);
            v.Require(a => a.BILLID);
            v.Require(a => a.BRANCHID);
            v.Require(a => a.PROMOTYPE);
            v.Require(a => a.PROMOTIONID);
            v.Require(a => a.START_DATE);
            v.Require(a => a.END_DATE);
            v.Require(a => a.WEEK);
            v.Require(a => a.START_TIME);
            v.Require(a => a.END_TIME);
            v.Verify();

            using (var Tran = DbHelper.BeginTransaction())
            {
                data.PROMOBILL_RR_BRAND?.ForEach(item =>
                {
                    if (!item.REDUCE_UP_RATE.IsEmpty())
                    {
                        item.REDUCE_UP_RATE = (item.REDUCE_UP_RATE.ToDouble() / 100).ToString();
                    }
                });
                DbHelper.Save(data);

                ////增加审核待办任务
                //var dcl = new BILLSTATUSEntity
                //{
                //    BILLID = data.BILLID,
                //    MENUID = "",
                //    BRABCHID = data.BRANCHID,
                //    URL = "CXGL/PROMOBILL_DIS/Promobill_DisEdit/"
                //};
                //InsertDclRw(dcl);

                Tran.Commit();
            }

            return data.BILLID;
        }
        public Tuple<dynamic, DataTable> Promobill_RRShowOneData(PROMOBILL_RREntity data)
        {
            string sql = @"select P.*,T.NAME PROMOTIONNAME,T.START_DATE START_DATE_LIMIT,T.END_DATE END_DATE_LIMIT 
                             from PROMOBILL_RR P,PROMOTION T 
                            where P.PROMOTIONID=T.ID and P.BILLID={0}";
            var dt = DbHelper.ExecuteTable(string.Format(sql, data.BILLID));
            if (dt.Rows.Count == 0)
            {
                throw new LogicException("找不到随机立减单!");
            }
            dt.NewEnumColumns<促销单状态>("STATUS", "STATUSMC");

            string sqlitem = @"select P.*,B.NAME BRANDNAME
                                 from PROMOBILL_RR_BRAND P,BRAND B 
                                where  P.BRANDID=B.ID and P.BILLID={0} order by P.INX ASC";
            var itemdt = DbHelper.ExecuteTable(string.Format(sqlitem, data.BILLID));
            foreach (DataRow dr in itemdt.Rows)
            {
                if (!dr["REDUCE_UP_RATE"].ToString().IsEmpty())
                    dr["REDUCE_UP_RATE"] = dr["REDUCE_UP_RATE"].ToString().ToDouble() * 100;
            }
            return new Tuple<dynamic, DataTable>(dt.ToOneLine(), itemdt);
        }
        public void DeletePromobill_RR(List<PROMOBILL_RREntity> data)
        {
            foreach (var con in data)
            {
                var Data = DbHelper.Select(con);
                if (Data.STATUS != ((int)促销单状态.未审核).ToString())
                    throw new LogicException($"单据(" + Data.BILLID + ")已经不是未审核不能删除!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                foreach (var con in data)
                {
                    ////删除审核待办任务
                    //var dcl = new BILLSTATUSEntity
                    //{
                    //    BILLID = con.BILLID,
                    //    MENUID = "",
                    //    BRABCHID = con.BRANCHID
                    //};
                    //DelDclRw(dcl);

                    DbHelper.Delete(con);
                }
                Tran.Commit();
            }
        }
        #endregion
    }
}
