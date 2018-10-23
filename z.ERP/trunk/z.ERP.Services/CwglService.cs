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
        public string ExportPz(VOUCHER_PARAMEntity Data)
        {
            var iPZBH = string.Empty;
            Int32 pFDBH = Data.BRANCHID;
            Int32 pCWNY = Data.CWNY;
            Int32 pVOUCHERID = Data.VOUCHERID;
            DateTime pDATE1 = Data.DATE1;
            DateTime pDATE2 = Data.DATE2;
            DateTime pPZRQ = Data.PZRQ;
            Int32 Year = Data.PZRQ.Year;
            Int32 MM = Data.PZRQ.Month;


            DataTable resultdt = new DataTable();
            resultdt.Columns.Add("PZID", typeof(Int32));   //凭证ID
            resultdt.Columns.Add("YEAR", typeof(String));  //会计年
            resultdt.Columns.Add("KJQJ", typeof(String));  //会计期间
            resultdt.Columns.Add("ZDRQ", typeof(DateTime));  //制单日期
            resultdt.Columns.Add("PZLB", typeof(String));  //凭证类别
            resultdt.Columns.Add("PZBH", typeof(Int32));  //凭证号
            resultdt.Columns.Add("ZDR", typeof(String));  //制单人
            resultdt.Columns.Add("DJZS", typeof(Int32));  //所附单据数
            resultdt.Columns.Add("KMBM", typeof(String));  //科目编码
            resultdt.Columns.Add("ZY", typeof(String));  //摘要
            resultdt.Columns.Add("BZMC", typeof(String));  //币种名称
            resultdt.Columns.Add("YBJF", typeof(decimal));  //原币借方
            resultdt.Columns.Add("YBDF", typeof(decimal));  //原币贷方
            resultdt.Columns.Add("JFJE", typeof(decimal));  //借方金额
            resultdt.Columns.Add("DFJE", typeof(decimal));  //贷方金额
            resultdt.Columns.Add("BMBM", typeof(String));  //部门编码
            resultdt.Columns.Add("ZYBM", typeof(String));  //职员编码
            resultdt.Columns.Add("KHBM", typeof(String));  //客户编码
            resultdt.Columns.Add("GYSBM", typeof(String));  //供应商编码
            resultdt.Columns.Add("XMDLBM", typeof(String));  //项目大类编码
            resultdt.Columns.Add("XMBM", typeof(String));  //项目编码
            resultdt.Columns.Add("YWY", typeof(String));  //业务员
            resultdt.Columns.Add("LC", typeof(String));  //楼层
            resultdt.Columns.Add("ZDYX10", typeof(String));  //自定义项10
            resultdt.Columns.Add("ZDYX11", typeof(String));  //自定义项11
            resultdt.Columns.Add("ZDYX12", typeof(String));  //自定义项12
            resultdt.Columns.Add("ZDYX13", typeof(String));  //自定义项13
            resultdt.Columns.Add("ZDYX14", typeof(String));  //自定义项14
            resultdt.Columns.Add("ZDYX15", typeof(String));  //自定义项15
            resultdt.Columns.Add("ZDYX16", typeof(String));  //自定义项16
            resultdt.Columns.Add("XJLLXM", typeof(String));  //现金流量项目
            resultdt.Columns.Add("XJLLJFJE", typeof(String));  //现金流量借方金额
            resultdt.Columns.Add("XJLLDFJE", typeof(String));  //现金流量贷方金额

            zParameter[] param = new zParameter[4] {
                           new zParameter("pDATE1",new DateTime (pDATE1.Year,pDATE1.Month,pDATE1.Day),DbType.Date),
                           new zParameter("pDATE2",new DateTime (pDATE2.Year,pDATE2.Month,pDATE2.Day),DbType.Date),
                           new zParameter("pCWNY",pCWNY,DbType.Int32),
                           new zParameter("pFDBH",pFDBH,DbType.Int32)};            

            using (var Tran = DbHelper.BeginTransaction())
            {
                iPZBH = CommonService.NewINC("PZBH");       
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
                                var flfat = fldata.SQLCOLTORECORD; //贷方贷方金额
                                var wldwdat = fldata.SQLCOLTOMERCHANT;    //商户
                                var bmdat = fldata.SQLCOLTOORG;       //部门
                                var rydat = fldata.SQLCOLTOUSER;        //人员
                                DataTable dtflid = DbHelper.ExecuteTable(sqlflid, param);
                                foreach (DataRow tr in dtflid.Rows)
                                {                                    
                                    string JFJE = tr[fldat].ToString();
                                    string DFJE = tr[flfat].ToString();
                                    string MERCHANTID = string.Empty;
                                    string ORGDM = string.Empty;
                                    string RYDM = string.Empty;
                                    if (!string.IsNullOrEmpty(wldwdat))
                                    {
                                        MERCHANTID = tr[wldwdat].ToString();
                                    }
                                    if (!string.IsNullOrEmpty(bmdat))
                                    {
                                        ORGDM = tr[bmdat].ToString();
                                    }
                                    if (!string.IsNullOrEmpty(rydat))
                                    {
                                        RYDM = tr[rydat].ToString();
                                    }
                                    DataRow rowNew = resultdt.NewRow();
                                    rowNew["PZID"] = "";   //凭证ID
                                    rowNew["YEAR"] = Year;  //会计年
                                    rowNew["KJQJ"] = MM;  //会计期间
                                    rowNew["ZDRQ"] = DateTime.Now.Date;  //制单日期
                                    rowNew["PZLB"] = "记";  //凭证类别
                                    rowNew["PZBH"] = iPZBH;  //凭证号
                                    rowNew["ZDR"] = employee.Name;  //制单人
                                    rowNew["DJZS"] = "";  //所附单据数
                                    rowNew["KMBM"] = "";  //rowNew[
                                    rowNew["ZY"] = "";  //摘要
                                    rowNew["BZMC"] = "人民币";  //币种名称
                                    rowNew["YBJF"] = "";  //原币借方
                                    rowNew["YBDF"] = "";  //原币贷方
                                    rowNew["JFJE"] = JFJE;  //借方金额
                                    rowNew["DFJE"] = DFJE;  //贷方金额
                                    rowNew["BMBM"] = ORGDM;  //部门编码
                                    rowNew["ZYBM"] = RYDM;  //职员编码
                                    rowNew["KHBM"] = "";  //客户编码
                                    rowNew["GYSBM"] = MERCHANTID;  //供应商编码
                                    rowNew["XMDLBM"] = "";  //项目大类编码
                                    rowNew["XMBM"] = "";  //项目编码
                                    rowNew["YWY"] = "";  //业务员
                                    rowNew["LC"] = "";  //楼层
                                    rowNew["ZDYX10"] = "";  //自定义项10
                                    rowNew["ZDYX11"] = "";  //自定义项11
                                    rowNew["ZDYX12"] = "";  //自定义项12
                                    rowNew["ZDYX13"] = "";  //自定义项13
                                    rowNew["ZDYX14"] = "";  //自定义项14
                                    rowNew["ZDYX15"] = "";  //自定义项15
                                    rowNew["ZDYX16"] = "";  //自定义项16
                                    rowNew["XJLLXM"] = "";  //现金流量项目
                                    rowNew["XJLLJFJE"] = "";  //现金流量借方金额
                                    rowNew["XJLLDFJE"] = "";  //现金流量贷方金额                               
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
            if (resultdt.Rows.Count > 0)
            {
                return GetExport("凭证导出", a =>
                {
                    a.SetTable(resultdt);
                });
            }
            else
                return "未导出数据";           
        }
    }
}
