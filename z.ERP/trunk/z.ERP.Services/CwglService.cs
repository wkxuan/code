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
        public string ExportPz(string Data)
        {
            var iPZBH = string.Empty;

            //DbCommand dbCommand = GetSqlStringCommond("select sum(SFJE) SFJE from wy_sfxmjfd where ZXRQ>=@pDATE1 and ZXRQ<=@pDATE2");
            //DbParameter param = (DbParameter)dbCommand.CreateParameter();
            //param.Value = "2018.10.1";
            //param.ParameterName = "@pDATE1";
            //dbCommand.Parameters.Add(param);
            //param.Value = "2018.10.1";
            //param.ParameterName = "@pDATE2";
            //dbCommand.Parameters.Add(param);

            DbParameter[] param = new DbParameter[2] {
                           new OracleParameter("pDATE1",new DateTime (2018,10,1)),
                           new OracleParameter("pDATE2",new DateTime (2018,10,1)) };
            DataTable dt = DbHelper.ExecuteTable("select sum(BILLID) SFJE from WORKITEM where PROC_TIME>=:pDATE1 and PROC_TIME<=:pDATE2", param);
            DataTable dt1 = DbHelper.ExecuteTable("select sum(BILLID) SFJE from WORKITEM where PROC_TIME>=:pDATE1 and PROC_TIME<=:pDATE2",
                new OracleParameter("pDATE1", new DateTime(2018, 10, 1)),
                           new OracleParameter("pDATE2", new DateTime(2018, 10, 1))
                           );




            DbParameter selectPara1 = new OracleParameter("@pDATE1", OracleDbType.Varchar2, 40);
            DbParameter selectPara2 = new OracleParameter("@pDATE2", OracleDbType.Varchar2, 40);

            selectPara1.Value = "2018.10.1";
            selectPara2.Value = "2018.10.2";


            param[0] = selectPara1;
            param[1] = selectPara2;

            var pzparme = new
            {
                pDATE1 = "2018.10.1",
                pDATE2 = "2018.10.1",
                pPZLB = "XXXX",
                pPZRQ = System.DateTime.Now,
                pCWNY = "201810",
                //pJLBH1 = res.KSDH,
                //pJLBH2 = res.JSDH,
                //pJLBH_IN = res.PJLBH,
                pSYSDATE = System.DateTime.Now
            };
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
                                var bmdat = fldata.SQLCOLTOMERCHANT;    //商户
                                var wldwdat = fldata.SQLCOLTOORG;       //部门
                                var fzdat = fldata.SQLCOLTOUSER;        //人员

                                DataTable dtflid = DbHelper.ExecuteTable(sqlflid, param);
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

            return iPZBH;
        }
    }
}
