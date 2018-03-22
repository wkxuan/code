using System.Data;
using z.MVC5.Results;
using z.ERP.Entities;
using System.Collections.Generic;
using z.Extensions;
using System;
using z.ERP.Entities.Enum;
using z.Exceptions;

namespace z.ERP.Services
{
    public class JsglService:ServiceBase
    {
        internal JsglService()
        {

        }

        public DataGridResult GetJoinBillList(SearchItem item)
        {
            string sql = $@"SELECT L.*,B.NAME BRANCHNAME,T.NAME MERCHANTNAME " +
                " FROM JOIN_BILL L,BRANCH B ,MERCHANT T " +
                "  WHERE L.BRANCHID = B.ID AND L.MERCHANTID = T.MERCHANTID";
            item.HasKey("BILLID", a => sql += $" and L.BILLID = {a}");
            item.HasKey("CONTRACTID", a => sql += $" and L.CONTRACTID={a}");
            item.HasKey("MERCHANTID", a => sql += $" and L.MERCHANTID={a}");
            item.HasKey("NIANYUE_START", a => sql += $" and L.NIANYUE>={a}");
            item.HasKey("NIANYUE_END", a => sql += $" and L.NIANYUE<={a}");
            item.HasKey("STATUS", a => sql += $" and L.STATUS={a}");            
            item.HasKey("REPORTER", a => sql += $" and L.REPORTER={a}");            
            item.HasKey("REPORTER_TIME_START", a => sql += $" and L.REPORTER_TIME>={a}");
            item.HasKey("REPORTER_TIME_END", a => sql += $" and L.REPORTER_TIME<={a}");
            item.HasKey("VERIFY", a => sql += $" and L.VERIFY={a}");
            item.HasKey("VERIFY_TIME_START", a => sql += $" and L.VERIFY_TIME>={a}");
            item.HasKey("VERIFY_TIME_END", a => sql += $" and L.VERIFY_TIME<={a}");
            sql += " ORDER BY  L.BILLID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<结算单状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetBillReturnList(SearchItem item)
        {
            string sql = $@"SELECT L.*,B.NAME BRANCHNAME,D.NAME MERCHANTNAME " +
                " FROM BILL_RETURN L,BRANCH B ,CONTRACT C,MERCHANT D " +
                "  WHERE L.BRANCHID = B.ID and L.CONTRACTID=C.CONTRACTID(+) and C.MERCHANTID = D.MERCHANTID(+) ";
            item.HasKey("BILLID", a => sql += $" and L.BILLID = {a}");
            item.HasKey("STATUS", a => sql += $" and L.STATUS={a}");
            item.HasKey("REPORTER", a => sql += $" and L.REPORTER={a}");
            item.HasKey("REPORTER_TIME_START", a => sql += $" and L.REPORTER_TIME>={a}");
            item.HasKey("REPORTER_TIME_END", a => sql += $" and L.REPORTER_TIME<={a}");
            item.HasKey("VERIFY", a => sql += $" and L.VERIFY={a}");
            item.HasKey("VERIFY_TIME_START", a => sql += $" and L.VERIFY_TIME>={a}");
            item.HasKey("VERIFY_TIME_END", a => sql += $" and L.VERIFY_TIME<={a}");
            item.HasKey("CONTRACTID", a => sql += $" and C.CONTRACTID={a}");
            item.HasKey("MERCHANTID", a => sql += $" and C.MERCHANTID={a}");
            sql += " ORDER BY  L.BILLID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }

        public void DeleteBillReturn(List<BILL_RETURNEntity> DeleteData)
        {
            foreach (var item in DeleteData)
            {
                BILL_RETURNEntity Data = DbHelper.Select(item);
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

        public string SaveBillReturn(BILL_RETURNEntity SaveData)
        {
            var v = GetVerify(SaveData);
            if (SaveData.BILLID.IsEmpty())
                SaveData.BILLID = NewINC("BILL_RETURN");
            SaveData.STATUS = ((int)普通单据状态.未审核).ToString();
            SaveData.REPORTER = employee.Id;
            SaveData.REPORTER_NAME = employee.Name;
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            SaveData.VERIFY = employee.Id;
            v.Require(a => a.BILLID);
            v.Require(a => a.CONTRACTID);
            v.Require(a => a.BRANCHID);
            v.Verify();

            using (var Tran = DbHelper.BeginTransaction())
            {
                SaveData.BILL_RETURN_ITEM?.ForEach(item =>
                {
                    GetVerify(item).Require(a => a.FINAL_BILLID);
                    GetVerify(item).Require(a => a.RETURN_MONEY);
                });
                DbHelper.Save(SaveData);

                Tran.Commit();
            }
            return SaveData.BILLID;
        }


        public Tuple<dynamic, DataTable> GetBillReturnElement(BILL_RETURNEntity Data)
        {
            //此处校验一次只能查询一个单号,校验单号必须存在
            string sql = $@"SELECT A.*,B.NAME BRANCHNAME,D.NAME MERCHANTNAME "
                        +"FROM BILL_RETURN A,BRANCH B,CONTRACT C,MERCHANT D " 
                        + "WHERE A.BRANCHID=B.ID and A.CONTRACTID=C.CONTRACTID and C.MERCHANTID = D.MERCHANTID ";
            if (!Data.BILLID.IsEmpty())
                sql += (" AND BILLID= " + Data.BILLID);
            DataTable billReturn = DbHelper.ExecuteTable(sql);
            billReturn.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");

            string sqlitem = $@"SELECT M.* " +
                " FROM BILL_RETURN_ITEM M " +
                " where 1=1 ";
            if (!Data.BILLID.IsEmpty())
                sqlitem += (" and M.BILLID= " + Data.BILLID);
            DataTable billReturnitem = DbHelper.ExecuteTable(sqlitem);

            return new Tuple<dynamic, DataTable>(billReturn.ToOneLine(), billReturnitem);
        }
        /// <summary>
        /// 资产变更审核
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public string ExecBillReturn(BILL_RETURNEntity Data)
        {
            BILL_RETURNEntity billReturn = DbHelper.Select(Data);
            if (billReturn.STATUS == ((int)普通单据状态.审核).ToString())
            {
                throw new LogicException("单据(" + Data.BILLID + ")已经审核不能再次审核!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                billReturn.VERIFY = employee.Id;
                billReturn.VERIFY_NAME = employee.Name;
                billReturn.VERIFY_TIME = DateTime.Now.ToString();
                billReturn.STATUS = ((int)普通单据状态.审核).ToString();
                DbHelper.Save(billReturn);
                Tran.Commit();
            }
            return billReturn.BILLID;
        }

        public object GetJoinBillElement(JOIN_BILLEntity Data)
        {
            string sql = $@"SELECT L.*,B.NAME BRANCHNAME,T.NAME MERCHANTNAME " +
            " FROM JOIN_BILL L,BRANCH B ,MERCHANT T " +
            "  WHERE L.BRANCHID = B.ID AND L.MERCHANTID = T.MERCHANTID";
            if (!Data.BILLID.IsEmpty())
                sql += (" and L.BILLID= " + Data.BILLID);
            DataTable dt = DbHelper.ExecuteTable(sql);

            //string sqlshop = $@"SELECT G.*,S.CODE,Y.CATEGORYCODE,Y.CATEGORYNAME " +
            //    "  FROM GOODS_SHOP G,SHOP S,CATEGORY Y  " +
            //    "  WHERE G.SHOPID=S.SHOPID AND G.CATEGORYID= Y.CATEGORYID";
            //if (!Data.GOODSID.IsEmpty())
            //    sqlshop += (" and G.GOODSID= " + Data.GOODSID);
            //DataTable dtshop = DbHelper.ExecuteTable(sqlshop);

            var result = new
            {
                joinbill = dt,
                //goods_shop = new dynamic[] {
                //   dtshop
                //}
            };
            return result;
        }        
    }
}
