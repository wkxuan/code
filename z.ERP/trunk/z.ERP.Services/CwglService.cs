using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DbHelper.DbDomain;
using z.DBHelper.DbDomain;
using z.ERP.Entities;
using z.ERP.Entities.Enum;
using z.Exceptions;
using z.Extensions;
using z.MVC5.Results;

namespace z.ERP.Services
{
    public class CwglService : ServiceBase
    {
        internal CwglService()
        {

        }
        private DbConnection _dbConnection = null;
        public DataGridResult GetVoucher(SearchItem item)
        {
            string sql = $@"select VOUCHERID,VOUCHERNAME  from VOUCHER order by VOUCHERID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public class PZDCMOUDLE {
            public string PZRQ { get; set;}
            public string JFJE { get; set; }
        }
        public string ExportPz(VOUCHER_PARAMEntity Data)
        {
            var iPZBH = string.Empty;
            Int32 pFDBH = Data.BRANCHID;
            Int32 pCWNY = Data.CWNY;
            Int32 pVOUCHERID = Data.VOUCHERID;
            DateTime pDATE1 = Data.DATE1;
            DateTime pDATE2 = Data.DATE2;            

            DataTable resultdt = new DataTable();
            resultdt.Columns.Add("PZRQ", typeof(String));
            resultdt.Columns.Add("JFJE", typeof(decimal));

            zParameter[] param = new zParameter[4] {
                           new zParameter("pDATE1",new DateTime (pDATE1.Year,pDATE1.Month,pDATE1.Day),DbType.Date),
                           new zParameter("pDATE2",new DateTime (pDATE2.Year,pDATE2.Month,pDATE2.Day),DbType.Date),
                           new zParameter("pCWNY",pCWNY,DbType.Int32),
                           new zParameter("pFDBH",pFDBH,DbType.Int32)};            

            using (var Tran = DbHelper.BeginTransaction())
            {
                //iPZBH = CommonService.NewINC("PZBH");       
                //获取sql         
                List<VOUCHER_MAKESQLEntity> p = DbHelper.SelectList(new VOUCHER_MAKESQLEntity()).Where(a => a.VOUCHERID == pVOUCHERID.ToString())
                    .OrderBy(a => a.VOUCHERID).ToList();
                foreach (var sqltxt in p)
                {
                    var sqlflid = sqltxt.MAKESQL; //应该循环的SQL语句
                    if (sqlflid != "" && sqlflid != null)
                    {
                        if (sqltxt.EXESQLTYPE == "S")
                        {
                            //获取分录
                            List<VOUCHER_RECORDEntity> record = DbHelper.SelectList(new VOUCHER_RECORDEntity()).
                                Where(a => a.VOUCHERID == "1").Where(a => a.SQLINX == sqltxt.SQLINX)
                                .OrderBy(a => a.VOUCHERID).ToList();
                            foreach (var fldata in record)
                            {
                                var fldat = fldata.SQLCOLTORECORD; //借方贷方金额
                                var wldwdat = fldata.SQLCOLTOMERCHANT;    //商户
                                var bmdat = fldata.SQLCOLTOORG;       //部门
                                var fzdat = fldata.SQLCOLTOUSER;        //人员

                                DataTable dtflid = DbHelper.ExecuteTable(sqlflid, param);
                                foreach (DataRow tr in dtflid.Rows)
                                {                                    
                                    string JFJE = tr[fldat].ToString();
                                    if (!string.IsNullOrEmpty(wldwdat))
                                    {
                                        string MERCHANTID = tr[wldwdat].ToString();
                                    }
                                    ;
                                    DataRow rowNew = resultdt.NewRow();
                                    rowNew["PZRQ"] = "22233";
                                    rowNew["JFJE"] = "2.0";                                    
                                    resultdt.Rows.Add(rowNew);                                    
                                }
                                //DataTable dtflid = DbHelper.ExecuteTable(sqlflid, pzparme);
                            }
                        }
                        else if (sqltxt.EXESQLTYPE == "U")
                        {
                            //DbHelper.ExecuteObject
                            // provider.ExecuteSql(sqlflid, pzparme);
                        }
                    }
                }
                Tran.Commit();
            }
            //if (resultdt.Rows.Count > 0)
            //{
            //    return GetExport("凭证导出", a =>
            //    {
            //        a.SetTable(resultdt);
            //    });
            //}
            //else
                return "未导出数据";           
        }
    }
}
