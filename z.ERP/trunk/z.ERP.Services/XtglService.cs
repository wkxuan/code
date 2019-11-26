﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using z.Encryption;
using z.ERP.Entities;
using z.ERP.Entities.Auto;
using z.ERP.Entities.Enum;
using z.ERP.Model.Vue;
using z.Exceptions;
using z.Extensions;
using z.MVC5.Results;
using z.SSO.Model;

namespace z.ERP.Services
{
    public class XtglService : ServiceBase
    {
        internal XtglService()
        {
        }
        public DataGridResult GetBrandData(SearchItem item)
        {
            string SqlyTQx = GetYtQx("C");
            string sql = $@"SELECT B.*,C.CATEGORYCODE,C.CATEGORYNAME,B.ID BRANDID FROM BRAND B,CATEGORY C where B.CATEGORYID = C.CATEGORYID ";
            if (SqlyTQx != "")
            {
                sql += " and " + SqlyTQx;
            }
            item.HasKey("ID", a => sql += $" and B.ID = {a}");
            item.HasKey("NAME", a => sql += $" and B.NAME LIKE '%{a}%'");
            item.HasKey("CATEGORYCODE", a => sql += $" and C.CATEGORYCODE LIKE '{a}%'");
            item.HasKey("ADRESS", a => sql += $" and B.ADRESS LIKE '%{a}%'");
            item.HasKey("CONTACTPERSON", a => sql += $" and B.CONTACTPERSON = '{a}'");
            item.HasKey("PHONENUM", a => sql += $" and B.PHONENUM = '{a}'");
            item.HasKey("PIZ", a => sql += $" and B.PIZ = '{a}'");
            item.HasKey("QQ", a => sql += $" and B.QQ = '{a}'");
            item.HasKey("WEIXIN", a => sql += $" and B.WEIXIN = '{a}'");
            item.HasKey("CONTRACTID", a => sql += $" and EXISTS(SELECT 1 FROM CONTRACT_BRAND D WHERE D.BRANDID=B.ID AND D.CONTRACTID={a})");
            item.HasKey("MERCHANTID", a => sql += $" and EXISTS(SELECT 1 FROM MERCHANT_BRAND D WHERE D.BRANDID=B.ID AND D.MERCHANTID={a})");
            sql += " ORDER BY ID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }
        public DataGridResult SrcORG(SearchItem item)
        {
            string sql = $@"SELECT ORGCODE,ORGNAME FROM ORG WHERE 1=1 ";
            item.HasKey("ORGCODE", a => sql += $" and ORGCODE LIKE '%{a}%'");
            item.HasKey("ORGNAME", a => sql += $" and ORGNAME LIKE '%{a}%'");
            sql += " ORDER BY  ORGCODE";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetPay(SearchItem item)
        {
            string sql = $@"SELECT P.PAYID,P.NAME FROM PAY P WHERE 1=1 ";
            item.HasKey("PAYID", a => sql += $" and P.PAYID LIKE '%{a}%'");
            item.HasKey("NAME", a => sql += $" and P.NAME LIKE '%{a}%'");
         //   item.HasKey("BRANCHID", a => sql += $" and PC.BRANCHID(+) ='{a}'");
            sql += " ORDER BY  P.PAYID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetPayElement(SearchItem item)
        {
            string sql = $@"SELECT * FROM PAY WHERE 1=1 ";
            item.HasKey("PAYID", a => sql += $" and PAYID = '{a}'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetFeeSubject(SearchItem item)
        {
            string sql = $@"SELECT TRIMID,NAME,TRIMID TERMID,TYPE FROM FEESUBJECT  WHERE 1=1";

            item.HasKey("NAME", a => sql += $" and NAME LIKE '%{a}%'");
            item.HasKey("TRIMID", a => sql += $" and TRIMID={a}");
            item.HasKey("CUSTOM", a => sql += $" and TRIMID < 1000");
            item.HasKey("PYM", a => sql += $" and PYM LIKE '%{a}%'");
            item.HasKey("TYPE", a => sql += $" and TYPE in ({a})");
            item.HasKey("ACCOUNT", a => sql += $" and ACCOUNT in ({a})");
            item.HasKey("DEDUCTION", a => sql += $" and DEDUCTION in ({a})");
            item.HasKey("SqlCondition", a => sql += $" and {a}");

            sql += " ORDER BY  TRIMID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetFeeSubjectElement(SearchItem item)
        {
            string sql = $@"SELECT * FROM FEESUBJECT WHERE 1=1 ";
            item.HasKey("TRIMID", a => sql += $" and TRIMID = '{a}'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetBranch(SearchItem item)
        {
            string sql = $@"SELECT B.*,O.ORGNAME FROM BRANCH B,ORG O WHERE B.ORGID=O.ORGID";
            item.HasKey("ID", a => sql += $" and B.ID = {a}");
            item.HasKey("NAME", a => sql += $" and B.NAME LIKE '%{a}%'");
            item.HasKey("ORGID", a => sql += $" and B.ORGID = {a}");
            item.HasKey("STATUS", a => sql += $" and B.STATUS = {a}");
            item.HasKey("AREA_BUILD_S", a => sql += $" and B.AREA_BUILD >= {a}");
            item.HasKey("AREA_BUILD_E", a => sql += $" and B.AREA_BUILD <= {a}");
            item.HasKey("CONTACT", a => sql += $" and B.CONTACT = {a}");
            sql += " ORDER BY B.ID desc";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<停用标记>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetConfig(SearchItem item)
        {
            string sql = $@"select * from CONFIG where 1=1 ";
            item.HasKey("ID", a => sql += $" and ID like '%{a}%'");
            item.HasKey("DESCRIPTION", a => sql += $" and DESCRIPTION like '%{a}%'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetFkfs(SearchItem item)
        {
            string sql = $@"select ID,NAME from FKFS where 1=1";
            item.HasKey("ID", a => sql += $" and ID = '{a}'");
            sql += "order by ID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetOperationrule(SearchItem item)
        {
            string sql = $@"select * from OPERATIONRULE where 1=1 ";
            item.HasKey("ID", a => sql += $" and ID = '{a}'");
            sql += " order by ID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetFeeRule(SearchItem item)
        {
            string sql = $@"select ID, NAME, to_char(UP_DATE) UP_DATE, PAY_CYCLE, PAY_UP_CYCLE, ADVANCE_CYCLE, VOID_FLAG, to_char(FEE_DAY) FEE_DAY from FEERULE where 1=1 ";
            item.HasKey("ID", a => sql += $" and ID = '{a}'");
            sql += "order by ID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetLateFeeRule(SearchItem item)

        {
            string sql = $@"select A.ID,A.NAME,A.DAYS,A.AMOUNTS,A.RATIO*100 RATIO from LATEFEERULE A where 1=1 ";
            item.HasKey("ID", a => sql += $" and A.ID = '{a}'");
            sql += "order by A.ID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetLateFeeRuleElement(SearchItem item)
        {
            string sql = $@"select A.ID,A.NAME,A.DAYS,A.AMOUNTS,A.RATIO*100 RATIO from LATEFEERULE A where 1=1 ";
            item.HasKey("ID", a => sql += $" and A.ID = '{a}'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetFloor(SearchItem item)
        {
            string sql = $@"SELECT A.ID,A.CODE,A.NAME FROM FLOOR A WHERE 1=1";
            sql += " AND A.BRANCHID IN (" + GetPermissionSql(PermissionType.Branch) + ")";    //分店权限 by：DZK
            item.HasKey("ID,", a => sql += $" and A.ID = {a}");
            item.HasKey("CODE,", a => sql += $" and A.CODE = '{a}'");
            item.HasKey("NAME", a => sql += $" and A.NAME = '{a}'");
            item.HasKey("BRANCHID", a => sql += $" and A.BRANCHID = {a}");
            sql += " ORDER BY  A.ID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetFloorElement(SearchItem item)
        {
            string sql = $@"select A.*,B.ORGIDCASCADER from FLOOR A,ORG B where A.ORGID=B.ORGID(+) ";
            sql += " AND A.BRANCHID IN (" + GetPermissionSql(PermissionType.Branch) + ")";    //分店权限 by：DZK
            item.HasKey("ID", a => sql += $" and A.ID = {a}");
            item.HasKey("CODE,", a => sql += $" and A.CODE = '{a}'");
            item.HasKey("NAME", a => sql += $" and A.NAME = '{a}'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetShop(SearchItem item)
        {
            string SqlyTQx = "";
            string yTQx = GetPermissionSql(PermissionType.Category);
            DataTable yTdt = DbHelper.ExecuteTable(yTQx);
            for (var i = 0; i <= yTdt.Rows.Count - 1; i++)
            {
                if (i == 0)
                {
                    SqlyTQx = "( B.CATEGORYCODE LIKE '" + yTdt.Rows[i][0].ToString() + "%'";
                }
                else
                {
                    SqlyTQx += " or B.CATEGORYCODE LIKE '" + yTdt.Rows[i][0].ToString() + "%'";
                }
                if (i == yTdt.Rows.Count - 1)
                {
                    SqlyTQx += ")";
                }
            }
            string sql = $@"SELECT  A.*,A.CODE SHOPCODE,A.AREA_BUILD AREA,B.CATEGORYCODE,B.CATEGORYNAME,D.NAME BRANCHNAME,F.NAME FLOORNAME " +
                   "  FROM SHOP A,CATEGORY B,ORG C,BRANCH D,FLOOR F "
                   + " WHERE  A.CATEGORYID = B.CATEGORYID(+) and A.ORGID=C.ORGID(+)"
                   + " and F.REGIONID in (" + GetPermissionSql(PermissionType.Region) + " )"
                   + " and  A.BRANCHID = D.ID and A.FLOORID=F.ID";
            if (SqlyTQx != "")
            {
                sql += " and " + SqlyTQx;
            }

            item.HasKey("CODE", a => sql += $" and A.CODE like '%{a}%'");
            item.HasKey("NAME", a => sql += $" and A.NAME like '%{a}%'");
            item.HasKey("BRANCHID", a => sql += $" and A.BRANCHID = {a}");
            item.HasKey("REGIONID", a => sql += $" and A.REGIONID = {a}");
            item.HasKey("FLOORID", a => sql += $" and A.FLOORID = {a}");
            item.HasKey("STATUS", a => sql += $" and A.STATUS = {a}");
            item.HasKey("SqlCondition", a => sql += $" and {a}");
            item.HasKey("RENT_STATUS", a => sql += $" and A.RENT_STATUS = {a}");

            sql += " ORDER BY  A.CODE";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetShopElement(SearchItem item)
        {
            string SqlyTQx = "";
            string yTQx = GetPermissionSql(PermissionType.Category);
            DataTable yTdt = DbHelper.ExecuteTable(yTQx);
            for (var i = 0; i <= yTdt.Rows.Count - 1; i++)
            {
                if (i == 0)
                {
                    SqlyTQx = "( B.CATEGORYCODE LIKE '" + yTdt.Rows[i][0].ToString() + "%'";
                }
                else
                {
                    SqlyTQx += " or B.CATEGORYCODE LIKE '" + yTdt.Rows[i][0].ToString() + "%'";
                }
                if (i == yTdt.Rows.Count - 1)
                {
                    SqlyTQx += ")";
                }
            }
            string sql = $@"SELECT  A.*,B.CATEGORYCODE,B.CATEGORYNAME,B.CATEGORYIDCASCADER,A.AREA_BUILD AREA,C.ORGIDCASCADER " +
                   "  FROM SHOP A,CATEGORY B,ORG C WHERE  A.CATEGORYID = B.CATEGORYID(+) "
                   + " and A.ORGID=C.ORGID(+)";
            if (SqlyTQx != "")
            {
                sql += " and " + SqlyTQx;
            }
            item.HasKey("SHOPID", a => sql += $" and A.SHOPID = '{a}'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public void DeleteShop(List<SHOPEntity> DefineDelete)
        {
            foreach (var item in DefineDelete)
            {
                SHOPEntity Data = DbHelper.Select(item);
                if (Data.RENT_STATUS == ((int)租用状态.出租).ToString())
                {
                    throw new LogicException("已经出租不能删除!");
                }
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                foreach (var item in DefineDelete)
                {
                    DbHelper.Delete(item);
                }
                Tran.Commit();
            }
        }
        public DataGridResult GetEnergyFiles(SearchItem item)
        {
            string sql = $@"SELECT A.*,B.CODE SHOPCODE FROM ENERGY_FILES A,SHOP B WHERE A.SHOPID=B.SHOPID(+)";
            item.HasKey("FILECODE", a => sql += $" and A.FILECODE LIKE '%{a}%'");
            item.HasKey("FILENAME", a => sql += $" and A.FILENAME LIKE '%{a}%'");
            sql += " ORDER BY  A.FILEID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetEnergyFilesElement(SearchItem item)
        {
            string sql = $@"SELECT A.*,B.CODE SHOPCODE FROM ENERGY_FILES A,SHOP B WHERE A.SHOPID=B.SHOPID(+)";
            item.HasKey("FILEID", a => sql += $" and A.FILEID = '{a}'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetPeriod(SearchItem item)
        {
            string sql = $@"select YEARMONTH,to_char(DATE_START,'yyyy-mm-dd') DATE_START,to_char(DATE_END,'yyyy-mm-dd')" +
                " DATE_END from PERIOD where 1=1";
            item.HasKey("YEAR", a => sql += $" and substr(YEARMONTH,1,4) = '{a}'");
            sql += "order by YEARMONTH";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetStaion(SearchItem item)
        {
            string sql = $@"select S.*,P.CODE SHOPCODE ,B.NAME BRANCHNAME from STATION S,SHOP P,BRANCH B where S.BRANCHID=B.ID AND S.SHOPID=P.SHOPID(+) ";
            sql += " AND S.BRANCHID IN (" + GetPermissionSql(PermissionType.Branch) + ")";    //分店权限 by：DZK
            item.HasKey("STATIONBH", a => sql += $" and STATIONBH = '{a}'");
            item.HasKey("BRANCHID", a => sql += $" and S.BRANCHID = '{a}'");
            sql += "order by STATIONBH";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public Tuple<dynamic, DataTable> GetStaionElement(STATIONEntity Data)
        {
            string sql = $@"select S.STATIONBH,S.TYPE,S.IP,S.SHOPID,S.NETWORK_NODE_ADDRESS,P.CODE SHOPCODE ";
            sql += " from STATION S,SHOP P where S.SHOPID=P.SHOPID(+) ";
            sql += " AND S.BRANCHID IN (" + GetPermissionSql(PermissionType.Branch) + ")";    //分店权限 by：DZK
            if (!Data.STATIONBH.IsEmpty())
                sql += $" and S.STATIONBH = '{ Data.STATIONBH}'";

            sql += " order by S.STATIONBH";


            DataTable station = DbHelper.ExecuteTable(sql);

            var sqlpay = $@"SELECT S.STATIONBH,S.PAYID,P.NAME from STATION_PAY S,PAY P WHERE S.PAYID=P.PAYID ";
            if (!Data.STATIONBH.IsEmpty())
                sqlpay += $" and STATIONBH = '{Data.STATIONBH.ToString()}'";
            sqlpay += " order by S.PAYID";
            DataTable pay = DbHelper.ExecuteTable(sqlpay);

            return new Tuple<dynamic, DataTable>(station.ToOneLine(), pay);
        }
        /*    public object GetStaionElement(STATIONEntity DefineSave)
            {
                string sql = $@"select STATIONBH,TYPE,IP,SHOPID from STATION where 1=1 ";
                sql += " and STATIONBH = " + DefineSave.STATIONBH.ToString();
                sql += "order by STATIONBH";


                DataTable dt = DbHelper.ExecuteTable(sql);

                var sqlpay = $@"SELECT S.STATIONBH,S.PAYID,P.NAME from STATION_PAY S,PAY P WHERE S.PAYID=P.PAYID ";
                sqlpay += " and STATIONBH = " + DefineSave.STATIONBH.ToString();
                sqlpay += " order by S.PAYID";
                DataTable dt1 = DbHelper.ExecuteTable(sqlpay);

                var result = new
                {
                    staion = dt,
                    station_pay = new dynamic[]
                    {
                        dt1
                    }
                };
                return result;
            }  */
        public object GetStaionPayList()
        {
            string sql = $@"SELECT PAYID,NAME FROM PAY  ";
            sql += "order by PAYID";

            DataTable dt = DbHelper.ExecuteTable(sql);

            var result = new
            {
                pay = dt
            };
            return result;
        }
        public string SaveSataion(STATIONEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            if (DefineSave.STATIONBH.IsEmpty())
                DefineSave.STATIONBH = CommonService.NewINC("STATION").PadLeft(5, '0');
            v.Require(a => a.STATIONBH);
            v.Require(a => a.BRANCHID);
            v.Require(a => a.TYPE);
            //  v.Require(a => a.IP);
            //  v.IsUnique(a => a.IP);

            //生成终端密钥，调用销售数据采集接口时用
            if (DefineSave.Encryption.IsEmpty())
                DefineSave.Encryption = MD5Encryption.Encrypt($"z.DGS.LoginSalt{DefineSave.STATIONBH }");


            DefineSave.STATION_PAY?.ForEach(sdb =>
            {
                GetVerify(sdb).Require(a => a.PAYID);
            });

            v.Verify();
            using (var tran = DbHelper.BeginTransaction())
            {
                DbHelper.Save(DefineSave);
                tran.Commit();
            }


            return DefineSave.STATIONBH;
        }
        public void DeleteStation(STATIONEntity DeleteData)
        {
            var v = GetVerify(DeleteData);
            string sql = $@"delete from STATION_PAY where  STATIONBH=" + DeleteData.STATIONBH.ToString();
            DbHelper.ExecuteNonQuery(sql);
            DbHelper.Delete(DeleteData);
        }

        public virtual UIResult TreeCategoryData(SearchItem item)
        {
            string sql = $@"select C.CATEGORYID,C.CATEGORYCODE,C.CATEGORYNAME,C.LEVEL_LAST,C.CATEGORYIDCASCADER,NVL(C.COLOR,'') COLOR from CATEGORY C where 1=1 ";
            item.HasKey("code", a => sql += $" and CATEGORYCODE = '{a}' ");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public virtual UIResult TreeCategoryList()
        {
            List<CATEGORYEntity> p = DbHelper.SelectList(new CATEGORYEntity()).OrderBy(a => a.CATEGORYCODE).ToList();
            return new UIResult(TreeModel.Create(p,
                a => a.CATEGORYCODE,
                a => new TreeModel()
                {
                    code = a.CATEGORYCODE,
                    title = a.CATEGORYCODE + " " + a.CATEGORYNAME,
                    expand = true
                })?.ToArray());
        }
        public virtual UIResult TreeOrgData(SearchItem item)
        {
            string sql = $@"select * from ORG where 1=1 ";
            item.HasKey("code", a => sql += $" and ORGCODE = '{a}' ");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public virtual UIResult TreeOrgList()
        {
            List<ORGEntity> p = DbHelper.SelectList(new ORGEntity()).OrderBy(a => a.ORGCODE).ToList();
            return new UIResult(TreeModel.Create(p,
                a => a.ORGCODE,
                a => new TreeModel()
                {
                    code = a.ORGCODE,
                    title = a.ORGCODE + " " + a.ORGNAME,
                    expand = true
                })?.ToArray());
        }
        public virtual UIResult TreeGoodsKindData(SearchItem item)
        {
            string sql = $@"select * from GOODS_KIND where 1=1 ";
            item.HasKey("code", a => sql += $" and CODE = '{a}' ");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public virtual UIResult TreeGoodsKindList()
        {
            List<GOODS_KINDEntity> p = DbHelper.SelectList(new GOODS_KINDEntity()).OrderBy(a => a.CODE).ToList();
            return new UIResult(TreeModel.Create(p,
                a => a.CODE,
                a => new TreeModel()
                {
                    code = a.CODE,
                    title = a.CODE + " " + a.NAME,
                    expand = true
                })?.ToArray());
        }
        public void Org_Update(string ID, int BRANCHID)
        {
            ORGEntity Data = new ORGEntity
            {
                ORGID = ID
            };
            ORGEntity org = DbHelper.Select(Data);
            string sqlUpdate = $@"update ORG set BRANCHID=" + BRANCHID + " where  ORGCODE like '" + org.ORGCODE + "%'";
            DbHelper.ExecuteNonQuery(sqlUpdate);
        }
        public string Org_BRANCHID(string CODE)
        {
            var sql = " select BRANCHID from ORG where ORGCODE='" + CODE + "'";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt.Rows[0][0].ToString();
        }
        public object GetBrandElement(BRANDEntity Data)
        {
            string sql = $@"select A.*,B.CATEGORYIDCASCADER,B.CATEGORYNAME from BRAND A,CATEGORY B where A.CATEGORYID=B.CATEGORYID ";
            if (!Data.ID.IsEmpty())
                sql += (" and ID= " + Data.ID);
            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");

            var result = new
            {
                main = dt,
            };
            return result;
        }
        public Tuple<dynamic> GetBrandDetail(BRANDEntity Data)
        {
            string sql = $@"select * from BRAND where 1=1 ";
            if (!Data.ID.IsEmpty())
                sql += (" and ID= " + Data.ID);
            DataTable dt = DbHelper.ExecuteTable(sql);

            return new Tuple<dynamic>(dt.ToOneLine());
        }
        public string BrandExecData(BRANDEntity Data)
        {
            BRANDEntity brand = DbHelper.Select(Data);
            if (brand.STATUS == ((int)普通单据状态.审核).ToString())
            {
                throw new LogicException("品牌(" + Data.NAME + ")已经审核不能再次审核!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                brand.VERIFY = employee.Id;
                brand.VERIFY_NAME = employee.Name;
                brand.VERIFY_TIME = DateTime.Now.ToString();
                brand.STATUS = ((int)普通单据状态.审核).ToString();
                DbHelper.Save(brand);
                Tran.Commit();
            }
            return brand.ID;
        }
        public DataGridResult GetPosO2OWftCfg(SearchItem item)
        {
            string sql = $@"select P.*,B.NAME BRANCHNAME from POSO2OWFTCFG P,STATION S,BRANCH B where P.POSNO=S.STATIONBH AND S.BRANCHID=B.ID ";
            sql += " AND S.BRANCHID IN (" + GetPermissionSql(PermissionType.Branch) + ")";    //分店权限 by：DZK
            item.HasKey("POSNO", a => sql += $" and POSNO = '{a}'");
            item.HasKey("BRANCHID", a => sql += $" and S.BRANCHID = '{a}'");
            sql += " order by POSNO";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetFeeAccount(SearchItem item)
        {
            string sql = $@"select F.*,B.NAME BRANCHNAME from fee_account F,BRANCH B where F.BRANCHID=B.ID ";
            sql += " AND F.BRANCHID IN (" + GetPermissionSql(PermissionType.Branch) + ")";    //分店权限 by：DZK
            item.HasKey("ID", a => sql += $" and F.ID = '{a}'");
            item.HasKey("BRANCHID", a => sql += $" and F.BRANCHID = '{a}'");
            item.HasKey("NAME", a => sql += $" and F.NAME LIKE '%{a}%'");
            sql += " order by F.ID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public SHOPEntity SelectShop(string shop)
        {
            return DbHelper.Select(new SHOPEntity() { SHOPID = shop });
        }
        /// <summary>
        /// POS银联支付配置列表
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public DataGridResult GetPOSUMSCONFIG(SearchItem item)
        {
            string sql = $@"select P.*,B.NAME BRANCHNAME from POSUMSCONFIG P,STATION S,BRANCH B WHERE P.POSNO=S.STATIONBH AND B.ID=S.BRANCHID ";
            sql += " AND S.BRANCHID IN (" + GetPermissionSql(PermissionType.Branch) + ")";    //分店权限 by：DZK
            item.HasKey("POSNO", a => sql += $" and POSNO = '{a}'");
            item.HasKey("BRANCHID", a => sql += $" and S.BRANCHID = '{a}'");
            sql += " order by POSNO";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetStationList(SearchItem item)
        {
            string sql = @"select STATION.STATIONBH POSNO, STATION.TYPE,BRANCH.NAME BRANCHNAME from STATION ,BRANCH WHERE BRANCH.ID=STATION.BRANCHID ";
            sql += " AND BRANCH.ID IN (" + GetPermissionSql(PermissionType.Branch) + ")";    //分店权限 by：DZK
            item.HasKey("BRANCHID", a => sql += $" and STATION.BRANCHID= '{a}'");
            item.HasKey("POSNO", a => sql += $" and STATION.STATIONBH LIKE '%{a}%'");
            item.HasKey("POSTYPE", a => sql += $" and STATION.TYPE = '{a}'");
            item.HasKey("SqlCondition", a => sql += $" and {a}");
            sql += @" ORDER BY STATION.STATIONBH";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<POS类型>("TYPE", "TYPENAME");
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetAlert(SearchItem item)
        {

            string sql = $@"SELECT ID,MC,XSSX,SQLSTR FROM DEF_ALERT WHERE 1=1 ";
            item.HasKey("ID", a => sql += $" and ID LIKE '%{a}%'");
            item.HasKey("MC", a => sql += $" and MC LIKE '%{a}%'");
            sql += " order by XSSX";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public Tuple<dynamic, DataTable> GetAlertElement(DEF_ALERTEntity Data)
        {
            string sql = $@"select ID,MC,XSSX,SQLSTR FROM DEF_ALERT where 1=1";
            if (!Data.ID.IsEmpty())
            {
                sql += " and ID = " + Data.ID;
            }
            DataTable defalert = DbHelper.ExecuteTable(sql);


            string sqlItem = $@"select FIELDMC,CHINAMC,WIDTH,PLSX from ALERT_FIELD where 1=1";
            if (!Data.ID.IsEmpty())
            {
                sqlItem += " and ID = " + Data.ID;
            }
            sqlItem += " order by PLSX";
            DataTable defalertItem = DbHelper.ExecuteTable(sqlItem);
            return new Tuple<dynamic, DataTable>(defalert.ToOneLine(), defalertItem);
        }
        public string SaveAlert(DEF_ALERTEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            v.Require(a => a.MC);
            v.Require(a => a.SQLSTR);

            var b = false;
            b = VerifySql(DefineSave.SQLSTR);
            if (b == false)
            {
                throw new LogicException("SQL语句不正确!");
            };
            v.IsUnique(a => a.MC);
            v.Verify();
            if (DefineSave.ID.IsEmpty())
                DefineSave.ID = CommonService.NewINC("DEF_ALERT");

            DbHelper.Save(DefineSave);

            return DefineSave.ID;
        }
        public bool VerifySql(string sql)
        {
            var b = false;
            try
            {
                DataTable dt = DbHelper.ExecuteTable(sql);
                b = true;
            }
            catch
            {
                b = false;
            }
            return b;
        }
        public Tuple<DataTable, DataTable,string> GetAlertSql(DEF_ALERTEntity Data)
        {
            DEF_ALERTEntity alert = new DEF_ALERTEntity();
            alert = DbHelper.Select(new DEF_ALERTEntity() { ID = Data.ID });
            DataTable alertSql = DbHelper.ExecuteTable(alert.SQLSTR);

            string sqlItem = $@"select FIELDMC,CHINAMC,WIDTH,PLSX from ALERT_FIELD where 1=1";
            if (!Data.ID.IsEmpty())
            {
                sqlItem += " and ID = " + Data.ID;
            }
            sqlItem += " order by PLSX";
            DataTable alertCol = DbHelper.ExecuteTable(sqlItem);
            return new Tuple<DataTable, DataTable,string>(alertSql, alertCol, alert.MC);
        }
        public string SaveSplc(SPLCDEFDEntity SPLCDEFD,
            List<SPLCJDEntity> SPLCJD, List<SPLCJGEntity> SPLCJG)
        {
            var v = GetVerify(SPLCDEFD);
            v.Require(a => a.MENUID);
            if (SPLCDEFD.BILLID.IsEmpty())
            {
                SPLCDEFD.BILLID = NewINC("SPLCDEFD");
                SPLCDEFD.STATUS = ((int)审批单状态.未审核).ToString();
            }
            else
            {
                SPLCDEFDEntity con = DbHelper.Select(SPLCDEFD);
                if (con.STATUS != ((int)审批单状态.未审核).ToString())
                {
                    throw new LogicException($"审批单({SPLCDEFD.BILLID})已经不是未审核状态!");
                }
                SPLCDEFD.VERIFY = con.VERIFY;
                SPLCDEFD.VERIFY_NAME = con.VERIFY_NAME;
                SPLCDEFD.VERIFY_TIME = con.VERIFY_TIME;
            }

            //在这里查询有没有未终止的当前菜单号的审批流程,有的话给提示
            string sql = $@"select nvl(min(BILLID),0) BILLID from SPLCDEFD where";
            sql += " MENUID = " + SPLCDEFD.MENUID + " and BILLID<> " + SPLCDEFD.BILLID + " and STATUS<>3";

            DataTable billid = DbHelper.ExecuteTable(sql);


            if (billid.Rows[0][0].ToString().ToInt() != 0)
            {
                throw new LogicException($"当前菜单号有未终止的审批流程!");
            }

            SPLCDEFD.REPORTER = employee.Id;
            SPLCDEFD.REPORTER_NAME = employee.Name;
            SPLCDEFD.REPORTER_TIME = DateTime.Now.ToString();

            using (var Tran = DbHelper.BeginTransaction())
            {
                foreach (var splcjd in SPLCJD)
                {
                    splcjd.BILLID = SPLCDEFD.BILLID;
                    DbHelper.Save(splcjd);
                }
                foreach (var splcjg in SPLCJG)
                {
                    splcjg.BILLID = SPLCDEFD.BILLID;
                    DbHelper.Save(splcjg);
                }
                DbHelper.Save(SPLCDEFD);
                Tran.Commit();
            }
            return SPLCDEFD.BILLID;
        }
        public void DeleteSplc(SPLCDEFDEntity Data)
        {
            using (var Tran = DbHelper.BeginTransaction())
            {
                List<SPLCJDEntity> lcjd = new List<SPLCJDEntity>();
                lcjd = DbHelper.SelectList(new SPLCJDEntity()).
                    Where(a => (a.BILLID == Data.BILLID)).ToList();

                foreach (var item in lcjd)
                {
                    DbHelper.Delete(item);
                };
                List<SPLCJGEntity> lcjg = new List<SPLCJGEntity>();
                lcjg = DbHelper.SelectList(new SPLCJGEntity()).
                    Where(a => (a.BILLID == Data.BILLID)).ToList();

                foreach (var item in lcjg)
                {
                    DbHelper.Delete(item);
                };
                DbHelper.Delete(Data);
                Tran.Commit();
            }
        }
        public Tuple<dynamic, DataTable, DataTable> GetSplcdefdElement(SPLCDEFDEntity Data)
        {
            if (Data.BILLID.IsEmpty())
            {
                throw new LogicException("请确认单号!");
            }
            string sql = $@"SELECT A.* ";
            sql += " from SPLCDEFD A where 1=1 ";
            sql += (" AND A.BILLID= " + Data.BILLID);
            DataTable spd = DbHelper.ExecuteTable(sql);
            if (!spd.IsNotNull())
            {
                throw new LogicException("找不到审批单!");
            }
            spd.NewEnumColumns<审批单状态>("STATUS", "STATUSMC");


            string sqlspjd = $@"SELECT A.JDID,JDNAME,JDTYPE,A.ROLEID,A.JDINX,B.ROLENAME  " +
                             " FROM SPLCJD A,ROLE B WHERE A.ROLEID=B.ROLEID ";
            sqlspjd += (" and A.BILLID= " + Data.BILLID);
            sqlspjd += " ORDER BY A.JDINX";

            DataTable spjd = DbHelper.ExecuteTable(sqlspjd);

            spjd.NewEnumColumns<审批流程节点类型>("JDTYPE", "JDTYPENAME");

            string sqlspjg = $@"SELECT B.JDID,B.JGID,B.TJMC,B.JGTYPE,B.JGMC,A.JDNAME JDNAMEXS,A.JDTYPE" +
                           " FROM SPLCJD A,SPLCJG B" +
                           " WHERE A.BILLID=B.BILLID AND A.JDID=B.JDID";

            sqlspjg += (" and A.BILLID= " + Data.BILLID);
            DataTable spjg = DbHelper.ExecuteTable(sqlspjg);
            spjg.NewEnumColumns<审批结果类型>("JGTYPE", "JGTYPENAME");

            return new Tuple<dynamic, DataTable, DataTable>(
                spd.ToOneLine(),
                spjd,
                spjg
            );
        }
        public string ExecSplc(SPLCDEFDEntity Data)
        {
            var v = GetVerify(Data);
            v.Require(a => a.BILLID);
            SPLCDEFDEntity con = DbHelper.Select(Data);
            if (con.STATUS != ((int)审批单状态.未审核).ToString())
            {
                throw new LogicException($"审批单({Data.BILLID})已经不是未审核状态!");
            }

            //在这里查询有没有未终止的当前菜单号的审批流程,有的话给提示
            string sql = $@"select nvl(min(BILLID),0) BILLID from SPLCDEFD where";
            sql += " MENUID = " + con.MENUID + " and BILLID<> " + con.BILLID + " and STATUS<>3";

            DataTable billid = DbHelper.ExecuteTable(sql);


            if (billid.Rows[0][0].ToString().ToInt() != 0)
            {
                throw new LogicException($"当前菜单号有未终止的审批流程!");
            }

            con.VERIFY = employee.Id;
            con.VERIFY_NAME = employee.Name;
            con.VERIFY_TIME = DateTime.Now.ToString();
            con.STATUS = ((int)审批单状态.审核).ToString();

            using (var Tran = DbHelper.BeginTransaction())
            {
                DbHelper.Save(con);
                Tran.Commit();
            }
            return con.BILLID;
        }
        public string OverSplc(SPLCDEFDEntity Data)
        {
            var v = GetVerify(Data);
            v.Require(a => a.BILLID);
            SPLCDEFDEntity con = DbHelper.Select(Data);
            if (con.STATUS != ((int)审批单状态.审核).ToString())
            {
                throw new LogicException($"审批单({Data.BILLID})已经不是审核状态!");
            }
            con.TERMINATE = employee.Id;
            con.TERMINATE_NAME = employee.Name;
            con.TERMINATE_TIME = DateTime.Now.ToString();
            con.STATUS = ((int)审批单状态.终止).ToString();

            using (var Tran = DbHelper.BeginTransaction())
            {
                DbHelper.Save(con);
                Tran.Commit();
            }
            return con.BILLID;
        }
        public Tuple<DataTable, DataTable, int> GetSplc(SPLCEntity Data)
        {

            if (Data.MENUID.IsEmpty())
            {
                throw new LogicException("请确认查找审批流程的菜单号信息!");
            }
            //查找菜单对应的审批流程数据
            //应该所有的流程都能看得见
            string sql = $@"select JDID,JDNAME from";
            sql += " SPLCDEFD A,SPLCJD B WHERE A.BILLID=B.BILLID AND A.STATUS=2 ";
            sql += " AND A.MENUID= " + Data.MENUID;
            DataTable splcjd = DbHelper.ExecuteTable(sql);

            //查找当前记录编号审批流程执行到哪个节点
            //找最后一个审批数据
            var curJdid = 1;
            string sql1 = $@"select JDID from";
            sql1 += " SPLCJG_MENU WHERE 1=1 ";
            sql1 += " AND MENUID= " + Data.MENUID;
            sql1 += " AND BILLID= " + Data.JLBH;
            sql1 += " order by CLSJ desc";
            DataTable spBillJg = DbHelper.ExecuteTable(sql1);
            if (spBillJg.Rows.Count > 0)
            {
                curJdid = spBillJg.Rows[0][0].ToString().ToInt();
            }

            string sqlxz = $@"select JDID,JGID,JGTYPE,JGMC from";
            sqlxz += " SPLCDEFD A,SPLCJG B WHERE A.BILLID=B.BILLID AND A.STATUS=2 ";

            sqlxz += " AND A.MENUID= " + Data.MENUID;
            sqlxz += " AND B.JDID= " + curJdid;
            DataTable splcjg = DbHelper.ExecuteTable(sqlxz);
            splcjg.NewEnumColumns<审批结果类型>("JGTYPE", "JGTYPENAME");

            return new Tuple<DataTable, DataTable, int>(
                 splcjd,
                 splcjg,
                 curJdid
            );
        }

        public bool SrchSplcQx(int MENUID, int CURJDID)
        {
            var result = false;

            string sqldef = $@"select JDID,JDNAME from";
            sqldef += " SPLCDEFD A,SPLCJD B WHERE A.BILLID=B.BILLID AND A.STATUS=2 ";
            sqldef += " AND A.MENUID= " + MENUID;
            DataTable splc = DbHelper.ExecuteTable(sqldef);


            string sql = $@"select JDID,JGID,JGTYPE,JGMC from";
            sql += " SPLCDEFD A,SPLCJG B WHERE A.BILLID=B.BILLID AND A.STATUS=2 ";

            sql += " AND  EXISTS(SELECT 1 FROM SPLCJD L,SYSUSER C,USER_ROLE D";
            sql += " WHERE C.USERID = D.USERID AND L.ROLEID = D.ROLEID  AND L.JDID=B.JDID";
            sql += " AND L.BILLID = B.BILLID AND C.USERID = " + employee.Id + ")";

            sql += " AND A.MENUID='" + MENUID + "' ";
            sql += " AND B.JDID= " + CURJDID;
            DataTable splcjd = DbHelper.ExecuteTable(sql);

       
            //当有审批流程定义，但是当前节点没权限
            if ((splc.Rows.Count > 0) && (splcjd.Rows.Count == 0))
                result = true;
            return result;
        }
        public void ExecMenuSplc(SPLCMENUEntity DataInto)
        {
            var JDID = DataInto.CURJDID.ToInt() + 1;
            if (SrchSplcQx(10600200, JDID))
            {
                throw new LogicException("当前操作员无当前审批权限!");
            }
            string sqlxz = $@"select JDTYPE from";
            sqlxz += " SPLCDEFD A,SPLCJD B WHERE A.BILLID=B.BILLID AND A.STATUS=2 ";
            sqlxz += " AND A.MENUID= " + DataInto.MENUID;
            sqlxz += " AND B.JDID= " + DataInto.JGJDID;
            DataTable splcjg = DbHelper.ExecuteTable(sqlxz);
            var JDTYPE = splcjg.Rows[0][0].ToString().ToInt();


            SPLCJG_MENUEntity Data = new SPLCJG_MENUEntity();

            Data.JGTYPE = JDTYPE.ToString();
            Data.MENUID = DataInto.MENUID;
            Data.JDID = DataInto.JGJDID;
            Data.BILLID = DataInto.BILLID;
            Data.BZ = DataInto.BZ;

            if (Data.JGTYPE == ((int)审批流程节点类型.结束).ToString())
            {
                var Data1 = new CONTRACTEntity();
                Data1.CONTRACTID = Data.BILLID;
                var res = HtglService.GetContractElement(Data1);
                Data1.HTLX = res.Item1.HTLX;
                Data1.STATUS = res.Item1.STATUS;
                HtglService.ExecData(Data1);
            }

            var v = GetVerify(Data);
            v.Require(a => a.BILLID);
            v.Require(a => a.MENUID);
            v.Require(a => a.JDID);
            v.Require(a => a.BZ);
            Data.CLSJ = DateTime.Now.ToString();
            Data.XH = NewINC("SPLCJG_MENUE");
            DbHelper.Save(Data);
        }
        //通知消息列表
        public DataGridResult GetNOTICE(SearchItem item)
        {
            string sql = @"select * from NOTICE N where exists(select 1 from NOTICE_BRANCH WHERE BRANCHID IN (" + GetPermissionSql(PermissionType.Branch) + ") AND N.ID=NOTICE_BRANCH.NOTICEID) ";
            item.HasKey("ID", a => sql += $" and ID = '{a}'");
            item.HasKey("TITLE", a => sql += $" and TITLE LIKE '%{a}%'");
            item.HasKey("STATUS", a => sql += $" and STATUS = '{a}'");
            item.HasKey("BRANCHID", a => sql += $"AND exists(select 1 from NOTICE_BRANCH WHERE BRANCHID ='{a}' AND N.ID=NOTICE_BRANCH.NOTICEID)");
            sql += " order by ID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<通知状态>("STATUS", "STATUSNAME");
            return new DataGridResult(dt, count);
        }
        //通知消息列表
        public Tuple<dynamic, DataTable> GetNOTICEElement(NOTICEEntity item)
        {
            string sql = @"select * from NOTICE where 1=1 ";
            if (!string.IsNullOrEmpty(item.ID))
            {
                sql += $" and ID =" + item.ID + " ";
            }
            sql += " order by ID DESC";
            DataTable dt = DbHelper.ExecuteTable(sql);
            string sql1 = @" select B.ID,B.NAME from NOTICE_BRANCH N,BRANCH B where B.ID=N.BRANCHID  ";
            if (!string.IsNullOrEmpty(item.ID))
            {
                sql1 += $" and N.NOTICEID =" + item.ID + " ";
            }
            DataTable branch = DbHelper.ExecuteTable(sql1);
            return new Tuple<dynamic, DataTable>(dt.ToOneLine(), branch);
        }
        //消息详情
        public DataTable GetNOTICEInfo(string id)
        {
            string sql = @"select N.*,TO_CHAR(N.VERIFY_TIME,'yyyy-MM-dd') release_time from NOTICE N where STATUS=2 ";
            if (!string.IsNullOrEmpty(id))
            {
                sql += @" and id=" + id + "";
            }
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt;
        }
        /// <summary>
        /// 保存通知消息
        /// </summary>
        /// <param name="DefineSave"></param>
        /// <returns></returns>
        public string SaveNotice(NOTICEEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            if (DefineSave.ID.IsEmpty())
                DefineSave.ID = NewINC("NOTICE");
            //如果状态为2，保存 发布人信息
            if (DefineSave.STATUS == "2")
            {
                DefineSave.VERIFY = employee.Id;
                DefineSave.VERIFY_NAME = employee.Name;
                DefineSave.VERIFY_TIME = DateTime.Now.ToString();
            }
            DefineSave.REPORTER = employee.Id;
            DefineSave.REPORTER_NAME = employee.Name;
            DefineSave.REPORTER_TIME = DateTime.Now.ToString();
            v.Require(a => a.ID);
            v.Require(a => a.TITLE);
            v.Require(a => a.STATUS);
            v.Require(a => a.CONTENT);
            v.Verify();
            using (var Tran = DbHelper.BeginTransaction())
            {
                DbHelper.Save(DefineSave);
                Tran.Commit();
            }
            return DefineSave.ID;
        }
        public void NoticeRead(string noticeid)
        {
            READNOTOCELOGEntity rnl = new READNOTOCELOGEntity();
            rnl.NOTICEID = noticeid;
            rnl.USERID = employee.Id;
            var v = GetVerify(rnl);
            v.Require(a => a.NOTICEID);
            v.Require(a => a.USERID);
            v.Verify();
            DbHelper.Save(rnl);
        }
        public DataGridResult SrchSplc(SearchItem item)
        {
            string sql = @"select * from SPLCDEFD where 1=1 ";
            item.HasKey("MENUID", a => sql += $" and MENUID = '{a}'");
            item.HasKey("STATUS", a => sql += $" and STATUS = '{a}'");
            item.HasKey("REPORTER_NAME", a => sql += $" and REPORTER_NAME = '{a}'");
            item.HasKey("VERIFY_NAME", a => sql += $" and VERIFY_NAME = '{a}'");
            item.HasKey("TERMINATE_NAME", a => sql += $" and TERMINATE_NAME = '{a}'");
            sql += " order by MENUID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<审批单状态>("STATUS", "STATUSNAME");
            dt.NewEnumColumns<审批流程菜单号>("MENUID", "MENUIDMC");
            return new DataGridResult(dt, count);
        }
        public string SAVEFEESUBJECT_ACCOUNT(FEESUBJECT_ACCOUNTEntity data)
        {
            var v = GetVerify(data);
            v.Require(a => a.TERMID);
            v.Require(a => a.FEE_ACCOUNTID);
            v.Require(a => a.BRANCHID);
            v.Require(a => a.NOTICE_CREATE_WAY);
            v.Verify();
            DbHelper.Save(data);
            return data.TERMID;
        }
        public DataGridResult GetFEESUBJECT_ACCOUNT(SearchItem item)
        {
            string sql = @"SELECT FA.*,B.NAME BRANCHNAME,F.NAME TERMNAME 
                             FROM FEESUBJECT_ACCOUNT FA,BRANCH B,FEESUBJECT F  
                            WHERE FA.BRANCHID=B.ID AND FA.TERMID=F.TRIMID";
            item.HasKey("BRANCHID", a => sql += $" and FA.BRANCHID = {a}");
            item.HasKey("TERMID", a => sql += $" and FA.TERMID = {a}");
            sql += " ORDER BY  FA.BRANCHID,FA.TERMID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public UIResult GetFEESUBJECT_ACCOUNTOne(FEESUBJECT_ACCOUNTEntity data)
        {
            string sql = @"SELECT FA.*,B.NAME BRANCHNAME,F.NAME TERMNAME 
                             FROM FEESUBJECT_ACCOUNT FA,BRANCH B,FEESUBJECT F  
                            WHERE FA.BRANCHID=B.ID AND FA.TERMID=F.TRIMID";
            if (!string.IsNullOrEmpty(data.BRANCHID))
                sql += $" and FA.BRANCHID = {data.BRANCHID}";
            if (!string.IsNullOrEmpty(data.TERMID))
                sql += $" and FA.TERMID = {data.TERMID}";
            sql += " ORDER BY  FA.BRANCHID,FA.TERMID";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return new UIResult(dt);
        }
        /// <summary>
        /// 终端密钥查询
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string POSKEYSrchSql(SearchItem item)
        {
            string sql = $@" SELECT B.NAME, S.STATIONBH, S.ENCRYPTION 
                             FROM STATION S
                             FULL OUTER JOIN BRANCH B
                             ON B.ID=S.BRANCHID  
                             WHERE ENCRYPTION IS NOT NULL";
            sql += "  AND S.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            item.HasKey("BRANCHID", a => sql += $" and S.BRANCHID LIKE '%{a}%'");
            item.HasKey("STATIONBH", a => sql += $" and S.STATIONBH LIKE '%{a}%'");

            return sql;
        }

        public DataGridResult POSKEYSrch(SearchItem item)
        {
            string sql = POSKEYSrchSql(item);
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);

        }

        public DataTable POSKEYSrchOutput(SearchItem item)
        {

            string sql = POSKEYSrchSql(item);

            DataTable dt = DbHelper.ExecuteTable(sql);

            return dt;


        }
        #region 收银终端监控
        public List<STATIONMONITOR> GetSTATIONMONITOR()
        {
            List<STATIONMONITOR> STATIONMONITORList = new List<STATIONMONITOR>();
            var branch = DataService.branch();
            foreach (var item in branch)
            {
                STATIONMONITOR stationinfo = new STATIONMONITOR
                {
                    BRANCHID = item.Key,
                    BRANCHNAME = item.Value
                };
                var sdata = GETSTATIONL(stationinfo.BRANCHID);
                if (sdata.Rows.Count > 0)
                {
                    List<STATIONINFO> stationlist = new List<STATIONINFO>();
                    foreach (DataRow item1 in sdata.Rows)
                    {

                        STATIONINFO station = new STATIONINFO();
                        station.STATIONBH = item1["STATIONBH"].ToString();
                        station.REFRESHTIME = item1["REFRESH_TIME"].ToString();
                        station.GAPTIME = item1["DT"].ToString().ToInt();
                        station.CASHIERNAME = item1["CASHIERNAME"].ToString();
                        if (station.GAPTIME <= 6 && station.GAPTIME >= 0)
                        {
                            station.STATIONSTATUS = (int)收银终端状态.工作;
                        }
                        else if (station.GAPTIME < 30 && station.GAPTIME > 6)
                        {
                            station.STATIONSTATUS = (int)收银终端状态.掉网;
                        }
                        else
                        {
                            station.STATIONSTATUS = (int)收银终端状态.关机;
                        }
                        stationlist.Add(station);
                    }
                    stationinfo.STATIONINFOList = stationlist;
                }
                STATIONMONITORList.Add(stationinfo);
            }
            return STATIONMONITORList;
        }
        public DataTable GETSTATIONL(string branchid)
        {
            string sql = $@" SELECT stationbh,floor(nvl((sysdate-refresh_time),-1) * 24*60)  dt,to_char(refresh_time,'yyyy-mm-dd hh24:mi:ss') refresh_time,SYSUSER.USERNAME CASHIERNAME 
                        FROM STATION,SYSUSER where STATION.CASHIERID=SYSUSER.USERID(+) AND STATION.TYPE<>3 AND branchid = {branchid} ORDER BY stationbh";

            DataTable dt = DbHelper.ExecuteTable(sql);

            return dt;
        }
        public Tuple<dynamic, DataTable> GetSTATIONSALE(string stationid)
        {
            string sql = $@" SELECT TO_CHAR(NVL(SUM(SS.SALE_AMOUNT),0),'FM999999999990.00') BRANCHAMOUNT FROM (
                SELECT S.*,ST.BRANCHID 
                FROM SALE S,STATION ST
                WHERE S.POSNO=ST.STATIONBH AND ST.BRANCHID  = (SELECT BRANCHID FROM STATION WHERE STATION.STATIONBH = '{stationid}')) SS ";
            DataTable dt = DbHelper.ExecuteTable(sql);

            string sql1 = $@" SELECT STATIONBH,PAYID,PAYNAME,TO_CHAR(NVL(SUM(AMOUNT),0),'FM999999999990.00') AMOUNT FROM 
                    (SELECT STP.STATIONBH, PAY.PAYID ,PAY.NAME PAYNAME ,SAP.AMOUNT
                    FROM STATION_PAY STP,SALE_PAY SAP,PAY 
                    WHERE STP.STATIONBH=SAP.POSNO(+) AND  STP.PAYID=SAP.PAYID(+) AND STP.PAYID=PAY.PAYID(+) AND STP.STATIONBH={stationid}
                    )
                    GROUP BY STATIONBH,PAYNAME,PAYID
                    ORDER BY PAYID";

            DataTable dti = DbHelper.ExecuteTable(sql1);
            if (dti.Rows.Count > 0)
            {
                var sqlsum = "SELECT TO_CHAR(NVL(SUM(AMOUNT),0),'FM999999999990.00') AMOUNT FROM ( " + sql1 + ")";
                DataTable dtSum = DbHelper.ExecuteTable(sqlsum);
                //交易金额汇总                
                decimal stsum = Convert.ToDecimal(dtSum.Rows[0]["AMOUNT"]);
                DataRow dr = dti.NewRow();
                dr["PAYNAME"] = "合计";
                dr["AMOUNT"] = stsum.ToString();

                dti.Rows.Add(dr);
            }

            return new Tuple<dynamic, DataTable>(dt.ToOneLine(), dti);
        }
        #endregion


        /// <summary>
        /// 交易小票信息设置
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string TicketInfoSql(SearchItem item)
        {
            string sql = $@"SELECT BRANCHID, PRINTCOUNT, BRANCH.NAME, HEAD, TAIL, ADQRCODE, ADCONTENT
                            FROM TICKETINFO,BRANCH
                            WHERE BRANCH.ID=TICKETINFO.BRANCHID";
            sql += "  AND TICKETINFO.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            item.HasKey("BRANCHID", a => sql += $" and BRANCHID LIKE '%{a}%'");
            //item.HasKey("HEAD", a => sql += $" and HEAD LIKE '%{a}%'");
            //item.HasKey("TAIL", a => sql += $" and TAIL LIKE '%{a}%'");
            //item.HasKey("ADQRCODE", a => sql += $" and ADQRCODE LIKE '%{a}%'");
            //item.HasKey("ADCONTENT", a => sql += $" and ADCONTENT LIKE '%{a}%'");
            return sql;

        }
        public DataGridResult TicketInfo(SearchItem item)
        {
            string sql = TicketInfoSql(item);
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public DataTable TicketInfoDetail(SearchItem item)
        {
            string sql = TicketInfoSql(item);
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt;
        }
        public DataTable GetTicketInfo(TicketInfoEntity data)
        {
            string sql = $@"SELECT BRANCHID, PRINTCOUNT, BRANCH.NAME, HEAD, TAIL, ADQRCODE, ADCONTENT
                            FROM TICKETINFO,BRANCH
                            WHERE BRANCH.ID=TICKETINFO.BRANCHID";
            sql += "  AND TICKETINFO.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            sql += " and TICKETINFO.BRANCHID=" + data.BRANCHID;
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt;
        }
        /// <summary>
        /// 收银员查询
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public DataGridResult CashierSearch(SearchItem item)
        {
            string sql = $@"SELECT S.STATIONBH,B.ID BRANCHID,B.NAME BRANCHNAME ,SHOP.SHOPID,SHOP.CODE SHOPCODE,SYSUSER.USERCODE,SYSUSER.USERNAME
                        FROM STATION S,BRANCH B,SHOP ,SYSUSER
                        WHERE S.BRANCHID=B.ID AND S.SHOPID=SHOP.SHOPID AND SYSUSER.SHOPID =S.SHOPID AND S.TYPE IN (1,2) ";
            sql += "  AND S.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            item.HasKey("BRANCHID", a => sql += $" and S.BRANCHID = '{a}'");
            item.HasKey("STATIONBH", a => sql += $" and S.STATIONBH = '{a}'");
            item.HasKey("USERID", a => sql += $" and SYSUSER.USERID = '{a}'");
            item.HasKey("SHOPID", a => sql += $" and S.SHOPID = '{a}'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        /// <summary>
        /// BrandImport
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public ImportMsg BrandImport(DataTable dt)
        {
            var SaveDataList = new List<BRANDEntity>();
            var data = VerifyImportBrand(dt, ref SaveDataList);
            if (data.SuccFlag)
            {
                using (var Tran = DbHelper.BeginTransaction())
                {
                    for (var i = 0; i < SaveDataList.Count; i++)
                    {
                        BrandPro(SaveDataList[i]);
                    }

                    Tran.Commit();
                }
            }
            return data;
        }

        public string BrandPro(BRANDEntity SaveData)
        {
            var v = GetVerify(SaveData);
            if (SaveData.ID.IsEmpty())
                SaveData.ID = CommonService.NewINC("BRAND");
            SaveData.STATUS = "1";
            SaveData.REPORTER = employee.Id;
            SaveData.REPORTER_NAME = employee.Name;
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            v.Require(a => a.ID);
            v.Require(a => a.NAME);
            v.Require(a => a.CATEGORYID);
            v.IsNumber(a => a.ID);
            v.IsNumber(a => a.CATEGORYID);
            v.IsUnique(a => a.ID);
            v.IsUnique(a => a.NAME);
            v.Verify();
            DbHelper.Save(SaveData);
            return SaveData.ID;
        }

        public string SaveBrand(BRANDEntity SaveData)
        {
            BrandPro(SaveData);
            //using (var Tran = DbHelper.BeginTransaction())
            //{
            //    BrandPro(SaveData);

            //    Tran.Commit();
            //}
            return SaveData.ID;
        }


        public ImportMsg VerifyImportBrand(DataTable dt, ref List<BRANDEntity> SaveDataList)
        {
            var backData = new ImportMsg();
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var SaveData = new BRANDEntity();

                if (dt.Columns.Contains("名称"))
                {
                    var mc = dt.Rows[i]["名称"].ToString();
                    if (string.IsNullOrEmpty(mc))
                    {
                        backData.Message = $@"第{i + 1}行的名称不能为空！";
                        backData.SuccFlag = false;
                        return backData;
                    }
                    SaveData.NAME = mc;
                }
                else
                {
                    backData.Message = "导入项没有名称！";
                    backData.SuccFlag = false;
                    return backData;
                }
                if (dt.Columns.Contains("业态代码"))
                {
                    var yt = dt.Rows[i]["业态代码"].ToString();
                    if (string.IsNullOrEmpty(yt))
                    {
                        backData.Message = $@"第{i + 1}行的业态不能为空！";
                        backData.SuccFlag = false;
                        return backData;
                    }
                    SaveData.CATEGORYID = GetTableDataKey("CATEGORY", "CATEGORYCODE", yt, "CATEGORYID");
                    if (string.IsNullOrEmpty(SaveData.CATEGORYID))
                    {
                        backData.Message = $@"第{i + 1}行的业态代码{yt}不存在！";
                        backData.SuccFlag = false;
                        return backData;
                    }
                }
                else
                {
                    backData.Message = "导入项没有业态！";
                    backData.SuccFlag = false;
                    return backData;
                }
                if (dt.Columns.Contains("地址"))
                {
                    var dz = dt.Rows[i]["地址"].ToString();
                    SaveData.ADRESS = dz;
                }
                if (dt.Columns.Contains("联系人"))
                {
                    var lxr = dt.Rows[i]["联系人"].ToString();
                    SaveData.CONTACTPERSON = lxr;
                }
                if (dt.Columns.Contains("电话"))
                {
                    var dh = dt.Rows[i]["电话"].ToString();
                    SaveData.PHONENUM = dh;
                }
                if (dt.Columns.Contains("微信"))
                {
                    var wx = dt.Rows[i]["微信"].ToString();
                    SaveData.WEIXIN = wx;
                }
                if (dt.Columns.Contains("邮编"))
                {
                    var yb = dt.Rows[i]["邮编"].ToString();
                    SaveData.PIZ = yb;
                }
                if (dt.Columns.Contains("QQ"))
                {
                    var qq = dt.Rows[i]["QQ"].ToString();
                    SaveData.QQ = qq;
                }
                SaveDataList.Add(SaveData);
            }
            return backData;

        }

        #region 收款方式手续费
        public DataGridResult GetPay_Charges(SearchItem item)
        {
            string sql = $@"SELECT PC.BRANCHID,PC.PAYID,PC.RATE*1000 RATE,PC.FLOOR,PC.CEILING,P.NAME PAYNAME FROM PAY P,PAY_CHARGES PC WHERE PC.PAYID=P.PAYID AND P.TYPE>3 AND P.TYPE<20";
            item.HasKey("BRANCHID", a => sql += $" and PC.BRANCHID = '{a}'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public object GetPAY()
        {
            string sql = $@"SELECT * FROM PAY WHERE VOID_FLAG=1 AND TYPE>3 AND TYPE<20 ORDER BY  PAYID ";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return new
            {
                dt = dt
            };
        }
        public UIResult GetPay_ChargesOne(PAY_CHARGESEntity data)
        {
            string sql = $@"SELECT PC.BRANCHID,PC.PAYID,PC.RATE*1000 RATE,PC.FLOOR,PC.CEILING FROM PAY_CHARGES PC WHERE 1=1  ";
            if (!string.IsNullOrEmpty(data.BRANCHID)) {
                sql += $@" AND PC.BRANCHID={data.BRANCHID} ";
            }
            if (!string.IsNullOrEmpty(data.PAYID))
            {
                sql += $@" AND PC.PAYID={data.PAYID} ";
            }
            DataTable dt = DbHelper.ExecuteTable(sql);
            return new UIResult(dt);
        }
        public string PAY_CHARGESSAVE(PAY_CHARGESEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            v.Require(a => a.BRANCHID);
            v.Require(a => a.PAYID);
            v.Require(a => a.FLOOR);
            v.Require(a => a.CEILING);
            v.Require(a => a.RATE);
            if (!DefineSave.RATE.IsEmpty())
            {
                DefineSave.RATE = (DefineSave.RATE.ToDouble().ToString("N").ToDouble() / 1000).ToString();
            }
            DbHelper.Save(DefineSave);

            return DefineSave.PAYID;
        }
        #endregion
       
        /// <summary>
        /// 手续费对账查询
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public DataGridResult ChargesSearch(SearchItem item)
        {
            string sql = $@"SELECT B.NAME BRANCHNAME,A.POSNO,A.DEALID,A.SALE_TIME,PAY.NAME PAYNAME,AP.AMOUNT,AP.CHARGES
                        FROM ALLSALE A,ALLSALE_PAY AP,STATION S,BRANCH B,PAY 
                        WHERE A.POSNO=AP.POSNO AND A.DEALID=AP.DEALID
                           AND A.POSNO=S.STATIONBH AND S.BRANCHID=B.ID
	                         AND PAY.PAYID=AP.PAYID ";
            sql += "  AND S.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            item.HasKey("BRANCHID", a => sql += $" and S.BRANCHID = '{a}'");
            item.HasKey("STATIONBH", a => sql += $" and A.POSNO = '{a}'");
            item.HasKey("DEALID", a => sql += $" and A.DEALID = '{a}'");
            item.HasKey("PAY", a => sql += $" and PAY.PAYID IN ({a})");
            item.HasDateKey("RQ_START", a => sql += $" and TRUNC(A.SALE_TIME) >= {a}");
            item.HasDateKey("RQ_END", a => sql += $" and TRUNC(A.SALE_TIME) <= {a}");
            sql += " ORDER BY SALE_TIME DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
    }
}
