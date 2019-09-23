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
            return new Tuple<dynamic, DataTable>(dt.ToOneLine(), itemdt);
        }
        public string SavePromobill(PROMOBILLEntity data)
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
                data.PROMOBILL_GOODS?.ForEach(item =>
                {
                    GetVerify(item).Require(a => a.BILLID);
                    GetVerify(item).Require(a => a.INX);
                    GetVerify(item).Require(a => a.GOODSID);
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
            //dt.NewEnumColumns<促销单状态>("STATUS", "STATUSMC");
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

            string sql =$@"SELECT P.ID , B.NAME BRANCHNAME, B.ID BRANCHID, P.NAME, P.PRICE, P.STATUS 
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
    }
}
