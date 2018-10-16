using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DbHelper.DbDomain;
using z.ERP.Entities;
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
        //public DbCommand GetSqlStringCommond(string sqlQuery)
        //{
        //    DbCommand dbCmd = this._dbConnection.CreateCommand();

        //    dbCmd.CommandText = sqlQuery;
        //    dbCmd.CommandType = CommandType.Text;

        //    return dbCmd;
        //}
        public DbCommand GetSqlStringCommond(string sqlQuery)
        {
            DbCommand dbCmd = this._dbConnection.CreateCommand();

            dbCmd.CommandText = sqlQuery;
            dbCmd.CommandType = CommandType.Text;

            return dbCmd;
        }
        public DataGridResult ExportPz(SearchItem item)
        {
            var iPZBH = string.Empty;
            string VOUCHERID, pFDBH, pDATE1, pDATE2, pCWNY;
            item.HasKey("pDATE1", a => pDATE1 =$"{a}");
            item.HasKey("pDATE2", a => pDATE2 = $"{a}");
            item.HasKey("VOUCHERID", a => VOUCHERID = $"{a}");
            item.HasKey("BRANCHID", a => pFDBH = $"{a}");
            item.HasKey("CWNY", a => pCWNY = $"{a}");

            DbParameter[] param = new DbParameter[3] {
                           new OracleParameter("pDATE1",new DateTime (2018,10,1)),
                           new OracleParameter("pDATE2",new DateTime (2018,10,10)),
                           new OracleParameter("pCWNY",201809)
            };
            //DataTable dt = DbHelper.ExecuteTable("select sum(BILLID) SFJE from WORKITEM where PROC_TIME>=:pDATE1 and PROC_TIME<=:pDATE2", param);
            //DataTable dt1 = DbHelper.ExecuteTable("select sum(BILLID) SFJE from WORKITEM where PROC_TIME>=:pDATE1 and PROC_TIME<=:pDATE2",
            //    new OracleParameter("pDATE1", new DateTime(2018, 10, 1)),
            //               new OracleParameter("pDATE2", new DateTime(2018, 10, 10))
            //               );

            using (var Tran = DbHelper.BeginTransaction())
            {
                //iPZBH = CommonService.NewINC("PZBH");       
                //获取sql         
                List<VOUCHER_MAKESQLEntity> p = DbHelper.SelectList(new VOUCHER_MAKESQLEntity()).Where(a => a.VOUCHERID == "1")
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

            return null;
        }
    }
}
