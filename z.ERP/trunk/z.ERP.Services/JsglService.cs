﻿using System.Data;
using z.MVC5.Results;
using z.ERP.Entities;
using System.Collections.Generic;
using z.Extensions;
using System;
using z.ERP.Entities.Enum;
using z.Exceptions;
using z.SSO.Model;
using z.ERP.Entities.Procedures;
using z.ERP.Entities.Auto;

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
            item.HasDateKey("REPORTER_TIME_START", a => sql += $" and L.REPORTER_TIME>={a}");
            item.HasDateKey("REPORTER_TIME_END", a => sql += $" and L.REPORTER_TIME<={a}");
            item.HasKey("VERIFY", a => sql += $" and L.VERIFY={a}");
            item.HasDateKey("VERIFY_TIME_START", a => sql += $" and L.VERIFY_TIME>={a}");
            item.HasDateKey("VERIFY_TIME_END", a => sql += $" and L.VERIFY_TIME<={a}");
            sql += " ORDER BY  L.BILLID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<结算单状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetBillReturnList(SearchItem item)
        {
            string sql = $@"SELECT L.*,B.NAME BRANCHNAME,D.MERCHANTID,D.NAME MERCHANTNAME " +
                " FROM BILL_RETURN L,BRANCH B ,CONTRACT C,MERCHANT D " +
                "  WHERE L.BRANCHID = B.ID and L.CONTRACTID=C.CONTRACTID(+) and C.MERCHANTID = D.MERCHANTID(+) ";
            item.HasKey("BILLID", a => sql += $" and L.BILLID = {a}");
            item.HasKey("STATUS", a => sql += $" and L.STATUS={a}");
            item.HasKey("REPORTER", a => sql += $" and L.REPORTER={a}");
            item.HasDateKey("REPORTER_TIME_START", a => sql += $" and L.REPORTER_TIME>={a}");
            item.HasDateKey("REPORTER_TIME_END", a => sql += $" and L.REPORTER_TIME<={a}");
            item.HasKey("VERIFY", a => sql += $" and L.VERIFY={a}");
            item.HasDateKey("VERIFY_TIME_START", a => sql += $" and L.VERIFY_TIME>={a}");
            item.HasDateKey("VERIFY_TIME_END", a => sql += $" and L.VERIFY_TIME<={a}");
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
                        + "WHERE A.BRANCHID=B.ID and A.CONTRACTID=C.CONTRACTID(+) and C.MERCHANTID = D.MERCHANTID(+) ";
            if (!Data.BILLID.IsEmpty())
                sql += (" AND BILLID= " + Data.BILLID);
            DataTable billReturn = DbHelper.ExecuteTable(sql);
            billReturn.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");

            string sqlitem = $@"SELECT M.*,B.MUST_MONEY,B.RECEIVE_MONEY " +
                " FROM BILL_RETURN_ITEM M，BILL B " +
                " where M.FINAL_BILLID=B.BILLID(+) ";
            if (!Data.BILLID.IsEmpty())
                sqlitem += (" and M.BILLID= " + Data.BILLID);
            DataTable billReturnitem = DbHelper.ExecuteTable(sqlitem);

            return new Tuple<dynamic, DataTable>(billReturn.ToOneLine(), billReturnitem);
        }
        /// <summary>
        /// 保证金返还审核
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
            string sql = $@"SELECT L.*,B.NAME BRANCHNAME,T.NAME MERCHANTNAME, " +
              " nvl(L.JE_16,0) +nvl(L.ZZSJE_16,0) JSHJ_16,nvl(L.JE_10,0) +nvl(L.ZZSJE_10,0) JSHJ_10,nvl(L.JE_QT,0) +nvl(L.ZZSJE_QT,0) JSHJ_QT," +
              " nvl(L.JE_16,0)+nvl(L.JE_10,0)+nvl(L.JE_QT,0) JKHJ,nvl(L.ZZSJE_16,0)+nvl(L.ZZSJE_10,0)+nvl(L.ZZSJE_QT,0) ZZSJEHJ," +
              " nvl(L.JE_16,0)+nvl(L.JE_10,0)+nvl(L.JE_QT,0) + nvl(L.ZZSJE_16,0)+nvl(L.ZZSJE_10,0)+nvl(L.ZZSJE_QT,0) JSHJ, " +
              " nvl(L.JE_16,0)+nvl(L.JE_10,0)+nvl(L.JE_QT,0) + nvl(L.ZZSJE_16,0)+nvl(L.ZZSJE_10,0)+nvl(L.ZZSJE_QT,0)-nvl(L.KKJE,0) SJFKJE" +
              " FROM JOIN_BILL L,BRANCH B ,MERCHANT T " +
              "  WHERE L.BRANCHID = B.ID AND L.MERCHANTID = T.MERCHANTID";
            if (!Data.BILLID.IsEmpty())
                sql += (" and L.BILLID= " + Data.BILLID);
            DataTable dt = DbHelper.ExecuteTable(sql);

            string goodssql = "select S.*,G.GOODSDM,G.NAME from JOIN_BILL_GOODS S,GOODS G where S.GOODSID=G.GOODSID ";
            if (!Data.BILLID.IsEmpty())
                goodssql += (" and S.BILLID= " + Data.BILLID);
            DataTable dtgoods = DbHelper.ExecuteTable(goodssql);

            string trimsql = " select S.*,F.NAME,F.TYPE from JOIN_BILL_TRINM S,FEESUBJECT F where S.TRIMID=F.TRIMID ";
            if (!Data.BILLID.IsEmpty())
                goodssql += (" and S.BILLID= " + Data.BILLID);
            DataTable dttrim = DbHelper.ExecuteTable(trimsql);

            var result = new
            {
                joinbill = dt,
                bill_goods = new dynamic[] {
                   dtgoods
                },
                bill_trim = new dynamic[] {
                   dttrim
                },

            };
            return result;
        }

        public DataGridResult GetBillAdjustList(SearchItem item)
        {
            string sql = $@"SELECT L.*,B.NAME BRANCHNAME " +
                " FROM BILL_ADJUST L,BRANCH B " +
                "  WHERE L.BRANCHID = B.ID  ";
            item.HasKey("BILLID", a => sql += $" and L.BILLID = {a}");
            item.HasKey("STATUS", a => sql += $" and L.STATUS={a}");
            item.HasKey("REPORTER", a => sql += $" and L.REPORTER={a}");
            item.HasDateKey("REPORTER_TIME_START", a => sql += $" and L.REPORTER_TIME>={a}");
            item.HasDateKey("REPORTER_TIME_END", a => sql += $" and L.REPORTER_TIME<={a}");
            item.HasKey("VERIFY", a => sql += $" and L.VERIFY={a}");
            item.HasDateKey("VERIFY_TIME_START", a => sql += $" and L.VERIFY_TIME>={a}");
            item.HasDateKey("VERIFY_TIME_END", a => sql += $" and L.VERIFY_TIME<={a}");
            sql += " ORDER BY  L.BILLID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }

        public void DeleteBillAdjust(List<BILL_ADJUSTEntity> DeleteData)
        {
            foreach (var item in DeleteData)
            {
                BILL_ADJUSTEntity Data = DbHelper.Select(item);
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

        public string SaveBillAdjust(BILL_ADJUSTEntity SaveData)
        {
            var v = GetVerify(SaveData);
            if (SaveData.BILLID.IsEmpty())
                SaveData.BILLID = NewINC("BILL_ADJUST");
            SaveData.STATUS = ((int)普通单据状态.未审核).ToString();
            SaveData.REPORTER = employee.Id;
            SaveData.REPORTER_NAME = employee.Name;
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            SaveData.VERIFY = employee.Id;
            v.Require(a => a.BILLID);
            v.Require(a => a.BRANCHID);
            v.Verify();

            using (var Tran = DbHelper.BeginTransaction())
            {
                SaveData.BILL_ADJUST_ITEM?.ForEach(item =>
                {
                    GetVerify(item).Require(a => a.TERMID);
                    GetVerify(item).Require(a => a.CONTRACTID);
                });
                DbHelper.Save(SaveData);

                Tran.Commit();
            }
            return SaveData.BILLID;
        }

        public Tuple<dynamic, DataTable> GetBillAdjustElement(BILL_ADJUSTEntity Data)
        {
            string sql = $@"SELECT A.*,B.NAME BRANCHNAME "
                        + "FROM BILL_ADJUST A,BRANCH B "
                        + "WHERE A.BRANCHID=B.ID ";
            if (!Data.BILLID.IsEmpty())
                sql += (" AND A.BILLID= " + Data.BILLID);
            DataTable billAdjust = DbHelper.ExecuteTable(sql);
            billAdjust.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");

            string sqlitem = $@"SELECT M.*,D.NAME MERCHANTNAME,F.NAME TERMNAME " +
                " FROM BILL_ADJUST_ITEM M ,CONTRACT C,MERCHANT D,FEESUBJECT F" +
                " where  M.CONTRACTID=C.CONTRACTID and C.MERCHANTID = D.MERCHANTID and M.TERMID=F.TRIMID";
            if (!Data.BILLID.IsEmpty())
                sqlitem += (" and M.BILLID= " + Data.BILLID);
            DataTable billAdjustitem = DbHelper.ExecuteTable(sqlitem);

            return new Tuple<dynamic, DataTable>(billAdjust.ToOneLine(), billAdjustitem);
        }

        /// <summary>
        /// 费用调整单审核
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public string ExecBillAdjust(BILL_ADJUSTEntity Data)
        {
            BILL_ADJUSTEntity billAdjust = DbHelper.Select(Data);
            if (billAdjust.STATUS == ((int)普通单据状态.审核).ToString())
            {
                throw new LogicException("单据(" + Data.BILLID + ")已经审核不能再次审核!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                Exec_BILL_ADJUST exec_billadjust = new Exec_BILL_ADJUST()
                {
                    p_BILLID = Data.BILLID,
                    p_VERIFY = employee.Id
                };
                DbHelper.ExecuteProcedure(exec_billadjust);
                Tran.Commit();
            }
            return billAdjust.BILLID;
        }

        public DataGridResult GetBillObtainList(SearchItem item)
        {
            string sql = $@"SELECT L.*,B.NAME BRANCHNAME,C.NAME MERCHANTNAME " +
                " FROM BILL_OBTAIN L,BRANCH B,MERCHANT C " +
                "  WHERE L.BRANCHID = B.ID and L.MERCHANTID=C.MERCHANTID(+)  ";
            item.HasKey("TYPE", a => sql += $" and L.TYPE = {a}");
            item.HasKey("BILLID", a => sql += $" and L.BILLID = {a}");
            item.HasKey("STATUS", a => sql += $" and L.STATUS={a}");
            item.HasKey("MERCHANTID", a => sql += $" and L.MERCHANTID={a}");
            item.HasKey("REPORTER", a => sql += $" and L.REPORTER={a}");
            item.HasKey("REPORTER_NAME", a => sql += $" and L.REPORTER_NAME  LIKE '%{a}%'");
            item.HasDateKey("REPORTER_TIME_START", a => sql += $" and L.REPORTER_TIME>={a}");
            item.HasDateKey("REPORTER_TIME_END", a => sql += $" and L.REPORTER_TIME<={a}");
            item.HasKey("VERIFY", a => sql += $" and L.VERIFY={a}");
            item.HasKey("VERIFY_NAME", a => sql += $" and L.VERIFY_NAME  LIKE '%{a}%'");
            item.HasDateKey("VERIFY_TIME_START", a => sql += $" and L.VERIFY_TIME>={a}");
            item.HasDateKey("VERIFY_TIME_END", a => sql += $" and L.VERIFY_TIME<={a}");
            sql += " ORDER BY  L.BILLID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }

        public void DeleteBillObtain(List<BILL_OBTAINEntity> DeleteData)
        {
            foreach (var item in DeleteData)
            {
                BILL_OBTAINEntity Data = DbHelper.Select(item);
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

        public string SaveBillObtain(BILL_OBTAINEntity SaveData)
        {
            var v = GetVerify(SaveData);
            if (SaveData.BILLID.IsEmpty())
                SaveData.BILLID = NewINC("BILL_OBTAIN");
            SaveData.STATUS = ((int)普通单据状态.未审核).ToString();
            SaveData.REPORTER = employee.Id;
            SaveData.REPORTER_NAME = employee.Name;
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            SaveData.VERIFY = employee.Id;
            v.Require(a => a.BILLID);
            v.Require(a => a.BRANCHID);
            v.Require(a => a.MERCHANTID);
            v.Verify();

            using (var Tran = DbHelper.BeginTransaction())
            {
                SaveData.BILL_OBTAIN_ITEM?.ForEach(item =>
                {
                    GetVerify(item).Require(a => a.FINAL_BILLID);
                });
                //账单发票保存
                SaveData.BILL_OBTAIN_INVOICE?.ForEach(item =>
                {
                    GetVerify(item).Require(a => a.INVOICEID);
                });
                DbHelper.Save(SaveData);

                Tran.Commit();
            }
            return SaveData.BILLID;
        }

        public Tuple<dynamic, DataTable, DataTable> GetBillObtainElement(BILL_OBTAINEntity Data)
        {
            string sql = $@"SELECT A.*,B.NAME BRANCHNAME,C.NAME MERCHANTNAME,D.NAME FKFSNAME ,F.NAME FEE_ACCOUNT_NAME "
                        + "FROM BILL_OBTAIN A,BRANCH B,MERCHANT C,FKFS D ,FEE_ACCOUNT F "
                        + "WHERE A.BRANCHID=B.ID and A.MERCHANTID = C.MERCHANTID(+) "
                        + " AND A.FKFSID=D.ID(+) AND A.FEE_ACCOUNT_ID=F.ID(+)";
            if (!Data.BILLID.IsEmpty())
                sql += (" AND A.BILLID= " + Data.BILLID);
            DataTable billObtain = DbHelper.ExecuteTable(sql);
            billObtain.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");

            string sqlitem = $@"SELECT M.*,B.NIANYUE,B.CONTRACTID,(B.MUST_MONEY-B.RECEIVE_MONEY) UNPAID_MONEY ,D.NAME TERMMC,B.YEARMONTH " +
                " FROM BILL_OBTAIN_ITEM M ,BILL B,CONTRACT C,FEESUBJECT D " +
                " where M.FINAL_BILLID=B.BILLID(+) and B.CONTRACTID=C.CONTRACTID(+) and B.TERMID=D.TRIMID(+)";
            if (!Data.BILLID.IsEmpty())
                sqlitem += (" and M.BILLID= " + Data.BILLID);
            DataTable billObtainItem = DbHelper.ExecuteTable(sqlitem);

            //发票数据
            string sqlinvoice = @"SELECT B.BILLID,B.TYPE BTYPE,M.NAME MERCHANTNAME,I.* FROM BILL_OBTAIN_INVOICE B,INVOICE I,MERCHANT M
                    WHERE B.INVOICEID=I.INVOICEID AND I.MERCHANTID=M.MERCHANTID";
            if (!Data.BILLID.IsEmpty())
                sqlinvoice += (" and B.BILLID= " + Data.BILLID);
            DataTable billObtainInvoice = DbHelper.ExecuteTable(sqlinvoice);
            billObtainInvoice.NewEnumColumns<发票类型>("TYPE", "TYPENAME");

            return new Tuple<dynamic, DataTable, DataTable>(billObtain.ToOneLine(), billObtainItem, billObtainInvoice);
        }

        /// <summary>
        /// 保证金收取审核
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public string ExecBillObtain(BILL_OBTAINEntity Data)
        {
            BILL_OBTAINEntity billObtain = DbHelper.Select(Data);
            if (billObtain.STATUS == ((int)普通单据状态.审核).ToString())
            {
                throw new LogicException("单据(" + Data.BILLID + ")已经审核不能再次审核!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                Exec_BILL_OBTAIN exec_billobtain = new Exec_BILL_OBTAIN()
                {
                    p_BILLID = Data.BILLID,
                    p_VERIFY = employee.Id
                };
                DbHelper.ExecuteProcedure(exec_billobtain);
                Tran.Commit();
            }
            return billObtain.BILLID;
        }

        public DataGridResult GetBillNoticeList(SearchItem item)
        {
            string sql = $@"SELECT L.*,B.NAME BRANCHNAME,C.MERCHANTID,D.NAME MERCHANTNAME,F.NAME FEE_ACCOUNTNAME" +
                "  FROM BILL_NOTICE L,BRANCH B,CONTRACT C,MERCHANT D,FEE_ACCOUNT F " +
                " WHERE L.BRANCHID = B.ID and L.CONTRACTID=C.CONTRACTID(+) and C.MERCHANTID=D.MERCHANTID(+)  "+
                "   AND L.FEE_ACCOUNTID = F.ID(+)";

            item.HasKey("MERCHANTID", a => sql += $" and C.MERCHANTID= {a}");
            item.HasKey("CONTRACTID", a => sql += $" and L.CONTRACTID = {a}");
            item.HasKey("BILLID", a => sql += $" and L.BILLID = {a}");
            item.HasKey("TYPE", a => sql += $" and L.TYPE={a}");
            item.HasKey("NIANYUE", a => sql += $" and L.NIANYUE={a}");
            item.HasKey("STATUS", a => sql += $" and L.STATUS={a}");
            item.HasKey("REPORTER", a => sql += $" and L.REPORTER={a}");
            item.HasKey("FEE_ACCOUNTID", a => sql += $" and L.FEE_ACCOUNTID={a}");
            item.HasDateKey("REPORTER_TIME_START", a => sql += $" and L.REPORTER_TIME>={a}");
            item.HasDateKey("REPORTER_TIME_END", a => sql += $" and L.REPORTER_TIME<={a}");
            item.HasKey("VERIFY", a => sql += $" and L.VERIFY={a}");
            item.HasDateKey("VERIFY_TIME_START", a => sql += $" and L.VERIFY_TIME>={a}");
            item.HasDateKey("VERIFY_TIME_END", a => sql += $" and L.VERIFY_TIME<={a}");
            sql += " ORDER BY  L.BILLID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            dt.NewEnumColumns<通知单类型>("TYPE", "TYPEMC");
            return new DataGridResult(dt, count);
        }

        public void DeleteBillNotice(List<BILL_NOTICEEntity> DeleteData)
        {
            foreach (var item in DeleteData)
            {
                BILL_NOTICEEntity Data = DbHelper.Select(item);
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

        public string SaveBillNotice(BILL_NOTICEEntity SaveData)
        {
            var v = GetVerify(SaveData);
            if (SaveData.BILLID.IsEmpty())
                SaveData.BILLID = NewINC("BILL_NOTICE");
            SaveData.STATUS = ((int)普通单据状态.未审核).ToString();
            SaveData.REPORTER = employee.Id;
            SaveData.REPORTER_NAME = employee.Name;
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            SaveData.VERIFY = employee.Id;
            v.Require(a => a.BILLID);
            v.Require(a => a.BRANCHID);
            v.Require(a => a.CONTRACTID);
            v.Require(a => a.TYPE);
            v.Require(a => a.FEE_ACCOUNTID);

            v.Verify();

            using (var Tran = DbHelper.BeginTransaction())
            {
                SaveData.BILL_NOTICE_ITEM?.ForEach(item =>
                {
                    GetVerify(item).Require(a => a.FINAL_BILLID);
                });
                DbHelper.Save(SaveData);

                Tran.Commit();
            }
            return SaveData.BILLID;
        }

        public Tuple<dynamic, DataTable> GetBillNoticeElement(BILL_NOTICEEntity Data)
        {
            string sql = $@"SELECT A.*,B.NAME BRANCHNAME,D.NAME MERCHANTNAME,F.NAME FEE_ACCOUNTNAME "
                        + " FROM BILL_NOTICE A,BRANCH B,CONTRACT C,MERCHANT D,FEE_ACCOUNT F "
                        + "WHERE A.BRANCHID=B.ID and A.CONTRACTID=C.CONTRACTID(+) and C.MERCHANTID=D.MERCHANTID(+)"
                        + "  AND A.FEE_ACCOUNTID=F.ID";
            if (!Data.BILLID.IsEmpty())
                sql += (" AND A.BILLID= " + Data.BILLID);
            DataTable billNotice = DbHelper.ExecuteTable(sql);
            billNotice.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            billNotice.NewEnumColumns<通知单类型>("TYPE", "TYPEMC");

            string sqlitem = $@"SELECT M.*,B.NIANYUE,B.MUST_MONEY,(B.MUST_MONEY-B.RECEIVE_MONEY) UNPAID_MONEY,C.NAME TERMMC " +
                " FROM BILL_NOTICE_ITEM M ,BILL B,FEESUBJECT C " +
                " where M.FINAL_BILLID=B.BILLID(+) and B.TERMID=C.TRIMID(+) ";
            if (!Data.BILLID.IsEmpty())
                sqlitem += (" and M.BILLID= " + Data.BILLID);
            DataTable billNoticeItem = DbHelper.ExecuteTable(sqlitem);

            return new Tuple<dynamic, DataTable>(billNotice.ToOneLine(), billNoticeItem);
        }
        public Tuple<dynamic, DataTable, DataTable> GetBillNoticePrint(BILL_NOTICEEntity Data)
        {
            string sql = $@"SELECT A.*,TO_CHAR(A.VERIFY_TIME,'YYYYMM') CZNY,B.NAME BRANCHNAME,'('||D.MERCHANTID||')'||D.NAME MERCHANTNAME,"
                 + " F.NAME PRINTNAME,F.BANK,F.ACCOUNT,"
                 + " substr(F.ADDRESS,1,instr(B.ADDRESS,';',-1)-1) ADDRESS1,substr(F.ADDRESS,instr(B.ADDRESS,';',-1)+1) ADDRESS2,"
                 + " (select min(S.SHOPDM) from CONTRACT_SHOPXX S where S.CONTRACTID=C.CONTRACTID) SHOPDM,"
                 + " (select min(BR.NAME) from CONTRACT_BRAND R,BRAND BR where R.BRANDID=BR.ID and R.CONTRACTID=C.CONTRACTID) BRANDNAME,"
                 + " (select sum(AREA_RENTABLE) from CONTRACT_SHOP S where S.CONTRACTID=C.CONTRACTID) AREA_RENTABLE,"
                 + " (select sum(Y.AMOUNT) from CONTRACT_SUMMARY Y where Y.CONTRACTID=C.CONTRACTID and Y.YEARMONTH=A.NIANYUE) AMOUNT,"
                 + " (select sum(RENTS) from CONTRACT_RENTITEM CR where CR.CONTRACTID=C.CONTRACTID and CR.YEARMONTH=A.NIANYUE) RENTS,"
                 + " (select SUM(Y.TCZJ) from CONTRACT_TCZJ Y where Y.CONTRACTID=C.CONTRACTID and Y.YEARMONTH=A.NIANYUE) KLZJ,"
                 + " (select sum(L.MUST_MONEY) from BILL_NOTICE_ITEM M,BILL L where M.BILLID=A.BILLID and M.FINAL_BILLID = L.BILLID) MUST_MONEY,"
                 + " (select sum(M.NOTICE_MONEY) from BILL_NOTICE_ITEM M,BILL L where M.BILLID=A.BILLID and M.FINAL_BILLID = L.BILLID) NOTICE_MONEY "
                 + " FROM BILL_NOTICE A,BRANCH B,CONTRACT C,MERCHANT D,FEE_ACCOUNT F "
                 + " WHERE A.BRANCHID=B.ID and A.CONTRACTID=C.CONTRACTID(+) "
                 + "   and C.MERCHANTID=D.MERCHANTID(+) and A.FEE_ACCOUNTID=F.ID(+)";
            if (!Data.BILLID.IsEmpty())
                sql += (" AND A.BILLID= " + Data.BILLID);
            DataTable billNotice = DbHelper.ExecuteTable(sql);
            billNotice.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            //  (case B.TYPE when 0 then '收费单' else '' end ) BILLTYPE,
            string sqlitem = $@"SELECT C.NAME TERMMC,TO_CHAR(B.START_DATE,'YYYY-MM-DD')||'至'||to_char(B.END_DATE,'YYYY-MM-DD') FYQJ,"+
                " SUM(B.MUST_MONEY) MUST_MONEY,SUM(B.MUST_MONEY-B.RECEIVE_MONEY) UNPAID_MONEY,SUM(M.NOTICE_MONEY) NOTICE_MONEY" +
                " FROM BILL_NOTICE_ITEM M ,BILL B,FEESUBJECT C " +
                " where M.FINAL_BILLID=B.BILLID(+) and B.TERMID=C.TRIMID(+) ";
            if (!Data.BILLID.IsEmpty())
                sqlitem += (" and M.BILLID= " + Data.BILLID);
            sqlitem += " GROUP BY C.NAME,TO_CHAR(B.START_DATE,'YYYY-MM-DD')||'至'||to_char(B.END_DATE,'YYYY-MM-DD')";
            sqlitem += " ORDER BY 2";
            DataTable billNoticeItem = DbHelper.ExecuteTable(sqlitem);

            //添加预收款余额明细 by：Dzk
            string sqlaccount = @"SELECT M.BALANCE  FROM MERCHANT_ACCOUNT M ,CONTRACT C,BILL_NOTICE B
									WHERE  C.MERCHANTID=M.MERCHANTID AND B.CONTRACTID=C.CONTRACTID AND M.FEE_ACCOUNT_ID=B.FEE_ACCOUNTID";
            if (!Data.BILLID.IsEmpty())
                sqlaccount += (" AND B.BILLID= " + Data.BILLID);
            DataTable DTA = DbHelper.ExecuteTable(sqlaccount);

            return new Tuple<dynamic, DataTable, DataTable>(billNotice.ToOneLine(), billNoticeItem,DTA);
        }
        /// <summary>
        /// 缴费通知单审核
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public string ExecBillNotice(BILL_NOTICEEntity Data)
        {
            BILL_NOTICEEntity billNotice = DbHelper.Select(Data);
            if (billNotice.STATUS == ((int)普通单据状态.审核).ToString())
            {
                throw new LogicException("单据(" + Data.BILLID + ")已经审核不能再次审核!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                billNotice.VERIFY = employee.Id;
                billNotice.VERIFY_NAME = employee.Name;
                billNotice.VERIFY_TIME = DateTime.Now.ToString();
                billNotice.STATUS = ((int)普通单据状态.审核).ToString();
                DbHelper.Save(billNotice);
                Tran.Commit();
            }
            return billNotice.BILLID;
        }

        public Tuple<dynamic > GetJoinBillDetail(JOIN_BILLEntity Data)
        {
            string sql = $@"SELECT L.*,B.NAME BRANCHNAME,T.NAME MERCHANTNAME, " +
                          " nvl(L.JE_16,0) +nvl(L.ZZSJE_16,0) JSHJ_16,nvl(L.JE_10,0) +nvl(L.ZZSJE_10,0) JSHJ_10,nvl(L.JE_QT,0) +nvl(L.ZZSJE_QT,0) JSHJ_QT," +
                          " nvl(L.JE_16,0)+nvl(L.JE_10,0)+nvl(L.JE_QT,0) JKHJ,nvl(L.ZZSJE_16,0)+nvl(L.ZZSJE_10,0)+nvl(L.ZZSJE_QT,0) ZZSJEHJ," +
                          " nvl(L.JE_16,0)+nvl(L.JE_10,0)+nvl(L.JE_QT,0) + nvl(L.ZZSJE_16,0)+nvl(L.ZZSJE_10,0)+nvl(L.ZZSJE_QT,0) JSHJ, " +
                          " nvl(L.JE_16,0)+nvl(L.JE_10,0)+nvl(L.JE_QT,0) + nvl(L.ZZSJE_16,0)+nvl(L.ZZSJE_10,0)+nvl(L.ZZSJE_QT,0)-nvl(L.KKJE,0) SJFKJE" +
                          " FROM JOIN_BILL L,BRANCH B ,MERCHANT T " +
                          "  WHERE L.BRANCHID = B.ID AND L.MERCHANTID = T.MERCHANTID";
            if (!Data.BILLID.IsEmpty())
                sql += (" and L.BILLID= " + Data.BILLID);

            DataTable joinbill = DbHelper.ExecuteTable(sql);
            joinbill.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");




            return new Tuple<dynamic>(joinbill.ToOneLine());
        }
        
        public object ShowOneJoinDetail(JOIN_BILLEntity Data)
        {
            string goodssql = "select S.*,G.GOODSDM,G.NAME from JOIN_BILL_GOODS S,GOODS G where S.GOODSID=G.GOODSID ";
            if (!Data.BILLID.IsEmpty())
                goodssql += (" and S.BILLID= " + Data.BILLID);
            DataTable dtgoods = DbHelper.ExecuteTable(goodssql);

            string trimsql = " select S.*,F.NAME,F.TYPE from JOIN_BILL_TRINM S,FEESUBJECT F where S.TRIMID=F.TRIMID ";
            if (!Data.BILLID.IsEmpty())
                goodssql += (" and S.BILLID= " + Data.BILLID);
            DataTable dttrim = DbHelper.ExecuteTable(trimsql);

            var result = new
            {                
                bill_goods = new dynamic[] {
                   dtgoods
                },
                bill_trim = new dynamic[] {
                   dttrim
                }
            };
            return result;
        }
        //public DataGridResult GetBillObtainList(SearchItem item)
        //{
        //    string sql = $@"SELECT L.*,B.NAME BRANCHNAME,C.NAME MERCHANTNAME " +
        //        " FROM BILL_OBTAIN L,BRANCH B,MERCHANT C " +
        //        "  WHERE L.BRANCHID = B.ID and L.MERCHANTID=C.MERCHANTID(+)  " +
        //        " and L.TYPE= " + ((int)收款类型.账单收款).ToString();
        //    item.HasKey("TYPE", a => sql += $" and L.TYPE = {a}");
        //    item.HasKey("BILLID", a => sql += $" and L.BILLID = {a}");
        //    item.HasKey("STATUS", a => sql += $" and L.STATUS={a}");
        //    item.HasKey("REPORTER", a => sql += $" and L.REPORTER={a}");
        //    item.HasKey("REPORTER_TIME_START", a => sql += $" and L.REPORTER_TIME>={a}");
        //    item.HasKey("REPORTER_TIME_END", a => sql += $" and L.REPORTER_TIME<={a}");
        //    item.HasKey("VERIFY", a => sql += $" and L.VERIFY={a}");
        //    item.HasKey("VERIFY_TIME_START", a => sql += $" and L.VERIFY_TIME>={a}");
        //    item.HasKey("VERIFY_TIME_END", a => sql += $" and L.VERIFY_TIME<={a}");
        //    sql += " ORDER BY  L.BILLID DESC";
        //    int count;
        //    DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
        //    dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
        //    return new DataGridResult(dt, count);
        //}

        public Tuple<dynamic, DataTable> GetBillObtainPrint(BILL_OBTAINEntity Data)
        {
            string sql = $@"SELECT A.BILLID,A.NIANYUE,A.ALL_MONEY,A.DESCRIPTION,A.BRANCHID,A.STATUS,A.ADVANCE_MONEY, "
                        + "B.NAME BRANCHNAME,'('||A.MERCHANTID||')'||C.NAME MERCHANTNAME,F.NAME FKFS,B.ACCOUNT,B.BANK, "
                        + "(select sum(L.MUST_MONEY) from BILL_OBTAIN_ITEM M,BILL L where M.BILLID=A.BILLID and M.FINAL_BILLID = L.BILLID) MUST_MONEY "
                        + " FROM BILL_OBTAIN A,BRANCH B,MERCHANT C,FKFS F "
                        + " WHERE A.BRANCHID=B.ID and A.MERCHANTID = C.MERCHANTID(+) "
                        + "and A.FKFSID =F.ID ";
            if (!Data.BILLID.IsEmpty())
                sql += (" AND A.BILLID= " + Data.BILLID);
            DataTable billObtain = DbHelper.ExecuteTable(sql);
            billObtain.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");

            string sqlitem = $@"SELECT M.*,B.NIANYUE,B.CONTRACTID,(B.MUST_MONEY-B.RECEIVE_MONEY) UNPAID_MONEY ,D.NAME TERMMC,B.YEARMONTH " +
                " FROM BILL_OBTAIN_ITEM M ,BILL B,CONTRACT C,FEESUBJECT D " +
                " where M.FINAL_BILLID=B.BILLID(+) and B.CONTRACTID=C.CONTRACTID(+) and B.TERMID=D.TRIMID(+)";
            if (!Data.BILLID.IsEmpty())
                sqlitem += (" and M.BILLID= " + Data.BILLID);
            sqlitem += $" order by M.FINAL_BILLID";
            DataTable billObtainItem = DbHelper.ExecuteTable(sqlitem);

            return new Tuple<dynamic, DataTable>(billObtain.ToOneLine(), billObtainItem);
        }

        public object GetContract(CONTRACTEntity Data)
        {
            string sql = $@"select T.MERCHANTID,S.NAME SHMC,T.STYLE,T.JXSL*100 JXSL,T.XXSL*100 XXSL from CONTRACT T,MERCHANT S where T.MERCHANTID=S.MERCHANTID ";
            if (!Data.CONTRACTID.IsEmpty())
                sql += (" and T.CONTRACTID= " + Data.CONTRACTID);
            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.NewEnumColumns<核算方式>("STYLE", "STYLEMC");
            var result = new
            {
                contract = dt
            };

            return result;
        }
        #region 发票管理
        public DataGridResult GetInvoiceList(SearchItem item)
        {
            string sql = @"SELECT I.*,M.NAME MERCHANTNAME,1 STATUS FROM INVOICE I,MERCHANT M
                    WHERE I.MERCHANTID=M.MERCHANTID ";
            item.HasKey("INVOICEID", a => sql += $" and I.INVOICEID = {a}");
            item.HasKey("INVOICENUMBER", a => sql += $" and I.INVOICENUMBER = {a}");
            item.HasKey("TYPE", a => sql += $" and I.TYPE={a}");
            item.HasKey("MERCHANTID", a => sql += $" and M.MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and M.NAME LIKE '%{a}%'");
            item.HasDateKey("INVOICEDATE", a => sql += $" and I.INVOICEDATE={a}");
            sql += " ORDER BY  I.CREATEDATE DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<发票类型>("TYPE", "TYPENAME");
            return new DataGridResult(dt, count);
        }
        public string SaveInvoice(InvoiceEntity SaveData) {
            var v = GetVerify(SaveData);
            if (SaveData.INVOICEID.IsEmpty()) {
                SaveData.INVOICEID = NewINC("INVOICE");
                SaveData.NOVATAMOUNT = (Convert.ToDecimal(SaveData.INVOICEAMOUNT) - Convert.ToDecimal(SaveData.VATAMOUNT)).ToString();
                SaveData.CREATEUSERID= employee.Id;
                SaveData.CREATENAME = employee.Name;
                SaveData.CREATEDATE = DateTime.Now.ToString();
                v.Require(a => a.INVOICEID);
                v.Verify();
            }
            DbHelper.Save(SaveData);
            return SaveData.INVOICEID;
        }
        public void DeleteInvoice(List<InvoiceEntity> DeleteData) {
            using (var Tran = DbHelper.BeginTransaction())
            {
                foreach (var item in DeleteData)
                {
                    DbHelper.Delete(item);
                }
                Tran.Commit();
            }
        }
        public Tuple<dynamic, DataTable> ShowOneInvoiceEdit(InvoiceEntity Data) {
            string sql = @"SELECT I.*,M.NAME MERCHANTNAME FROM INVOICE I,MERCHANT M
                    WHERE I.MERCHANTID=M.MERCHANTID ";
            if (!Data.INVOICEID.IsEmpty())
                sql += (" and I.INVOICEID= " + Data.INVOICEID);
            if (!Data.INVOICENUMBER.IsEmpty())
                sql += (" and I.INVOICENUMBER= " + Data.INVOICENUMBER);
            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.NewEnumColumns<发票类型>("TYPE", "TYPENAME");
            return new Tuple<dynamic, DataTable>(dt.ToOneLine(),dt);
        }
        #endregion
    }
}
