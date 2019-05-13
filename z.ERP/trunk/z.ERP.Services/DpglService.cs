using System.Data;
using z.MVC5.Results;
using z.ERP.Entities;
using System.Collections.Generic;
using z.Extensions;
using System;
using z.ERP.Entities.Enum;
using z.Exceptions;
using z.ERP.Entities.Procedures;

namespace z.ERP.Services
{
    public class DpglService : ServiceBase
    {
        internal DpglService()
        {
        }

        public DataGridResult SearchRegion(SearchItem item)
        {
            string sql = $@"SELECT A.REGIONID,A.CODE,A.NAME FROM REGION A WHERE 1=1";
            item.HasKey("REGIONID,", a => sql += $" and A.REGIONID = {a}");
            item.HasKey("CODE,", a => sql += $" and A.CODE = '{a}'");
            item.HasKey("NAME", a => sql += $" and A.NAME = '{a}'");
            item.HasKey("BRANCHID", a => sql += $" and A.BRANCHID = {a}");
            sql += " ORDER BY  A.REGIONID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public Tuple<dynamic, DataTable> GetRegion(REGIONEntity Data)
        {
            string sql = "select A.*,B.ORGIDCASCADER from REGION A,ORG B where A.ORGID=B.ORGID(+)";
            if (!Data.BRANCHID.IsEmpty())
                sql += (" AND A.BRANCHID = " + Data.BRANCHID);
            if (!Data.REGIONID.IsEmpty())
                sql += (" AND A.REGIONID= " + Data.REGIONID);
            if (!Data.CODE.IsEmpty())
                sql += (" AND A.CODE= " + Data.CODE);
            if (!Data.NAME.IsEmpty())
                sql += (" AND A.NAME like %" + Data.NAME + "%");
            DataTable region = DbHelper.ExecuteTable(sql);
            return new Tuple<dynamic, DataTable>(region.ToOneLine(), region);
        }

        public DataGridResult SearchFloor(SearchItem item)
        {
            string sql = $@"SELECT A.ID,A.CODE,A.NAME FROM FLOOR A WHERE 1=1";
            item.HasKey("ID,", a => sql += $" and A.ID = {a}");
            item.HasKey("CODE,", a => sql += $" and A.CODE = '{a}'");
            item.HasKey("NAME", a => sql += $" and A.NAME = '{a}'");
            item.HasKey("BRANCHID", a => sql += $" and A.BRANCHID = {a}");
            item.HasKey("REGIONID", a => sql += $" and A.REGIONID= {a}");
            sql += " ORDER BY  A.ID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }


        /// <summary>
        /// 可返回一行楼层记录或符合条件的所有记录
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Tuple<dynamic,DataTable> GetFloor(FLOOREntity Data)
        {
            string sql = $@"select A.*,B.ORGIDCASCADER from FLOOR A,ORG B where A.ORGID=B.ORGID(+) ";
            if (!Data.ID.IsEmpty())
                sql += (" AND A.ID= " + Data.ID);
            if (!Data.CODE.IsEmpty())
                sql += (" AND A.CODE= " + Data.CODE);
            if (!Data.NAME.IsEmpty())
                sql += (" AND A.NAME like %" + Data.NAME+"%");
            DataTable floor = DbHelper.ExecuteTable(sql);
            return new Tuple<dynamic,DataTable>(floor.ToOneLine(),floor);
        }

        public DataGridResult SearchShop(SearchItem item)
        {
            string sql = $@"SELECT  A.*,A.CODE SHOPCODE,A.AREA_BUILD AREA,B.CATEGORYCODE,B.CATEGORYNAME,D.NAME BRANCHNAME,F.NAME FLOORNAME " +
                   "  FROM SHOP A,CATEGORY B,ORG C,BRANCH D,FLOOR F "
                   + " WHERE  A.CATEGORYID = B.CATEGORYID(+) and A.ORGID=C.ORGID(+)"
                   + " and  A.BRANCHID = D.ID and A.FLOORID=F.ID";
            item.HasKey("CODE", a => sql += $" and A.CODE like '%{a}%'");
            item.HasKey("NAME", a => sql += $" and A.NAME like '%{a}%'");
            item.HasKey("BRANCHID", a => sql += $" and A.BRANCHID = '{a}'");
            item.HasKey("FLOORID", a => sql += $" and A.FLOORID = '{a}'");
            item.HasKey("REGIONID", a => sql += $" and A.REGIONID = {a}");
            sql += " ORDER BY  A.CODE";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        /// <summary>
        /// 可返回一行店铺记录或符合条件的所有记录
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Tuple<dynamic, DataTable> GetShop(SHOPEntity Data)
        {
            string sql = $@"SELECT  A.*,B.CATEGORYCODE,B.CATEGORYNAME,B.CATEGORYIDCASCADER,A.AREA_BUILD AREA,C.ORGIDCASCADER " +
                   "  FROM SHOP A,CATEGORY B,ORG C WHERE  A.CATEGORYID = B.CATEGORYID(+) "
                   + " and A.ORGID=C.ORGID(+)";
            if (!Data.SHOPID.IsEmpty())
                sql += (" AND A.SHOPID= " + Data.SHOPID);
            if (!Data.CODE.IsEmpty())
                sql += (" AND A.CODE= " + Data.CODE);
            if (!Data.NAME.IsEmpty())
                sql += (" AND A.NAME like %" + Data.NAME + "%");
            DataTable shop = DbHelper.ExecuteTable(sql);
            return new Tuple<dynamic, DataTable>(shop.ToOneLine(), shop);
        }
        public object GetOneShop(SHOPEntity Data)
        {
            string sql = " SELECT  A.* FROM SHOP A " +
                "  WHERE  1=1 ";
            if (!Data.SHOPID.IsEmpty())
                sql += (" and A.SHOPID= " + Data.SHOPID);
            DataTable dt = DbHelper.ExecuteTable(sql);
            return new
            {
                dt = dt.ToOneLine()
            };
        }
        public DataGridResult GetAssetChangeList(SearchItem item)
        {
            string sql = $@"SELECT A.*,B.NAME BRANCHNAME FROM ASSETCHANGE A,BRANCH B WHERE A.BRANCHID=B.ID ";
            item.HasKey("CHANGE_TYPE", a => sql += $" and CHANGE_TYPE = '{a}'");
            item.HasKey("STATUS", a => sql += $" and A.STATUS = '{a}'");
            item.HasKey("BRANCHID", a => sql += $" and A.BRANCHID  = '{a}'");
            item.HasKey("DISCRIPTION", a => sql += $" and A.DISCRIPTION  LIKE '%{a}%'");
            sql += " ORDER BY  BILLID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }

        public void DeleteAssetChange(List<ASSETCHANGEEntity> DeleteData)
        {
            foreach (var item in DeleteData)
            {
                ASSETCHANGEEntity Data = DbHelper.Select(item);
                if (Data.STATUS == ((int)普通单据状态.审核).ToString())
                {
                    throw new LogicException("已经审核不能删除!");
                }
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

        public string SaveAssetChange(ASSETCHANGEEntity SaveData)
        {
            var v = GetVerify(SaveData);
            if (SaveData.BILLID.IsEmpty())
                SaveData.BILLID = NewINC("ASSETCHANGE");
            SaveData.STATUS = ((int)普通单据状态.未审核).ToString();
            SaveData.REPORTER = employee.Id;
            SaveData.REPORTER_NAME = employee.Name;
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            v.Require(a => a.BILLID);
            v.Require(a => a.CHANGE_TYPE);
            v.Require(a => a.BRANCHID);
            v.Verify();

            using (var Tran = DbHelper.BeginTransaction())
            {
                SaveData.ASSETCHANGEITEM2?.ForEach(newasset =>
               {
                   GetVerify(newasset).Require(a => a.ASSETID);
               });
                DbHelper.Save(SaveData);

                Tran.Commit();
            }
            return SaveData.BILLID;
        }


        public Tuple<dynamic, DataTable, DataTable> GetAssetChangeElement(ASSETCHANGEEntity Data)
        {
            //此处校验一次只能查询一个单号,校验单号必须存在
            string sql = $@"SELECT A.*,B.NAME BRANCHNAME FROM ASSETCHANGE A,BRANCH B  WHERE A.BRANCHID=B.ID ";
            if (!Data.BILLID.IsEmpty())
                sql += (" AND BILLID= " + Data.BILLID);
            DataTable assetchange = DbHelper.ExecuteTable(sql);
            assetchange.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");

            string sqlitem = $@"SELECT M.*,P.CODE " +
                " FROM ASSETCHANGEITEM M，SHOP P " +
                " where M.ASSETID=P.SHOPID ";
            if (!Data.BILLID.IsEmpty())
                sqlitem += (" and M.BILLID= " + Data.BILLID);
            DataTable assetchangeitem = DbHelper.ExecuteTable(sqlitem);

            string sqlitem2 = $@"SELECT M.*,P.CODE " +
                " FROM ASSETCHANGEITEM2 M，SHOP P " +
                " where M.ASSETID=P.SHOPID ";
            if (!Data.BILLID.IsEmpty())
                sqlitem2 += (" and M.BILLID= " + Data.BILLID);
            DataTable assetchangeitem2 = DbHelper.ExecuteTable(sqlitem2);

            return new Tuple<dynamic, DataTable, DataTable>(assetchange.ToOneLine(), assetchangeitem, assetchangeitem2);
        }
        /// <summary>
        /// 资产变更审核
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public string ExecAssetChange(ASSETCHANGEEntity Data)
        {
            ASSETCHANGEEntity assetchange = DbHelper.Select(Data);
            if (assetchange.STATUS == ((int)普通单据状态.审核).ToString())
            {
                throw new LogicException("单据(" + Data.BILLID + ")已经审核不能再次审核!");
            }
            //using (var Tran = DbHelper.BeginTransaction())
            //{
            //    assetchange.VERIFY = employee.Id;
            //    assetchange.VERIFY_NAME = employee.Name;
            //    assetchange.VERIFY_TIME = DateTime.Now.ToString();
            //    assetchange.STATUS = ((int)普通单据状态.审核).ToString();
            //    DbHelper.Save(assetchange);
            //    Tran.Commit();
            //}
            using (var Tran = DbHelper.BeginTransaction())
            {
                EXEC_ASSET_CHANGE exec_asset_change = new EXEC_ASSET_CHANGE()
                {
                    V_BILLID = Data.BILLID,
                    V_USERID = employee.Id
                };
                DbHelper.ExecuteProcedure(exec_asset_change);
                Tran.Commit();
            }
            return assetchange.BILLID;
        }

        /// <summary>
        /// 资产拆分审核
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public string ExecAssetSpilt(ASSETCHANGEEntity Data)
        {
            ASSETCHANGEEntity assetchange = DbHelper.Select(Data);
            if (assetchange.STATUS == ((int)普通单据状态.审核).ToString())
            {
                throw new LogicException("单据(" + Data.BILLID + ")已经审核不能再次审核!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                EXEC_ASSET_SPILT exec_asset_spilt = new EXEC_ASSET_SPILT()
                {
                    V_BILLID = Data.BILLID,
                    V_USERID = employee.Id
                };
                DbHelper.ExecuteProcedure(exec_asset_spilt);
                Tran.Commit();
            }
            return assetchange.BILLID;
        }
        public DataGridResult GetFloorMap(SearchItem item)
        {
            string sql = $@"select A.MAPID, A.FLOORID, A.BACKMAP, A.WIDTHS, A.LENGTHS, A.INITINATE_TIME_P, A.REPORTER
                            , A.REPORTER_NAME, A.REPORTER_TIME, A.VERIFY, A.VERIFY_NAME, A.VERIFY_TIME, A.INITINATE, A.INITINATE_NAME
                            , A.INITINATE_TIME, A.TERMINATE, A.TERMINATE_NAME, A.TERMINATE_TIME, A.STATUS, A.FILENAME
                            , A.TZBJ,D.NAME BRANCHNAME,C.NAME REGIONNAME,B.NAME as FLOORNAME 
                    from FLOORMAP A,FLOOR B,REGION C,BRANCH D 
                    where NVL(A.TZBJ,0)=0 AND A.FLOORID=B.ID AND B.REGIONID=C.REGIONID "
                + " AND C.BRANCHID=D.ID ";
            item.HasKey("MAPID", a => sql += $" and A.MAPID = {a}");
            item.HasKey("BRANCHID", a => sql += $" and C.BRANCHID = '{a}'");
            item.HasKey("STATUS", a => sql += $" and A.STATUS = '{a}'");
            item.HasKey("TZBJ", a => sql += $" and A.TZBJ = '{a}'");
            sql += " ORDER BY  MAPID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<布局图状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetFloorMapAdjust(SearchItem item)
        {
            string sql = $@"select A.MAPID,A.MAPID_OLD, A.FLOORID, A.BACKMAP, A.WIDTHS, A.LENGTHS, A.INITINATE_TIME_P, A.REPORTER
                            , A.REPORTER_NAME, A.REPORTER_TIME, A.INITINATE, A.INITINATE_NAME
                            , A.INITINATE_TIME, A.TERMINATE, A.TERMINATE_NAME, A.TERMINATE_TIME
                            , (CASE NVL(A.TZBJ,0) WHEN 0 THEN '' ELSE TO_CHAR(A.VERIFY) END) VERIFY
                            , (CASE NVL(A.TZBJ,0) WHEN 0 THEN '' ELSE A.VERIFY_NAME END) VERIFY_NAME
                            , (CASE NVL(A.TZBJ,0) WHEN 0 THEN '' ELSE TO_CHAR(A.VERIFY_TIME,'YYYY-MM-DD hh24:mi:ss') END) VERIFY_TIME
                            , (CASE NVL(A.TZBJ,0) WHEN 0 THEN 1 ELSE A.STATUS END) STATUS
                            , (CASE NVL(A.TZBJ,0) WHEN 0 THEN '新建' ELSE '调整'END) TZBJMC
                    , A.FILENAME, A.TZBJ,D.NAME BRANCHNAME,C.NAME REGIONNAME,B.NAME as FLOORNAME 
                    from FLOORMAP A,FLOOR B,REGION C,BRANCH D 
                    where (A.TZBJ=1 OR (NVL(A.TZBJ,0)=0 AND A.STATUS=3 ))  AND A.FLOORID=B.ID AND B.REGIONID=C.REGIONID "
                + " AND C.BRANCHID=D.ID ";
            item.HasKey("MAPID", a => sql += $" and A.MAPID = {a}");
            item.HasKey("BRANCHID", a => sql += $" and C.BRANCHID = '{a}'");
            item.HasKey("STATUS", a => sql += $" and A.STATUS = '{a}'");
            sql += " ORDER BY  A.MAPID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<布局图状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }
        public string SaveFloorMap(FLOORMAPEntity SaveData)
        {
            var v = GetVerify(SaveData);
            if (SaveData.MAPID.IsEmpty())
            {
                SaveData.MAPID = NewINC("FLOORMAP"); 
                SaveData.STATUS = ((int)布局图状态.未审核).ToString();
            }
            else
            {
                FLOORMAPEntity mer = DbHelper.Select(SaveData);
                SaveData.VERIFY = mer.VERIFY;
                SaveData.VERIFY_NAME = mer.VERIFY_NAME;
                SaveData.VERIFY_TIME = mer.VERIFY_TIME;
            }
            SaveData.REPORTER = employee.Id;
            SaveData.REPORTER_NAME = employee.Name;
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            v.Require(a => a.MAPID);
            v.Require(a => a.FLOORID);
            v.Verify();
            using (var Tran = DbHelper.BeginTransaction())
            {
                SaveData.FLOORSHOP?.ForEach(shop =>
                {
                    GetVerify(shop).Require(a => a.SHOPCODE);
                });
                DbHelper.Save(SaveData);

                Tran.Commit();
            }
            return SaveData.MAPID;
        }

        public Tuple<dynamic, DataTable> GetFloorMapElement(FLOORMAPEntity Data)
        {
            if (Data.MAPID.IsEmpty())
            {
                throw new LogicException("请确认图纸编号!");
            }
            string sql = $@"SELECT P.*,R.REGIONID,R.NAME FLOORNAME,N.BRANCHID,N.NAME REGIONNAME,H.NAME BRANCHNAME FROM FLOORMAP P,FLOOR R,REGION N ,BRANCH H WHERE P.FLOORID=R.ID AND R.REGIONID=N.REGIONID AND N.BRANCHID=H.ID ";
            if (!Data.MAPID.IsEmpty())
                sql += (" AND MAPID= " + Data.MAPID);
            DataTable floormap = DbHelper.ExecuteTable(sql);

            floormap.NewEnumColumns<布局图状态>("STATUS", "STATUSMC");

            string sqlitem = $@"SELECT M.MAPID,M.SHOPCODE,M.SHOPID,M.P_X,M.P_Y " +
                " FROM FLOORSHOP M " +
                " where 1=1";
            if (!Data.MAPID.IsEmpty())
                sqlitem += (" and M.MAPID= " + Data.MAPID);
            DataTable floorshop = DbHelper.ExecuteTable(sqlitem);
            return new Tuple<dynamic, DataTable>(floormap.ToOneLine(), floorshop);
        }

        public Tuple<dynamic, DataTable> GetFloorMapAdjustElement(FLOORMAPEntity Data)
        {
            if (Data.MAPID.IsEmpty())
            {
                throw new LogicException("请确认图纸编号!");
            }
            string sql = $@"SELECT (CASE NVL(A.TZBJ,0) WHEN 0 THEN '' ELSE TO_CHAR(A.MAPID) END) MAPID
                            ,(CASE NVL(A.TZBJ,0) WHEN 0 THEN A.MAPID ELSE A.MAPID_OLD END) MAPID_OLD
                            , (CASE NVL(A.TZBJ,0) WHEN 0 THEN 1 ELSE A.STATUS END) STATUS
                            , A.FLOORID, A.BACKMAP, A.WIDTHS, A.LENGTHS, A.INITINATE_TIME_P, A.REPORTER
                            , A.REPORTER_NAME, A.REPORTER_TIME, A.VERIFY, A.VERIFY_NAME, A.VERIFY_TIME, A.INITINATE, A.INITINATE_NAME
                            , A.INITINATE_TIME, A.TERMINATE, A.TERMINATE_NAME, A.TERMINATE_TIME                            
                            , A.FILENAME, A.TZBJ,R.REGIONID,R.NAME FLOORNAME,N.BRANCHID,N.NAME REGIONNAME,H.NAME BRANCHNAME 
                            FROM FLOORMAP A,FLOOR R,REGION N ,BRANCH H WHERE A.FLOORID=R.ID AND R.REGIONID=N.REGIONID 
                            AND N.BRANCHID=H.ID ";
            if (!Data.MAPID.IsEmpty())
                sql += (" AND A.MAPID= " + Data.MAPID);
            DataTable floormap = DbHelper.ExecuteTable(sql);

            floormap.NewEnumColumns<布局图状态>("STATUS", "STATUSMC");

            string sqlitem = $@"SELECT M.MAPID,M.SHOPCODE,M.SHOPID,M.P_X,M.P_Y " +
                " FROM FLOORSHOP M " +
                " where 1=1";
            if (!Data.MAPID.IsEmpty())
                sqlitem += (" and M.MAPID= " + Data.MAPID);
            DataTable floorshop = DbHelper.ExecuteTable(sqlitem);
            return new Tuple<dynamic, DataTable>(floormap.ToOneLine(), floorshop);
        }
        public Tuple<dynamic, DataTable> GetFloorShowMap(FLOORMAPSHOWEntity Data)
        {
            string sql = $@"SELECT NVL(MAX(MAPID),0) MAPID FROM FLOORMAP P WHERE P.INITINATE_TIME<=SYSDATE AND NVL(P.TERMINATE_TIME,SYSDATE)<=SYSDATE ";
            if (!Data.FLOORID.IsEmpty())
                sql += (" AND FLOORID= " + Data.FLOORID);
            DataTable dtmapid = DbHelper.ExecuteTable(sql);
            int mapid = Convert.ToInt32(dtmapid.Rows[0]["MAPID"].ToString());

            if (mapid== 0)
            {
                throw new LogicException("没有符合条件的分析图纸!");
            }
            string sqlmap = $@"SELECT * FROM FLOORMAP WHERE 1=1 ";
            sqlmap += (" AND MAPID= " + mapid);
            DataTable floormap = DbHelper.ExecuteTable(sqlmap);

            string sqlitem1 = $@"select SUM(A.AMOUNT), P.CATEGORYID, Y.CATEGORYNAME || '(' || SUM(B.AREA_RENTABLE) || ')' CATEGORYNAME, Y.CATEGORYCODE
                , (SELECT COLOR FROM CATEGORYCOLOR R WHERE R.CATEGORYCODE = Y.CATEGORYCODE) COLOR
                from CONTRACT_SUMMARY A, CONTRACT_SHOP P,CATEGORY Y, SHOP B, FLOORSHOP C, FLOORMAP D
                where A.CONTRACTID = P.CONTRACTID AND A.SHOPID = P.SHOPID AND P.CATEGORYID = Y.CATEGORYID
                AND A.YEARMONTH = 201810 AND A.SHOPID = B.SHOPID AND B.CODE = C.SHOPCODE
                AND C.MAPID = D.MAPID AND B.FLOORID = D.FLOORID  ";
                sqlitem1 += (" and D.MAPID = " + mapid );
                sqlitem1 += " GROUP BY  P.CATEGORYID,Y.CATEGORYNAME,Y.CATEGORYCODE ";
            DataTable floorcategory = DbHelper.ExecuteTable(sqlitem1);

            return new Tuple<dynamic, DataTable>(floormap.ToOneLine(), floorcategory);
        }
        public Tuple<dynamic, DataTable, DataTable> GetGetFloorShowMapData(FLOORMAPSHOWEntity Data)
        {
            string sql = $@"SELECT NVL(MAX(MAPID),0) MAPID FROM FLOORMAP P WHERE TO_NUMBER(to_char(NVL(P.INITINATE_TIME,P.VERIFY_TIME),'YYYYMM'))<=" +Data.YEARMONTH 
                + @" AND TO_NUMBER(to_char(NVL(P.TERMINATE_TIME,SYSDATE),'YYYYMM'))>= " + Data.YEARMONTH  ;
                   sql += " AND FLOORID= " + Data.FLOORID;
            DataTable dtmapid = DbHelper.ExecuteTable(sql);
            int mapid = Convert.ToInt32(dtmapid.Rows[0]["MAPID"].ToString());

            if (mapid == 0)
            {
                throw new LogicException("没有符合条件的分析图纸!");
            }
            string sqlmap = $@"SELECT * FROM FLOORMAP WHERE 1=1 ";
            sqlmap += " AND MAPID= " + mapid;
            DataTable floormap = DbHelper.ExecuteTable(sqlmap);

            string sqlitem1 = $@"select SUM(A.AMOUNT), P.CATEGORYID, Y.CATEGORYNAME || '(' || SUM(B.AREA_RENTABLE) || ')' CATEGORYNAME, Y.CATEGORYCODE
                , (SELECT COLOR FROM CATEGORYCOLOR R WHERE R.CATEGORYCODE = Y.CATEGORYCODE) COLOR
                from CONTRACT_SUMMARY A, CONTRACT_SHOP P,CATEGORY Y, SHOP B, FLOORSHOP C, FLOORMAP D
                where A.CONTRACTID = P.CONTRACTID AND A.SHOPID = P.SHOPID AND P.CATEGORYID = Y.CATEGORYID
                AND A.YEARMONTH = "+Data.YEARMONTH +@" AND A.SHOPID = B.SHOPID AND B.CODE = C.SHOPCODE
                AND C.MAPID = D.MAPID AND B.FLOORID = D.FLOORID  ";
            sqlitem1 += " and D.MAPID = " + mapid;
            sqlitem1 += " GROUP BY  P.CATEGORYID,Y.CATEGORYNAME,Y.CATEGORYCODE ";
            DataTable floorcategory = DbHelper.ExecuteTable(sqlitem1);

            string sqlitem2 = $@" select A.SHOPID,A.CONTRACTID,SUM(A.AMOUNT) AMOUNT
                ,(CASE P.AREA WHEN 0 THEN 0 ELSE ROUND(SUM(A.AMOUNT)/P.AREA,2) END) AMOUNTEFFECT
                ,TO_CHAR(T.CONT_START,'YYYY-MM-DD')||'至'||TO_CHAR(T.CONT_END,'YYYY-MM-DD') AS CONT_S_E,T.OPERATERULE,T.QZRQ,T.DESCRIPTION,T1.NAME MERCHANTNAME
                ,P.AREA RENTAREA,P.CATEGORYID,Y.CATEGORYNAME,Y.CATEGORYCODE
                ,B.CODE SHOPCODE,B.RENT_STATUS,C.P_X,C.P_Y 
              ,(SELECT COLOR FROM CATEGORYCOLOR R WHERE R.CATEGORYCODE=Y.CATEGORYCODE) COLOR
              from CONTRACT_SUMMARY A,CONTRACT T,MERCHANT T1,CONTRACT_SHOP P,CATEGORY Y,SHOP B, FLOORSHOP C,FLOORMAP D 
              where A.CONTRACTID=T.CONTRACTID AND A.MERCHANTID=T1.MERCHANTID AND A.CONTRACTID=P.CONTRACTID AND A.SHOPID=P.SHOPID AND P.CATEGORYID=Y.CATEGORYID 
              AND A.YEARMONTH=" + Data.YEARMONTH + @" AND A.SHOPID=B.SHOPID AND  B.CODE=C.SHOPCODE 
              AND C.MAPID=D.MAPID AND B.FLOORID=D.FLOORID ";
            sqlitem2 += " and D.MAPID = " + mapid;
            sqlitem2 += " GROUP BY  A.SHOPID ,A.CONTRACTID,T.CONT_START,T.CONT_END,T.OPERATERULE,T.QZRQ,T.DESCRIPTION ,T1.NAME,P.AREA,P.CATEGORYID,Y.CATEGORYNAME,Y.CATEGORYCODE,B.CODE,B.RENT_STATUS,C.P_X,C.P_Y ";
            DataTable floorshop = DbHelper.ExecuteTable(sqlitem2);
            floorshop.NewEnumColumns<租用状态>("RENT_STATUS", "STATUSMC");
            floorshop.NewEnumColumns<合作方式>("OPERATERULE", "OPERATERULENAME");

            return new Tuple<dynamic, DataTable, DataTable>(floormap.ToOneLine(), floorcategory, floorshop);
        }
        /// <summary>
        /// 列表页的删除,可以批量删除
        /// </summary>
        /// <param name="DeleteData"></param>
        public void DeleteFloorMap(List<FLOORMAPEntity> DeleteData)
        {
            foreach (var map in DeleteData)
            {
                FLOORMAPEntity Data = DbHelper.Select(map);
                if (Data.STATUS == ((int)普通单据状态.审核).ToString())
                {
                    throw new LogicException("此图纸已经审核不能删除!");
                }
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                foreach (var map in DeleteData)
                {
                    DbHelper.Delete(map);
                }
                Tran.Commit();
            }
        }

        /// <summary>
        /// 详情页的审核
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public string ExecData(FLOORMAPEntity Data)
        {
            FLOORMAPEntity map = DbHelper.Select(Data);
            if (map.STATUS == ((int)布局图状态.审核).ToString())
            {
                throw new LogicException("图纸(" + Data.MAPID + ")已经审核不能再次审核!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                map.VERIFY = employee.Id;
                map.VERIFY_NAME = employee.Name;
                map.VERIFY_TIME = DateTime.Now.ToString();
                map.STATUS = ((int)布局图状态.审核).ToString();
                DbHelper.Save(map);
                Tran.Commit();
            }
            return map.MAPID;
        }

        /// <summary>
        /// 详情页的作废
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public string EliminateData(FLOORMAPEntity Data)
        {
            FLOORMAPEntity map = DbHelper.Select(Data);
            if (map.STATUS == ((int)布局图状态.作废).ToString())
            {
                throw new LogicException("图纸(" + Data.MAPID + ")已经作废不能再次作废!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                map.VERIFY = employee.Id;
                map.VERIFY_NAME = employee.Name;
                map.VERIFY_TIME = DateTime.Now.ToString();
                map.STATUS = ((int)布局图状态.作废).ToString();
                DbHelper.Save(map);
                Tran.Commit();
            }
            return map.MAPID;
        }

    }
}
