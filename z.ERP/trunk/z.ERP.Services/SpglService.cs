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
using z.SSO.Model;

namespace z.ERP.Services
{
    public class SpglService: ServiceBase
    {
        internal SpglService()
        {
        }
        #region 商品信息
        public DataGridResult GetGoods(SearchItem item)
        {
            string sql = $@"SELECT G.*,K.CODE KINDCODE,K.NAME KINDNAME,M.NAME MERCHANTNAME ";
            sql += "   FROM GOODS G,GOODS_KIND K,MERCHANT M,CONTRACT C";
            sql += " WHERE G.CONTRACTID = C.CONTRACTID and G.MERCHANTID=M.MERCHANTID(+) AND G.KINDID=K.ID(+)";
            sql += "   and C.BRANCHID IN (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            item.HasKey("GOODSDM", a => sql += $" and G.GOODSDM = '{a}'");
            item.HasKey("BARCODE", a => sql += $" and G.BARCODE={a}");
            item.HasKey("NAME", a => sql += $" and G.NAME  LIKE '%{a}%'");
            item.HasKey("PYM", a => sql += $" and G.PYM  LIKE '%{a}%'");
            item.HasKey("CONTRACTID", a => sql += $" and G.CONTRACTID={a}");
            item.HasKey("MERCHANTID", a => sql += $" and G.MERCHANTID={a}");
            item.HasKey("KINDID", a => sql += $" and G.KINDID={a}");
            item.HasKey("TYPE", a => sql += $" and G.TYPE={a}");
            sql += " ORDER BY  G.REPORTER_TIME DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<商品状态>("STATUS", "STATUSMC");
            dt.NewEnumColumns<商品类型>("TYPE", "TYPEMC");
            return new DataGridResult(dt, count);
        }
        public string SaveGoods(GOODSEntity SaveData)
        {
            var v = GetVerify(SaveData);
            
            //定义随机数,条码末尾校验位
            Random sj = new Random();

            CONTRACTEntity cont = new CONTRACTEntity();
            cont = DbHelper.Select(new CONTRACTEntity() { CONTRACTID = SaveData.CONTRACTID });

            CONFIGEntity cfgfs = new CONFIGEntity(); //商品编码生成方式:0系统自动生成流水码1手工录入自编码
            cfgfs = DbHelper.Select(new CONFIGEntity() { ID = "3001" });
            if (String.IsNullOrEmpty(cfgfs.CUR_VAL) || (!"012".Contains(cfgfs.CUR_VAL)))
            {
                throw new LogicException("参数3001(商品编码生成方式)设置有误");
            }
            CONFIGEntity cfgcd = new CONFIGEntity();  //商品编码长度
            cfgcd = DbHelper.Select(new CONFIGEntity() { ID = "3002" });
            if (String.IsNullOrEmpty(cfgcd.CUR_VAL) || (cfgcd.CUR_VAL.ToInt() < 6) || (cfgcd.CUR_VAL.ToInt() > 10))
            {
                throw new LogicException("参数3002(商品编码长度)设置有误");
            }
            int spbmfs = cfgfs.CUR_VAL.ToInt();
            int spbmcd = cfgcd.CUR_VAL.ToInt();

            if (SaveData.GOODSID.IsEmpty())
            { 
                SaveData.GOODSID = cont.BRANCHID +CommonService.NewINC("GOODSID_" + cont.BRANCHID).PadLeft(spbmcd - 1, '0');
                if (spbmfs == 0 && !SaveData.GOODSDM.IsEmpty())
                    SaveData.GOODSDM = null;

                SaveData.REPORTER = employee.Id;
                SaveData.REPORTER_NAME = employee.Name;
                SaveData.REPORTER_TIME = DateTime.Now.ToString();
            }

            if (SaveData.GOODSDM.IsEmpty())
            {
                if (spbmfs == 0)
                {
                    SaveData.GOODSDM = SaveData.GOODSID;
                }
                else
                {
                    throw new LogicException("商品编码不能为空,请录入!");
                }

              //  SaveData.GOODSDM = CommonService.NewINC("GOODSDM").PadLeft(spbmcd, '0');
            }
            else
            {
               if (  SaveData.GOODSDM.Length != spbmcd)
                    throw new LogicException("商品编码长度必须是"+ spbmcd + "位!");
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

            SaveData.STATUS = ((int)商品状态.未审核).ToString();

            SaveData.GOODS_SHOP.ForEach(sdb =>
            {
                GetVerify(sdb).Require(a => a.GOODSID);
                GetVerify(sdb).Require(a => a.BRANCHID);
                GetVerify(sdb).Require(a => a.SHOPID);
                GetVerify(sdb).Require(a => a.CATEGORYID);
            });
            v.Verify();

            //增加审核待办任务
            var dcl = new BILLSTATUSEntity
            {
                BILLID = SaveData.GOODSID,
                MENUID = "10500202",
                BRABCHID = (SaveData.CONTRACTID).Substring(1, 2),
                URL = "SPGL/GOODS/GoodsEdit/"
            };

            using (var Tran = DbHelper.BeginTransaction())
            {
                DbHelper.Save(SaveData);
                InsertDclRw(dcl);
                Tran.Commit();
            }

            return SaveData.GOODSID;
        }
        public void DeleteGoods(List<GOODSEntity> DeleteData)
        {
            using (var Tran = DbHelper.BeginTransaction())
            {
                foreach (var goods in DeleteData)
                {
                    //var v = GetVerify(goods);
                    GOODSEntity g = DbHelper.Select(new GOODSEntity() { GOODSID = goods.GOODSID});
                    if (g.STATUS != ((int)商品状态.未审核).ToString())
                        throw new LogicException("商品(" + g.GOODSDM + ")不是未审核状态,不能删除!");

                    var dcl = new BILLSTATUSEntity
                    {
                        BILLID = goods.GOODSID,
                        MENUID = "10500202"
                    };

                    DbHelper.Delete(goods);

                    DelDclRw(dcl);  //删除审核待办任务
                }
                Tran.Commit();
            }  
        }

        public Tuple<dynamic, DataTable, DataTable> ShowOneEdit(GOODSEntity Data)
        {
            string sql = $@" select G.*,M.NAME SHMC,D.NAME BRANDMC,C.CODE,C.PKIND_ID from GOODS G,MERCHANT M,GOODS_KIND C,BRAND D";
            sql += "  where G.MERCHANTID=M.MERCHANTID  AND G.KINDID=C.ID and G.BRANDID =D.ID ";
            if (!Data.GOODSID.IsEmpty())
                sql += (" and G.GOODSID= " + Data.GOODSID);
            DataTable dt = DbHelper.ExecuteTable(sql);
            if (!dt.IsNotNull())
            {
                throw new LogicException("此商品不存在！");
            }
            dt.Rows[0]["JXSL"] = dt.Rows[0]["JXSL"].ToString().ToDouble() * 100;
            dt.Rows[0]["XXSL"] = dt.Rows[0]["XXSL"].ToString().ToDouble() * 100;
            dt.NewEnumColumns<商品状态>("STATUS", "STATUSMC");
            dt.NewEnumColumns<商品类型>("TYPE", "TYPEMC");
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

            return new Tuple<dynamic, DataTable, DataTable>(dt.ToOneLine(), dtshop, jsklGroup);
        }
        public object GetContract(CONTRACTEntity Data)
        {
            string sql = $@"select T.MERCHANTID,S.NAME SHMC,T.STYLE,T.JXSL*100 JXSL,T.XXSL*100 XXSL ";
            sql += "from CONTRACT T,MERCHANT S where T.MERCHANTID=S.MERCHANTID ";
            sql += " and T.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
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
        public DataTable GetKLZinfo(CONTJSKLEntity Data) {
            string sql_jskl = $@"SELECT L.CONTRACTID,L.GROUPNO,L.INX,to_char(L.STARTDATE,'YYYY.MM.DD') STARTDATE, " +
                " to_char(L.ENDDATE,'YYYY.MM.DD') ENDDATE,L.SALES_START,L.SALES_END,L.JSKL   FROM CONTJSKL L WHERE 1=1 ";
            if (!Data.CONTRACTID.IsEmpty())
                sql_jskl += (" and CONTRACTID= " + Data.CONTRACTID);
            if (!Data.GROUPNO.IsEmpty())
                sql_jskl += (" and GROUPNO= " + Data.GROUPNO);
            sql_jskl += "  order by INX";
            DataTable jskl = DbHelper.ExecuteTable(sql_jskl);
            return jskl;
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
                throw new LogicException("商品(" + Data.GOODSDM + ")已经审核不能再次审核!");
            }

            string sql = $"SELECT DISTINCT C.CONTRACTID FROM GOODS A,GOODS_SHOP B,CONTRACT C "
                            + "  WHERE A.GOODSID=B.GOODSID "
                             + "     AND A.CONTRACTID = C.CONTRACTID "
                             + "   AND C.HTLX =1 "
                             + $"   AND A.CONTRACTID !=  '{Data.CONTRACTID}'"
                             + $"   AND B.SHOPID IN (SELECT SHOPID FROM GOODS_SHOP WHERE GOODSID= {Data.GOODSID})"
                             + "    AND A.STATUS=2";

            DataTable dt = DbHelper.ExecuteTable(sql);

            if (dt.Rows.Count > 0)
            {
                throw new LogicException("该商品所选店铺下存在其它合同(" + dt.Rows[0][0].ToString() + ")的正常状态商品,请先处理再审核");
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

            //删除审核待办任务
            var dcl = new BILLSTATUSEntity
            {
                BILLID = Data.GOODSID,
                MENUID = "10500202"
            };

            DelDclRw(dcl);

            return mer.GOODSDM;
        }
        #endregion

        #region 销售补录单
        public DataGridResult GetSaleBillList(SearchItem item)
        {
            string sql = $@"SELECT L.*,B.NAME BRANCHMC,S1.USERNAME SYYMC,S2.USERNAME YYYMC "
               + "  FROM SALEBILL L,BRANCH B,SYSUSER S1,SYSUSER S2"
               + " where L.BRANCHID = B.ID  and L.CASHIERID = S1.USERID  and L.CLERKID = S2.USERID  "
               + "   and  B.ID IN (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            item.HasKey("BILLID", a => sql += $" and L.BILLID = {a}");
            item.HasKey("BRANCHID", a => sql += $" and L.BRANCHID={a}");
            item.HasKey("MERCHANTID", a => sql += $" and  exists(select 1 from CONTRACT_SHOP S,CONTRACT C where S.CONTRACTID=C.CONTRACTID and S.SHOPID=S2.SHOPID and C.MERCHANTID={a})");
            item.HasKey("BRANDID", a => sql += $" and exists(select 1 from CONTRACT_BRAND R,CONTRACT_SHOP S where R.CONTRACTID=S.CONTRACTID and S.SHOPID=S2.SHOPID and R.BRANDID={a})");
            item.HasDateKey("ACCOUNT_DATE_START", a => sql += $" and L.ACCOUNT_DATE>={a}");
            item.HasDateKey("ACCOUNT_DATE_END", a => sql += $" and L.ACCOUNT_DATE<={a}");
            item.HasKey("STATUS", a => sql += $" and L.STATUS={a}");
            item.HasKey("REPORTER", a => sql += $" and L.REPORTER={a}");
            item.HasKey("REPORTER_NAME", a => sql += $" and L.REPORTER_NAME  LIKE '%{a}%'");
            item.HasKey("VERIFY_NAME", a => sql += $" and L.VERIFY_NAME  LIKE '%{a}%'");
            item.HasDateKey("REPORTER_TIME_START", a => sql += $" and TRUNC(L.REPORTER_TIME)>={a}");
            item.HasDateKey("REPORTER_TIME_END", a => sql += $" and TRUNC(L.REPORTER_TIME)<={a}");
            item.HasKey("VERIFY", a => sql += $" and L.VERIFY={a}");
            item.HasDateKey("VERIFY_TIME_START", a => sql += $" and TRUNC(L.VERIFY_TIME)>={a}");
            item.HasDateKey("VERIFY_TIME_END", a => sql += $" and TRUNC(L.VERIFY_TIME)<={a}");
            sql += " ORDER BY  BILLID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }
        public object ShowOneSaleBillEdit(SALEBILLEntity Data)
        {
            string sql = $@" SELECT L.*,B.NAME BRANCHMC,S1.USERNAME SYYMC,S2.USERNAME YYYMC" +
                "   FROM SALEBILL L,BRANCH B,SYSUSER S1,SYSUSER S2 " +
                "  where L.BRANCHID = B.ID and L.CASHIERID = S1.USERID  and L.CLERKID = S2.USERID " +
                "    and B.ID IN (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            if (!Data.BILLID.IsEmpty())
                sql += (" and L.BILLID= " + Data.BILLID);
            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");

            string sqlsale = $@"SELECT G.*,S.GOODSDM,S.NAME,P.NAME PAYNAME,O.CODE   FROM SALEBILLITEM G,GOODS S,PAY P,SHOP O " +
                "  WHERE G.GOODSID=S.GOODSID and G.PAYID = P.PAYID  and G.SHOPID= O.SHOPID";
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
        private void SaleBillPro(SALEBILLEntity data)
        {
            var v = GetVerify(data);
            if (data.BILLID.IsEmpty())
                data.BILLID = GetBillINC("SALEBILL", data.BRANCHID);
            data.STATUS = ((int)普通单据状态.未审核).ToString();
            data.REPORTER = employee.Id;
            data.REPORTER_NAME = employee.Name;
            data.REPORTER_TIME = DateTime.Now.ToString();
            data.POSNO = (data.BRANCHID + "0999").PadLeft(6, '0');
            
            v.Require(a => a.BILLID);
            v.Require(a => a.BRANCHID);
            v.Require(a => a.ACCOUNT_DATE);
            v.Require(a => a.CASHIERID);
            v.Require(a => a.CLERKID);
            v.Verify();

            data.ACCOUNT_DATE = Convert.ToDateTime(data.ACCOUNT_DATE).ToShortDateString();

            data.SALEBILLITEM?.ForEach(item =>
            {
                GetVerify(item).Require(a => a.GOODSID);
                GetVerify(item).Require(a => a.PAYID);
                GetVerify(item).Require(a => a.QUANTITY);
                GetVerify(item).Require(a => a.AMOUNT);
            });
            DbHelper.Save(data);

            //添加待处理任务
            var dcl = new BILLSTATUSEntity
            {
                BILLID = data.BILLID,
                MENUID = "10500402",
                BRABCHID = data.BRANCHID,
                URL = "SPGL/SALEBILL/SaleBillEdit/"
            };
            InsertDclRw(dcl);
        }
        public string SaveSaleBill(SALEBILLEntity SaveData)
        {
           
            using (var Tran = DbHelper.BeginTransaction())
            {
                SaleBillPro(SaveData);
                Tran.Commit();
            }
            return SaveData.BILLID;
        }
        public ImportMsg VerifyImportDataSaleBill(DataTable dt, ref List<SALEBILLEntity> SaveDataList)
        {
            var backData = new ImportMsg();
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var SaveData = new SALEBILLEntity();

                if (dt.Columns.Contains("门店编号"))
                {
                    var fdbh = dt.Rows[i]["门店编号"].ToString();
                    if (string.IsNullOrEmpty(fdbh))
                    {
                        backData.Message = $@"第{i + 1}行的门店编号不能为空！";
                        backData.SuccFlag = false;
                        return backData;
                    }
                    SaveData.BRANCHID = fdbh;
                }
                else
                {
                    backData.Message = "导入项没有门店编号！";
                    backData.SuccFlag = false;
                    return backData;
                }

                if (dt.Columns.Contains("记账日期"))
                {
                    var jzri = dt.Rows[i]["记账日期"].ToString();
                    if (string.IsNullOrEmpty(jzri))
                    {
                        backData.Message = $@"第{i + 1}行的记账日期不能为空！";
                        backData.SuccFlag = false;
                        return backData;
                    }
                    SaveData.ACCOUNT_DATE = Convert.ToDateTime(jzri).ToShortDateString();
                }
                else
                {
                    backData.Message = "导入项没有记账日期！";
                    backData.SuccFlag = false;
                    return backData;
                }
                var syy = "";
                if (dt.Columns.Contains("收银员"))
                {
                    syy = "收银员";
                }
                if (dt.Columns.Contains("收银员编码"))
                {
                    syy = "收银员编码";
                }
                if (string.IsNullOrEmpty(syy))
                {
                    backData.Message = "导入项没有收银员！";
                    backData.SuccFlag = false;
                    return backData;
                }
                var syyValue = dt.Rows[i][syy].ToString();

                if (string.IsNullOrEmpty(syyValue))
                {
                    backData.Message = $@"第{i + 1}行的{syy}不能为空！";
                    backData.SuccFlag = false;
                    return backData;
                }else
                {
                    SaveData.CASHIERID = GetTableDataKey("SYSUSER", "USERCODE", syyValue, "USERID");
                }

                var yyy = "";
                if (dt.Columns.Contains("营业员"))
                {
                    yyy = "营业员";
                }
                if (dt.Columns.Contains("营业员编码"))
                {
                    yyy = "营业员编码";
                }
                if (string.IsNullOrEmpty(syy))
                {
                    backData.Message = "导入项没有收银员！";
                    backData.SuccFlag = false;
                    return backData;
                }
                var yyyValue = dt.Rows[i][yyy].ToString();
                if (string.IsNullOrEmpty(syyValue))
                {
                    backData.Message = $@"第{i + 1}行的{yyy}不能为空！";
                    backData.SuccFlag = false;
                    return backData;
                }
                else
                {
                    SaveData.CLERKID = GetTableDataKey("SYSUSER", "USERCODE", yyyValue, "USERID");
                }

                var list = new List<SALEBILLITEMEntity>();
                var item = new SALEBILLITEMEntity();
                if (dt.Columns.Contains("商品代码"))
                {
                    var goodsdm = dt.Rows[i]["商品代码"].ToString();
                    if (string.IsNullOrEmpty(goodsdm))
                    {
                        backData.Message = $@"第{i + 1}行的商品代码不能为空！";
                        backData.SuccFlag = false;
                        return backData;
                    }
                    item.GOODSID = GetTableDataKey("GOODS", "GOODSDM", goodsdm, "GOODSID");
                    item.SHOPID = GetTableDataKey("GOODS_SHOP", "GOODSID", item.GOODSID, "SHOPID");
                }
                else
                {
                    backData.Message = "导入项没有商品代码！";
                    backData.SuccFlag = false;
                    return backData;
                }
                if (dt.Columns.Contains("收款金额"))
                {
                    var skje = dt.Rows[i]["收款金额"].ToString();
                    if (string.IsNullOrEmpty(skje))
                    {
                        backData.Message = $@"第{i + 1}行的收款金额不能为空！";
                        backData.SuccFlag = false;
                        return backData;
                    }
                    item.AMOUNT = skje;
                }
                else
                {
                    backData.Message = "导入项没有收款金额！";
                    backData.SuccFlag = false;
                    return backData;
                }
                item.PAYID = "1";
                item.QUANTITY = "1";
                list.Add(item);
                SaveData.SALEBILLITEM = list;

                SaveDataList.Add(SaveData);
            }
            return backData;
        }
        public ImportMsg SaleBillImport(DataTable dt)
        {
            var SaveDataList = new List<SALEBILLEntity>();
            var data = VerifyImportDataSaleBill(dt, ref SaveDataList);
            if (data.SuccFlag)
            {
                using (var Tran = DbHelper.BeginTransaction())
                {
                    for (var i = 0; i < SaveDataList.Count; i++)
                    {
                        SaleBillPro(SaveDataList[i]);
                    }
                    Tran.Commit();
                }
            }           
            return data;
        }
        public void DeleteSaleBill(List<SALEBILLEntity> DeleteData)
        {
            foreach (var con in DeleteData)
            {
                SALEBILLEntity Data = DbHelper.Select(con);
                if (Data.STATUS != ((int)普通单据状态.未审核).ToString())
                {
                    throw new LogicException($"补录单({Data.BILLID})已经不是未审核不能删除!");
                }
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                foreach (var salebill in DeleteData)
                {
                    var v = GetVerify(salebill);
                    //校验

                    var dcl = new BILLSTATUSEntity
                    {
                        BILLID = salebill.BILLID,
                        MENUID = "10500402",
                        BRABCHID = salebill.BRANCHID,
                        URL = "SPGL/SALEBILL/SaleBillEdit/"
                    };
                    DelDclRw(dcl);
                    DbHelper.Delete(salebill);
                }
                Tran.Commit();
            }
        }
        public string ExecSaleBillData(SALEBILLEntity Data)
        {
            SALEBILLEntity mer = DbHelper.Select(Data);
            if (mer.STATUS == ((int)普通单据状态.审核).ToString())
            {
                throw new LogicException("单据(" + Data.BILLID + ")已经审核不能再次审核!");
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
            var dcl = new BILLSTATUSEntity
            {
                BILLID = Data.BILLID,
                MENUID = "10500402",
                BRABCHID = Data.BRANCHID,
                URL = "SPGL/SALEBILL/SaleBillEdit/"
            };
            DelDclRw(dcl);
            return mer.BILLID;
        }
        public void ExecSaleBillDataList(List<SALEBILLEntity> DataList)
        {
            foreach (var Data in DataList) {            
                SALEBILLEntity mer = DbHelper.Select(Data);
                if (mer.STATUS == ((int)普通单据状态.审核).ToString()) continue;
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
                var dcl = new BILLSTATUSEntity
                {
                    BILLID = Data.BILLID,
                    MENUID = "10500402",
                    BRABCHID = Data.BRANCHID,
                    URL = "SPGL/SALEBILL/SaleBillEdit/"
                };
                DelDclRw(dcl);
            }
        }
        #endregion

        #region 扣率调整单
        public DataGridResult GetRateAdjustList(SearchItem item)
        {
            string sql = @"SELECT A.*,B.NAME BRANCHNAME "
                        + "  FROM RATE_ADJUST A,BRANCH B "
                        + " WHERE A.BRANCHID=B.ID "
                        + "   AND B.ID IN ("+GetPermissionSql(PermissionType.Branch)+")";   //门店权限
            item.HasKey("ADID", a => sql += $" and A.ID = {a}");
            item.HasKey("BRANCHID", a => sql += $" and A.BRANCHID={a}");           
            item.HasDateKey("DATE_START", a => sql += $" and TRUNC(A.STARTTIME)>={a}");
            item.HasDateKey("DATE_END", a => sql += $" and TRUNC(A.ENDTIME)<={a}");
            item.HasKey("STATUS", a => sql += $" and A.STATUS={a}");
            item.HasKey("REPORTER", a => sql += $" and A.REPORTER={a}");
            item.HasKey("REPORTER_NAME", a => sql += $" and A.REPORTER_NAME  LIKE '%{a}%'");
            item.HasKey("VERIFY_NAME", a => sql += $" and A.VERIFY_NAME  LIKE '%{a}%'");
            item.HasDateKey("REPORTER_TIME_START", a => sql += $" and TRUNC(A.REPORTER_TIME)>={a}");
            item.HasDateKey("REPORTER_TIME_END", a => sql += $" and TRUNC(A.REPORTER_TIME)<={a}");
            item.HasKey("VERIFY", a => sql += $" and A.VERIFY={a}");
            item.HasDateKey("VERIFY_TIME_START", a => sql += $" and TRUNC(A.VERIFY_TIME)>={a}");
            item.HasDateKey("VERIFY_TIME_END", a => sql += $" and TRUNC(A.VERIFY_TIME)<={a}");
            sql += " ORDER BY A.ID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }

        public object ShowOneRateAdjustEdit(RATE_ADJUSTEntity Data)
        {
            string sql = @"SELECT A.*,B.NAME BRANCHNAME FROM RATE_ADJUST A,BRANCH B
                    WHERE A.BRANCHID=B.ID ";
            if (!Data.ID.IsEmpty())
                sql += (" and A.ID= " + Data.ID);
            DataTable dt = DbHelper.ExecuteTable(sql);

            string sqlitem = @"SELECT A.*,G.GOODSDM,G.NAME FROM RATE_ADJUST_ITEM A,GOODS G
                    WHERE A.GOODSID=G.GOODSID";
            if (!Data.ID.IsEmpty())
                sqlitem += (" and A.ID= " + Data.ID);
            DataTable dtitem = DbHelper.ExecuteTable(sqlitem);

            var result = new
            {
                RATE_ADJUST = dt,
                RATE_ADJUST_ITEM = new dynamic[] {
                   dtitem
                }
            };
            return result;
        }
        public string SaveRateAdjust(RATE_ADJUSTEntity SaveData)
        {
            var v = GetVerify(SaveData);
            if (SaveData.ID.IsEmpty())
            {
                SaveData.ID = NewINC("RATE_ADJUST");
                SaveData.REPORTER = employee.Id;
                SaveData.REPORTER_NAME = employee.Name;
                SaveData.REPORTER_TIME = DateTime.Now.ToString();
            }
            else {
                RATE_ADJUSTEntity data = DbHelper.Select(new RATE_ADJUSTEntity() { ID = SaveData.ID });

                if (data == null)
                {
                    throw new LogicException("该单据不存在!");
                }

                if (data.STATUS != ((int)普通单据状态.未审核).ToString())
                {
                    throw new LogicException("该单据不是未审核状态，不能修改!");
                }
            }
            SaveData.STATUS = ((int)普通单据状态.未审核).ToString();
            v.Require(a => a.ID);
            v.Require(a => a.BRANCHID);
            v.Require(a => a.STARTTIME);
            v.Require(a => a.ENDTIME);
            v.Verify();

            using (var Tran = DbHelper.BeginTransaction())
            {
                SaveData.RATE_ADJUST_ITEM?.ForEach(item =>
                {
                    item.SHEETID = "1";
                    GetVerify(item).Require(a => a.GOODSID);
                    GetVerify(item).Require(a => a.RATE_NEW);
                });
                DbHelper.Save(SaveData);
                Tran.Commit();
            }

            //增加审核待办任务
            var dcl = new BILLSTATUSEntity
            {
                BILLID = SaveData.ID,
                MENUID = "10500702",
                BRABCHID = SaveData.BRANCHID,
                URL = "SPGL/RATE_ADJUST/Rate_AdjustEdit/"    
            };

            InsertDclRw(dcl);



            return SaveData.ID;
        }
        public void DeleteRateAdjust(List<RATE_ADJUSTEntity> DeleteData)
        {
            foreach (var con in DeleteData)
            {
                RATE_ADJUSTEntity Data = DbHelper.Select(con);
                if (Data.STATUS != ((int)普通单据状态.未审核).ToString())
                {
                    throw new LogicException($"单据({Data.ID})不是未审核状态,不能执行删除操作!");
                }
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                foreach (var AD in DeleteData)
                {
                    var v = GetVerify(AD);
                    //校验
                    DbHelper.Delete(AD);
                }
                Tran.Commit();
            }

            //删除审核待办任务
            using (var Tran = DbHelper.BeginTransaction())
            {
                foreach (var AD in DeleteData)
                {
                    var dcl = new BILLSTATUSEntity
                    {
                        BILLID = AD.ID,
                        MENUID = "10500702",
                        BRABCHID = AD.BRANCHID,
                        URL = "SPGL/RATE_ADJUST/Rate_AdjustEdit/"
                    };
                    DelDclRw(dcl);

                }
                Tran.Commit();
            }
        }

        public string ExecRateAdjust(RATE_ADJUSTEntity Data)
        {
            RATE_ADJUSTEntity ra = DbHelper.Select(Data);
            if (ra.STATUS != ((int)普通单据状态.未审核).ToString())
            {
                throw new LogicException("单据(" + Data.ID + ")不是未审核状态,不能执行审核操作!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                EXEC_RATE_ADJUST proExec = new EXEC_RATE_ADJUST()
                {
                    in_ID = Data.ID,
                    in_USERID = employee.Id
                };
                DbHelper.ExecuteProcedure(proExec);
                Tran.Commit();
            }

            //删除审核待办任务
            var dcl = new BILLSTATUSEntity
            {
                BILLID = Data.ID,
                MENUID = "10500702",
                BRABCHID = Data.BRANCHID,
                URL = "SPGL/RATE_ADJUST/Rate_AdjustEdit/"    
            };
            DelDclRw(dcl);


            return ra.ID;
        }

        public string StopRateAdjust(RATE_ADJUSTEntity Data)
        {
            RATE_ADJUSTEntity ra = DbHelper.Select(Data);
            if (ra.STATUS != ((int)普通单据状态.审核).ToString())
            {
                throw new LogicException("单据(" + Data.ID + ")不是审核状态,不能执行终止操作!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                STOP_RATE_ADJUST proStop = new STOP_RATE_ADJUST()
                {
                    in_ID = Data.ID,
                    in_USERID = employee.Id
                };
                DbHelper.ExecuteProcedure(proStop);
                Tran.Commit();
            }

            return ra.ID;
        }


        #endregion
    }    
}
