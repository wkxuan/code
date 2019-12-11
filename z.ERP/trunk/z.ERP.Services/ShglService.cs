using System.Data;
using z.MVC5.Results;
using z.ERP.Entities;
using System.Collections.Generic;
using z.Extensions;
using System;
using z.ERP.Entities.Enum;
using z.Exceptions;
using System.Linq;
using z.ERP.Model.Vue;
using z.Extensions;
using z.SSO.Model;

namespace z.ERP.Services
{
    public class ShglService : ServiceBase
    {
        internal ShglService()
        {
        }
        /// <summary>
        /// 列表页的查询
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public DataGridResult GetMerchant(SearchItem item)
        {
            string sql = $@"SELECT * FROM MERCHANT M WHERE 1=1 ";
            item.HasKey("MERCHANTID", a => sql += $" and M.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and M.NAME  LIKE '%{a}%'");
            item.HasKey("SH", a => sql += $" and M.SH LIKE '%{a}%'");
            item.HasKey("BANK", a => sql += $" and M.BANK LIKE '%{a}%'");
            item.HasKey("REPORTER_NAME", a => sql += $" and M.REPORTER_NAME  LIKE '%{a}%'");
            item.HasKey("REPORTER_TIME_START", a => sql += $" and M.REPORTER_TIME>= to_date('{a.ToDateTime().ToLocalTime()}','YYYY-MM-DD  HH24:MI:SS')");
            item.HasKey("REPORTER_TIME_END", a => sql += $" and M.REPORTER_TIME< to_date('{a.ToDateTime().ToLocalTime()}','YYYY-MM-DD  HH24:MI:SS') + 1");
            item.HasKey("STATUS", a => sql += $" and M.STATUS in ({a})");
            item.HasKey("TYPE", a => sql += $" and M.TYPE in ({a})");
            item.HasKey("SHOPCODE", a => sql += $" AND EXISTS(SELECT 1 FROM CONTRACT C,CONTRACT_SHOP CS,SHOP WHERE M.MERCHANTID=C.MERCHANTID AND C.CONTRACTID=CS.CONTRACTID AND SHOP.SHOPID=CS.SHOPID AND SHOP.CODE ='{a}')");
            sql += " ORDER BY  M.MERCHANTID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            dt.NewEnumColumns<商户类型>("TYPE", "TYPENAME");
            return new DataGridResult(dt, count);
        }

        /// <summary>
        /// 列表页的删除,可以批量删除
        /// </summary>
        /// <param name="DeleteData"></param>
        public void DeleteMerchant(List<MERCHANTEntity> DeleteData)
        {
            foreach (var mer in DeleteData)
            {
                MERCHANTEntity Data = DbHelper.Select(mer);
                if (Data.STATUS == ((int)普通单据状态.审核).ToString())
                {
                    throw new LogicException("商户(" + Data.NAME + ")已经审核不能删除!");
                }
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                foreach (var mer in DeleteData)
                {
                    DbHelper.Delete(mer);
                }
                Tran.Commit();
            }
        }
        public DataTable SearchCMP(string MERCHANTID, string PAYMENTID) {
            string sql = $@"SELECT * FROM CONTRACT where MERCHANTID={MERCHANTID} and PAYMENTID={PAYMENTID} ";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt;
        }
        /// <summary>
        /// 编辑页的保存
        /// </summary>
        /// <param name="SaveData"></param>
        /// <returns></returns>
        public string SaveMerchant(MERCHANTEntity SaveData)
        {
            var v = GetVerify(SaveData);
            if (SaveData.MERCHANTID.IsEmpty())
            {
                SaveData.MERCHANTID = NewINC("MERCHANT").PadLeft(4, '0');  //暂定6位
                SaveData.STATUS = ((int)普通单据状态.未审核).ToString();
            }
            else
            {
                MERCHANTEntity mer = DbHelper.Select(SaveData);
                SaveData.VERIFY = mer.VERIFY;
                SaveData.VERIFY_NAME = mer.VERIFY_NAME;
                SaveData.VERIFY_TIME = mer.VERIFY_TIME;
            }
            SaveData.REPORTER = employee.Id;
            SaveData.REPORTER_NAME = employee.Name;
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            v.Require(a => a.MERCHANTID);
            v.Require(a => a.NAME);
            v.IsUnique(a => a.MERCHANTID);
            v.IsUnique(a => a.NAME);

            if (SaveData.TYPE == "1")
                v.IsUnique(a => a.LICENSE);
            else
                v.IsUnique(a => a.IDCARD);

            v.Verify();
            using (var Tran = DbHelper.BeginTransaction())
            {
                SaveData.MERCHANT_BRAND?.ForEach(shpp =>
                {
                    GetVerify(shpp).Require(a => a.BRANDID);
                });
                DbHelper.Save(SaveData);

                Tran.Commit();
            }
            return SaveData.MERCHANTID;
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Output(string id)
        {
            if (id.IsEmpty())
            {
                throw new LogicException("请确认商户编号!");
            }
            string sql = $@"SELECT * FROM MERCHANT WHERE 1=1  AND MERCHANTID= {id}";
            DataTable merchant = DbHelper.ExecuteTable(sql);
            merchant.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            string sqlitem = $@"SELECT M.BRANDID,C.NAME,D.CATEGORYCODE,D.CATEGORYNAME " +
            " FROM MERCHANT_BRAND M,MERCHANT E,BRAND C,CATEGORY D " +
            $" where M.MERCHANTID = E.MERCHANTID AND M.BRANDID=C.ID AND  C.CATEGORYID = D.CATEGORYID  and E.MERCHANTID= {id}";
            DataTable merchantBrand = DbHelper.ExecuteTable(sqlitem);
            merchantBrand.TableName = "merchantBrand";
            return GetExport("商户导出", a =>
            {
                a.SetString("MERCHANTID", merchant.Rows[0]["MERCHANTID"].ToString());
                a.SetString("NAME", merchant.Rows[0]["NAME"].ToString());
                a.SetString("SH", merchant.Rows[0]["SH"].ToString());
                a.SetString("BANK", merchant.Rows[0]["BANK"].ToString());
                a.SetString("BANK_NAME", merchant.Rows[0]["BANK_NAME"].ToString());
                a.SetString("STATUSMC", merchant.Rows[0]["STATUSMC"].ToString());
                a.SetTable(merchantBrand);
            });


        }

        /// <summary>
        /// 从列表页编辑跳转到编辑页数据的展示查询
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public Tuple<dynamic, DataTable, dynamic, DataTable> GetMerchantElement(MERCHANTEntity Data)
        {
            if (Data.MERCHANTID.IsEmpty())
            {
                throw new LogicException("请确认商户编号!");
            }
            string sql = $@"SELECT * FROM MERCHANT WHERE 1=1 ";
            if (!Data.MERCHANTID.IsEmpty())
                sql += (" AND MERCHANTID= " + Data.MERCHANTID);
            DataTable merchant = DbHelper.ExecuteTable(sql);

            merchant.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            //品牌
            string sqlitem = $@"SELECT M.BRANDID,C.NAME,D.CATEGORYCODE,D.CATEGORYNAME " +
                " FROM MERCHANT_BRAND M,MERCHANT E,BRAND C,CATEGORY D " +
                " where M.MERCHANTID = E.MERCHANTID AND M.BRANDID=C.ID AND  C.CATEGORYID = D.CATEGORYID ";
            if (!Data.MERCHANTID.IsEmpty())
                sqlitem += (" and E.MERCHANTID= " + Data.MERCHANTID);
            DataTable merchantBrand = DbHelper.ExecuteTable(sqlitem);
            //付款信息
            string sqlitempay = $@" SELECT * FROM  MERCHANT_PAYMENT A WHERE 1=1 ";
            if (!Data.MERCHANTID.IsEmpty())
                sqlitempay += (" and A.MERCHANTID= " + Data.MERCHANTID);
            DataTable merchantPayment = DbHelper.ExecuteTable(sqlitempay);

            List<ORGEntity> p = DbHelper.SelectList(new ORGEntity()).OrderBy(a => a.ORGCODE).ToList();
            var treeOrg = new UIResult(TreeModel.Create(p,
                a => a.ORGCODE,
                a => new TreeModel()
                {
                    code = a.ORGCODE,
                    title = a.ORGNAME,
                    value = a.ORGID,
                    label = a.ORGNAME,
                    expand = true
                })?.ToArray());

            return new Tuple<dynamic, DataTable, dynamic, DataTable>(merchant.ToOneLine(), merchantBrand, treeOrg, merchantPayment);
        }



        /// <summary>
        /// 详情页的审核
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public string ExecData(MERCHANTEntity Data)
        {
            MERCHANTEntity mer = DbHelper.Select(Data);
            if (mer.STATUS == ((int)普通单据状态.审核).ToString())
            {
                throw new LogicException("商户(" + Data.NAME + ")已经审核不能再次审核!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                mer.VERIFY = employee.Id;
                mer.VERIFY_NAME = employee.Name;
                mer.VERIFY_TIME = DateTime.Now.ToString();
                mer.STATUS = ((int)普通单据状态.审核).ToString();
                DbHelper.Save(mer);
                Tran.Commit();
            }
            return mer.MERCHANTID;
        }
        /// <summary>
        /// 商户预收款余额
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public DataGridResult GetMerchantAccount(SearchItem item)
        {
            string sql = @"select M.*,A.NAME MERCHANTNAME,F.NAME FEE_ACCOUNTNAME,B.NAME BRANCHNAME,B.ID BRANCHID 
                             from MERCHANT_ACCOUNT M ,MERCHANT A,FEE_ACCOUNT F,BRANCH B
                            where M.MERCHANTID=A.MERCHANTID AND M.FEE_ACCOUNT_ID=F.ID AND F.BRANCHID=B.ID ";
            sql += " AND B.ID IN (" + GetPermissionSql(PermissionType.Branch) + ")";    //分店权限 by：DZK
            item.HasKey("BRANCHID", a => sql += $" and B.ID = {a}");
            item.HasKey("MERCHANTID", a => sql += $" and A.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and A.NAME  LIKE '%{a}%'");
            item.HasKey("FEE_ACCOUNT_ID", a => sql += $" and F.ID  LIKE '%{a}%'");
            sql += " ORDER BY  M.MERCHANTID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        /// <summary>
        /// 商户预收款余额明细查询
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public DataGridResult GetMerchantAccountDetail(SearchItem item)
        {
            string sql = @"select D.MERCHANTID,A.NAME MERCHANTNAME,F.NAME FEE_ACCOUNTNAME,B.NAME BRANCHNAME,
                                  B.ID BRANCHID,D.CHANGE_TIME,D.REFERTYPE,D.REFERID, 
                                  D.BALANCE ACCOUNT,D.SAVE_MONEY,D.USE_MONEY  
	                         from MERCHANT_ACCOUNT_RECORD D, MERCHANT A,FEE_ACCOUNT F,BRANCH B
                            where D.MERCHANTID=A.MERCHANTID AND D.FEE_ACCOUNT_ID=F.ID AND F.BRANCHID=B.ID ";
            sql += " AND B.ID IN (" + GetPermissionSql(PermissionType.Branch) + ")";    //分店权限 by：DZK
            item.HasKey("BRANCHID", a => sql += $" and B.ID = {a}");
            item.HasKey("MERCHANTID", a => sql += $" and A.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and A.NAME  LIKE '%{a}%'");
            item.HasKey("FEE_ACCOUNT_ID", a => sql += $" and F.ID  = {a}");
            item.HasKey("REFERTYPE", a => sql += $" and D.REFERTYPE  = {a}");
            item.HasKey("REFERID", a => sql += $" and D.REFERID  = {a}");
            item.HasDateKey("STARTTIME", a => sql += $" and trunc(D.CHANGE_TIME) >= {a}");
            item.HasDateKey("ENDTIME", a => sql += $" and trunc(D.CHANGE_TIME) <= {a}");
            sql += " ORDER BY D.MERCHANTID,D.FEE_ACCOUNT_ID,D.CHANGE_TIME DESC ";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<收款类型>("REFERTYPE", "REFERTYPENAME");
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetMerchantPayment(SearchItem item)
        {
            string sql = $@"SELECT A.*,M.NAME MERCHANTNAME FROM  MERCHANT_PAYMENT A,MERCHANT M WHERE M.MERCHANTID=A.MERCHANTID";
            item.HasKey("MERCHANTID", a => sql += $" and A.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and M.NAME  LIKE '%{a}%'");
            item.HasKey("PAYMENTID", a => sql += $" and A.PAYMENTID LIKE '%{a}%'");
            sql += " ORDER BY  A.PAYMENTID ";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public MERCHANT_PAYMENTEntity GetMerchantPayment(string mid,string paymentid)
        {
            MERCHANT_PAYMENTEntity pay = new MERCHANT_PAYMENTEntity();
            if (!string.IsNullOrEmpty(mid)&& !string.IsNullOrEmpty(paymentid)) {
                pay = DbHelper.Select(new MERCHANT_PAYMENTEntity() { MERCHANTID = mid,PAYMENTID= paymentid });
            }
            return pay;
        }
    }
}
