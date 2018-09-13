using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using z.ERP.Entities;
using z.ERP.Entities.Enum;
using z.ERP.Entities.Procedures;
using z.ERP.Model.Vue;
using z.Exceptions;
using z.Extensions;
using z.MVC5.Results;

namespace z.ERP.Services
{
    public class SpglService: ServiceBase
    {
        internal SpglService()
        {

        }

        public DataGridResult GetGoods(SearchItem item)
        {
            string sql = $@"SELECT G.*,K.CODE KINDCODE,K.NAME KINDNAME,M.NAME MERCHANTNAME FROM GOODS G,GOODS_KIND K,MERCHANT M";
            sql += " WHERE G.MERCHANTID=M.MERCHANTID(+) AND G.KINDID=K.ID(+)";
            item.HasKey("GOODSDM", a => sql += $" and G.GOODSDM = '{a}'");
            item.HasKey("BARCODE", a => sql += $" and G.BARCODE={a}");
            item.HasKey("NAME", a => sql += $" and G.NAME  LIKE '%{a}%'");
            item.HasKey("PYM", a => sql += $" and G.PYM  LIKE '%{a}%'");
            item.HasKey("CONTRACTID", a => sql += $" and G.CONTRACTID={a}");
            item.HasKey("MERCHANTID", a => sql += $" and G.MERCHANTID={a}");
            item.HasKey("KINDID", a => sql += $" and G.KINDID={a}");
            item.HasKey("TYPE", a => sql += $" and G.TYPE={a}");
            sql += " ORDER BY  GOODSDM DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<商品状态>("STATUS", "STATUSMC");
            dt.NewEnumColumns<商品类型>("TYPE", "TYPEMC");
            return new DataGridResult(dt, count);
        }

        public Tuple<dynamic, DataTable> GetGoodsDetail(GOODSEntity Data)
        {
            string sql = $@"select * from GOODS where 1=1 ";
            if (!Data.GOODSID.IsEmpty())
                sql += (" and GOODSID= " + Data.GOODSID);
            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.NewEnumColumns<商品状态>("STATUS", "STATUSMC");
            dt.NewEnumColumns<商品类型>("TYPE", "TYPEMC");
            dt.NewEnumColumns<核算方式>("STYLE", "STYLEMC");
            dt.Rows[0]["JXSL"] = dt.Rows[0]["JXSL"].ToString().ToDouble() * 100;
            dt.Rows[0]["XXSL"] = dt.Rows[0]["XXSL"].ToString().ToDouble() * 100;
            string sqlshop = $@"SELECT G.*,S.CODE,Y.CATEGORYCODE,Y.CATEGORYNAME " +
                "  FROM GOODS_SHOP G,SHOP S,CATEGORY Y  " +
                "  WHERE G.SHOPID=S.SHOPID AND G.CATEGORYID= Y.CATEGORYID";
            if (!Data.GOODSID.IsEmpty())
                sqlshop += (" and G.GOODSID= " + Data.GOODSID);
            DataTable dtshop = DbHelper.ExecuteTable(sqlshop);

            return new Tuple<dynamic, DataTable>(dt.ToOneLine(), dtshop);
        }

        public string SaveGoods (GOODSEntity SaveData)
        {
            var v = GetVerify(SaveData);
            var spbmcd = 6;
            //定义随机数,条码末尾校验位
            Random sj = new Random();
            if (SaveData.GOODSID.IsEmpty())
            {
                SaveData.GOODSID = CommonService.NewINC("GOODSID");
                SaveData.GOODSDM = CommonService.NewINC("GOODSDM").PadLeft(spbmcd, '0');
            }
            if (SaveData.BARCODE.IsEmpty())
            {
                var szFormatBarcode = "21{0:D" + spbmcd + "}{1:D" + (10 - Convert.ToInt32(spbmcd)) + "}";
                var barCode = string.Format(szFormatBarcode, SaveData.GOODSDM, 0);
                SaveData.BARCODE = (barCode + sj.Next(10)).ToString();
            }
                
            v.Require(a => a.GOODSDM);
            v.IsUnique(a => a.GOODSDM);
            v.Require(a => a.TYPE);
            v.Require(a => a.NAME);
            v.Require(a => a.BARCODE);
            v.Require(a => a.CONTRACTID);
            v.Require(a => a.MERCHANTID);
            v.Require(a => a.STYLE);
            v.Require(a => a.BRANDID);
            //v.Require(a => a.XXSL);
            v.Require(a => a.JSKL_GROUP);

            SaveData.JXSL = (SaveData.JXSL.ToDouble() / 100).ToString();
            SaveData.XXSL = (SaveData.XXSL.ToDouble() / 100).ToString();
            SaveData.REPORTER = employee.Id;
            SaveData.REPORTER_NAME = employee.Name;
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            SaveData.STATUS = ((int)商品状态.未审核).ToString();

            SaveData.GOODS_SHOP.ForEach(sdb =>
            {
                GetVerify(sdb).Require(a => a.GOODSID);
                GetVerify(sdb).Require(a => a.BRANCHID);
                GetVerify(sdb).Require(a => a.SHOPID);                
                GetVerify(sdb).Require(a => a.CATEGORYID);
            });
            v.Verify();
            DbHelper.Save(SaveData);

            return SaveData.GOODSDM;
        }
        
        public void DeleteGoods(List<GOODSEntity> DeleteData)
        {
            using (var Tran = DbHelper.BeginTransaction())
            {
                foreach (var goods in DeleteData)
                {
                    var v = GetVerify(goods);
                    //校验
                    DbHelper.Delete(goods);
                }
                Tran.Commit();
            }
        }
        public object ShowOneEdit(GOODSEntity Data)
        {
            string sql = $@" select G.*,M.NAME SHMC,D.NAME BRANDMC,C.CODE from GOODS G,MERCHANT M,GOODS_KIND C,BRAND D";
            sql += "  where G.MERCHANTID=M.MERCHANTID  AND G.KINDID=C.ID and G.BRANDID =D.ID ";
            if (!Data.GOODSID.IsEmpty())
                sql += (" and G.GOODSID= " + Data.GOODSID);
            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.Rows[0]["JXSL"] = dt.Rows[0]["JXSL"].ToString().ToDouble() * 100;
            dt.Rows[0]["XXSL"] = dt.Rows[0]["XXSL"].ToString().ToDouble() * 100;
            dt.NewEnumColumns<核算方式>("STYLE", "STYLEMC");

            string sqlshop = $@"SELECT G.*,S.CODE,Y.CATEGORYCODE,Y.CATEGORYNAME " +
                "  FROM GOODS_SHOP G,SHOP S,CATEGORY Y  " +
                "  WHERE G.SHOPID=S.SHOPID AND G.CATEGORYID= Y.CATEGORYID";
            if (!Data.GOODSID.IsEmpty())
                sqlshop += (" and G.GOODSID= " + Data.GOODSID);
            DataTable dtshop = DbHelper.ExecuteTable(sqlshop);

            string sql_jsklGroup = $@"SELECT L.CONTRACTID,L.GROUPNO,L.INX,to_char(L.STARTDATE,'YYYY.MM.DD') STARTDATE, " +
                " to_char(L.ENDDATE,'YYYY.MM.DD') ENDDATE,L.SALES_START,L.SALES_END,L.JSKL   FROM CONTJSKL L WHERE 1=1 ";
            if (!dt.Rows[0]["CONTRACTID"].ToString().IsEmpty())
                sql_jsklGroup += (" and CONTRACTID= " + dt.Rows[0]["CONTRACTID"].ToString());
            if (!dt.Rows[0]["JSKL_GROUP"].ToString().IsEmpty())
                sql_jsklGroup += (" and GROUPNO= " + dt.Rows[0]["JSKL_GROUP"].ToString());
            sql_jsklGroup += "  order by GROUPNO";
            DataTable jsklGroup = DbHelper.ExecuteTable(sql_jsklGroup);

            var result = new
            {
                goods = dt,
                goods_shop = new dynamic[] {
                   dtshop
                },
                goods_group = new dynamic[] {
                   jsklGroup
                }
            };
            return result;
        }

        public object GetContract(CONTRACTEntity Data)
        {
            string sql = $@"select T.MERCHANTID,S.NAME SHMC,T.STYLE,T.JXSL*100 JXSL,T.XXSL*100 XXSL from CONTRACT T,MERCHANT S where T.MERCHANTID=S.MERCHANTID ";
            if (!Data.CONTRACTID.IsEmpty())
                sql += (" and T.CONTRACTID= " + Data.CONTRACTID);
            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.NewEnumColumns<核算方式>("STYLE", "STYLEMC");

            string sql_shop = $@"SELECT P.SHOPID,P.CATEGORYID,S.CODE,Y.CATEGORYCODE,Y.CATEGORYNAME,T.BRANCHID "
                + " FROM CONTRACT_SHOP P,SHOP S,CATEGORY Y,CONTRACT T " 
                + " WHERE  P.SHOPID=S.SHOPID and P.CATEGORYID=Y.CATEGORYID and P.CONTRACTID = T.CONTRACTID";
            if (!Data.CONTRACTID.IsEmpty())
                sql_shop += (" and P.CONTRACTID= " + Data.CONTRACTID);
            sql_shop += " order by S.CODE";
            DataTable shop = DbHelper.ExecuteTable(sql_shop);

            string sql_jsklGroup = $@"SELECT GROUPNO value,JSKL label FROM CONTRACT_GROUP WHERE 1=1";
            if (!Data.CONTRACTID.IsEmpty())
                sql_jsklGroup += (" and CONTRACTID= " + Data.CONTRACTID);
            sql_jsklGroup += "  order by GROUPNO";
            DataTable jsklGroup = DbHelper.ExecuteTable(sql_jsklGroup);
            DataTable jskl = null;
            if (jsklGroup.Rows.Count == 1)
            {
                string sql_jskl = $@"SELECT L.CONTRACTID,L.GROUPNO,L.INX,to_char(L.STARTDATE,'YYYY.MM.DD') STARTDATE, " +
                " to_char(L.ENDDATE,'YYYY.MM.DD') ENDDATE,L.SALES_START,L.SALES_END,L.JSKL   FROM CONTJSKL L WHERE 1=1 ";
                if (!Data.CONTRACTID.IsEmpty())
                    sql_jskl += (" and CONTRACTID= " + Data.CONTRACTID);
                sql_jskl += "  order by INX";
                jskl = DbHelper.ExecuteTable(sql_jskl);
            }
            var result = new
            {
                contract = dt,
                shop = shop,
                jsklGroup = jskl
            };

            return result;
        }

        public Tuple<dynamic> GetKindInit()
        {
            List<GOODS_KINDEntity> p = DbHelper.SelectList(new GOODS_KINDEntity()).OrderBy(a => a.CODE).ToList();
            var treeOrg = new UIResult(TreeModel.Create(p,
                a => a.CODE,
                a => new TreeModel()
                {
                    code = a.CODE,
                    title = a.NAME,
                    value = a.ID,
                    label = a.NAME,
                    expand = true
                })?.ToArray());
            return new Tuple<dynamic>(treeOrg);
        }

        public string ExecData(GOODSEntity Data)
        {
            GOODSEntity mer = DbHelper.Select(Data);
            if (mer.STATUS == ((int)商品状态.审核).ToString())
            {
                throw new LogicException("商品(" + Data.GOODSDM+ ")已经审核不能再次审核!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                mer.VERIFY = employee.Id;
                mer.VERIFY_NAME = employee.Name;
                mer.VERIFY_TIME = DateTime.Now.ToString();
                mer.STATUS = ((int)商品状态.审核).ToString();
                DbHelper.Save(mer);
                Tran.Commit();
            }
            return mer.GOODSDM;
        }        
        public DataGridResult GetSaleBillList(SearchItem item)
        {
            string sql = $@"SELECT L.*,B.NAME BRANCHMC,S1.USERNAME SYYMC,S2.USERNAME YYYMC  FROM SALEBILL L,BRANCH B,SYSUSER S1,SYSUSER S2" +
                " where L.BRANCHID = B.ID  and L.CASHIERID = S1.USERID  and L.CLERKID = S2.USERID  ";
            item.HasKey("BILLID", a => sql += $" and L.BILLID = {a}");
            item.HasKey("BRANCHID", a => sql += $" and L.BRANCHID={a}");
            item.HasKey("MERCHANTID", a => sql += $" and L.MERCHANTID={a}");
            item.HasKey("ACCOUNT_DATE_START ", a => sql += $" and L.ACCOUNT_DATE>={a}");
            item.HasKey("ACCOUNT_DATE_END", a => sql += $" and L.ACCOUNT_DATE<={a}");
            item.HasKey("STATUS", a => sql += $" and L.STATUS={a}");
            item.HasKey("REPORTER", a => sql += $" and L.REPORTER={a}");
            item.HasKey("REPORTER_TIME_START", a => sql += $" and L.REPORTER_TIME>={a}");
            item.HasKey("REPORTER_TIME_END", a => sql += $" and L.REPORTER_TIME<={a}");
            item.HasKey("VERIFY", a => sql += $" and L.VERIFY={a}");
            item.HasKey("VERIFY_TIME_START", a => sql += $" and L.VERIFY_TIME>={a}");
            item.HasKey("VERIFY_TIME_END", a => sql += $" and L.VERIFY_TIME<={a}");
            sql += " ORDER BY  BILLID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }
        public string SaveSaleBill(SALEBILLEntity SaveData)
        {
            var v = GetVerify(SaveData);
            if (SaveData.BILLID.IsEmpty())
                SaveData.BILLID = NewINC("SALEBILL");
            SaveData.STATUS = ((int)普通单据状态.未审核).ToString();
            SaveData.REPORTER = employee.Id;
            SaveData.REPORTER_NAME = employee.Name;
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            SaveData.VERIFY = employee.Id;
            v.Require(a => a.BILLID);
            v.Require(a => a.BRANCHID);
            v.Require(a => a.ACCOUNT_DATE);
            v.Require(a => a.CASHIERID);
            v.Require(a => a.CLERKID);
            v.Verify();

            using (var Tran = DbHelper.BeginTransaction())
            {
                SaveData.SALEBILLITEM?.ForEach(item =>
                {
                    GetVerify(item).Require(a => a.GOODSID);
                    GetVerify(item).Require(a => a.PAYID);
                    GetVerify(item).Require(a => a.QUANTITY);
                    GetVerify(item).Require(a => a.AMOUNT);
                });
                DbHelper.Save(SaveData);
                Tran.Commit();
            }
            return SaveData.BILLID;
        }
        public object ShowOneSaleBillEdit(SALEBILLEntity Data)
        {
            string sql = $@" SELECT L.*,B.NAME BRANCHMC,S1.USERNAME SYYMC,S2.USERNAME YYYMC" +
                "   FROM SALEBILL L,BRANCH B,SYSUSER S1,SYSUSER S2 " +
                "  where L.BRANCHID = B.ID and L.CASHIERID = S1.USERID  and L.CLERKID = S2.USERID ";
            if (!Data.BILLID.IsEmpty())
                sql += (" and L.BILLID= " + Data.BILLID);
            DataTable dt = DbHelper.ExecuteTable(sql);

            string sqlsale = $@"SELECT G.*,S.GOODSDM,S.NAME,P.NAME PAYNAME,O.CODE   FROM SALEBILLITEM G,GOODS S,PAY P,SHOP O " +
                "  WHERE G.GOODSID=S.GOODSID and G.PAYID = P.PAYID  and G.SHOPID= O.SHOPID" ;
            if (!Data.BILLID.IsEmpty())
                sqlsale += (" and G.BILLID= " + Data.BILLID);
            DataTable dtsale = DbHelper.ExecuteTable(sqlsale);

            var result = new
            {
                saleBill = dt,
                saleBillItem = new dynamic[] {
                   dtsale
                }
            };
            return result;
        }

        public void DeleteSaleBill(List<SALEBILLEntity> DeleteData)
        {
            foreach (var con in DeleteData)
            {
                SALEBILLEntity Data = DbHelper.Select(con);
                if (Data.STATUS != ((int)普通单据状态.未审核).ToString())
                {
                    throw new LogicException($"租约({Data.BILLID})已经不是未审核不能删除!");
                }
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                foreach (var salebill in DeleteData)
                {
                    var v = GetVerify(salebill);
                    //校验
                    DbHelper.Delete(salebill);
                }
                Tran.Commit();
            }
        }
        public Tuple<dynamic, DataTable> GetSaleBillDetail(SALEBILLEntity Data)
        {
            string sql = $@" SELECT L.*,B.NAME BRANCHMC,S1.USERNAME SYYMC,S2.USERNAME YYYMC" +
                "   FROM SALEBILL L,BRANCH B,SYSUSER S1,SYSUSER S2 " +
                "  where L.BRANCHID = B.ID and L.CASHIERID = S1.USERID  and L.CLERKID = S2.USERID ";
            if (!Data.BILLID.IsEmpty())
                sql += (" and L.BILLID= " + Data.BILLID);
            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.NewEnumColumns<退铺单状态>("STATUS", "STATUSMC");

            string sqlsale = $@"SELECT G.*,S.GOODSDM,S.NAME,P.NAME PAYNAME,O.CODE  FROM SALEBILLITEM G,GOODS S,PAY P,SHOP O " +
                "  WHERE G.GOODSID=S.GOODSID and G.PAYID = P.PAYID  and G.SHOPID= O.SHOPID";
            if (!Data.BILLID.IsEmpty())
                sqlsale += (" and G.BILLID= " + Data.BILLID);
            DataTable dtsale = DbHelper.ExecuteTable(sqlsale);

            return new Tuple<dynamic, DataTable>(dt.ToOneLine(), dtsale);
        }
        public string ExecSaleBillData(SALEBILLEntity Data)
        {
            SALEBILLEntity mer = DbHelper.Select(Data);
            if (mer.STATUS == ((int)普通单据状态.审核).ToString())
            {
                throw new LogicException("商品(" + Data.BILLID + ")已经审核不能再次审核!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                EXEC_SALEBILL execsalebill = new EXEC_SALEBILL()
                {
                    P_BILLID = Data.BILLID,
                    P_VERIFY = employee.Id
                };
                DbHelper.ExecuteProcedure(execsalebill);
                Tran.Commit();
            }
            return mer.BILLID;
        }
    }
}
