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
using z.ERP.Entities.Procedures;
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
            resultdt.Columns.Add("ZDRQ", typeof(String));  //制单日期
            resultdt.Columns.Add("PZLB", typeof(String));  //凭证类别
            resultdt.Columns.Add("PZBH", typeof(Int32));  //凭证号
            resultdt.Columns.Add("ZDR", typeof(String));  //制单人
            resultdt.Columns.Add("DJZS", typeof(String));  //所附单据数
            resultdt.Columns.Add("KMBM", typeof(String));  //科目编码
            resultdt.Columns.Add("ZY", typeof(String));  //摘要
            resultdt.Columns.Add("PJRQ", typeof(String));  //票据日期
            resultdt.Columns.Add("BZMC", typeof(String));  //币种名称
            resultdt.Columns.Add("YBJF", typeof(String));  //原币借方
            resultdt.Columns.Add("YBDF", typeof(String));  //原币贷方
            resultdt.Columns.Add("JFJE", typeof(String));  //借方金额
            resultdt.Columns.Add("DFJE", typeof(String));  //贷方金额
            resultdt.Columns.Add("BMBM", typeof(String));  //部门编码
            resultdt.Columns.Add("ZYBM", typeof(String));  //职员编码
            resultdt.Columns.Add("KHBM", typeof(String));  //客户编码
            resultdt.Columns.Add("GYSBM", typeof(String));  //供应商编码
            resultdt.Columns.Add("XMDLBM", typeof(String));  //项目大类编码
            resultdt.Columns.Add("XMBM", typeof(String));  //项目编码
            resultdt.Columns.Add("YWY", typeof(String));  //业务员
            resultdt.Columns.Add("ZDYX9", typeof(String));  //自定义项9
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
                        if (sqltxt.EXESQLTYPE == "1")
                        {
                            //获取分录
                            List<VOUCHER_RECORDEntity> record = DbHelper.SelectList(new VOUCHER_RECORDEntity()).
                                Where(a => a.VOUCHERID == pVOUCHERID.ToString()).Where(a => a.SQLINX == sqltxt.SQLINX)
                                .OrderBy(a => a.VOUCHERID).ToList();
                            foreach (var fldata in record)
                            {
                                var fldat = fldata.SQLCOLTORECORD; //借方贷方金额
                                var wldwdat = fldata.SQLCOLTOMERCHANT;    //商户
                                var bmdat = fldata.SQLCOLTOORG;       //部门
                                var rydat = fldata.SQLCOLTOUSER;        //人员
                                var lcdat = fldata.SQLCOLTOFLOOR;        //楼层
                                var ytdat = fldata.SQLCOLTOCATEGORY;        //业态
                                DataTable dtflid = DbHelper.ExecuteTable(sqlflid, param);
                                foreach (DataRow tr in dtflid.Rows)
                                {
                                    decimal JFJE = 0;
                                    decimal DFJE = 0;
                                    if (Convert.ToInt32(fldata.TYPE) == 1)
                                    {
                                        if (string.IsNullOrEmpty(tr[fldat].ToString()))
                                        {
                                            JFJE = 0;
                                        }
                                        else
                                        {
                                            JFJE = Convert.ToDecimal(tr[fldat].ToString());
                                        }
                                        
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(tr[fldat].ToString()))
                                        {
                                            DFJE = 0;
                                        }
                                        else
                                        {
                                            DFJE = Convert.ToDecimal(tr[fldat].ToString());
                                        }                                        
                                    }                                                                        
                                    string MERCHANTID = string.Empty;
                                    string ORGDM = string.Empty;
                                    string RYDM = string.Empty;
                                    string LCSJ = string.Empty;
                                    string YTSJ = string.Empty;
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
                                    if (!string.IsNullOrEmpty(lcdat))
                                    {
                                        LCSJ = tr[lcdat].ToString();
                                    }
                                    if (!string.IsNullOrEmpty(ytdat))
                                    {
                                        YTSJ = tr[ytdat].ToString();
                                    }
                                    List<VOUCHER_RECORD_PZKMEntity> pzkm = DbHelper.SelectList(new VOUCHER_RECORD_PZKMEntity()).
                                        Where(e => e.VOUCHERID == pVOUCHERID.ToString()).Where(a => a.RECORDID == fldata.RECORDID).
                                        OrderBy(b => b.INX).ToList();
                                    string PZMK = "";
                                    foreach (var kmata in pzkm)
                                    {
                                        if (Convert.ToInt32(kmata.SQLBJ) == 2)
                                        {
                                            PZMK += kmata.DESCRIPTION;
                                        }
                                        else
                                        {
                                            PZMK += tr[kmata.SQLCOLTORECORD].ToString();
                                        }
                                    }
                                    List<VOUCHER_RECORD_ZYEntity> pzzy = DbHelper.SelectList(new VOUCHER_RECORD_ZYEntity()).
                                        Where(e => e.VOUCHERID == pVOUCHERID.ToString()).Where(a => a.RECORDID == fldata.RECORDID).
                                        OrderBy(b => b.INX).ToList();
                                    string ZY = "";
                                    foreach (var zydata in pzzy)
                                    {
                                        if (Convert.ToInt32(zydata.SQLBJ) == 2)
                                        {
                                            ZY += zydata.DESCRIPTION;
                                        }
                                        else
                                        {
                                            ZY += tr[zydata.SQLCOLTORECORD].ToString();
                                        }
                                    }
                                    DataRow rowNew = resultdt.NewRow();
                                    rowNew["PZID"] = iPZBH;   //凭证ID
                                    rowNew["YEAR"] = Year;  //会计年
                                    rowNew["KJQJ"] = MM;  //会计期间
                                    rowNew["ZDRQ"] = DateTime.Now.Date.ToString("yyyy-MM-dd");  //制单日期
                                    rowNew["PZLB"] = "记";  //凭证类别
                                    rowNew["PZBH"] = iPZBH;  //凭证号
                                    rowNew["ZDR"] = employee.Name;  //制单人
                                    rowNew["DJZS"] = "";  //所附单据数
                                    rowNew["KMBM"] = PZMK;  //rowNew[
                                    rowNew["ZY"] = ZY;  //摘要
                                    rowNew["PJRQ"] = DateTime.Now.Date.ToString("yyyy-MM-dd");  //票据日期
                                    rowNew["BZMC"] = "人民币";  //币种名称
                                    rowNew["YBJF"] = "";  //原币借方
                                    rowNew["YBDF"] = "";  //原币贷方
                                    rowNew["JFJE"] = (JFJE == 0 ? "" : JFJE.ToString());  //借方金额
                                    rowNew["DFJE"] = (DFJE == 0 ? "" : DFJE.ToString());  //贷方金额
                                    rowNew["BMBM"] = ORGDM;  //部门编码
                                    rowNew["ZYBM"] = RYDM;  //职员编码
                                    rowNew["KHBM"] = "";  //客户编码
                                    rowNew["GYSBM"] = MERCHANTID;  //供应商编码
                                    rowNew["XMDLBM"] = "";  //项目大类编码
                                    rowNew["XMBM"] = YTSJ;  //项目编码
                                    rowNew["YWY"] = "";  //业务员
                                    rowNew["ZDYX9"] = LCSJ;  //自定义项9
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
                        else if (sqltxt.EXESQLTYPE == "2")
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
                resultdt.TableName = "resultdt";
                return GetExport("凭证导出", a =>
                {
                    a.SetTable(resultdt);
                });
            }
            else
                return "未导出数据";           
        }
        public DataGridResult GetVoucherList(SearchItem item)
        {
            string sql = $@"SELECT L.* " +
                " FROM VOUCHER L" +
                "  WHERE 1=1  ";
            item.HasKey("VOUCHERID", a => sql += $" and L.VOUCHERID = {a}");
            item.HasKey("VOUCHERNAME", a => sql += $" and L.VOUCHERNAME like '%{a}%'");
            item.HasKey("STATUS", a => sql += $" and L.STATUS={a}");
            item.HasKey("REPORTER", a => sql += $" and L.REPORTER={a}");
            item.HasDateKey("REPORTER_TIME_START", a => sql += $" and L.REPORTER_TIME>={a}");
            item.HasDateKey("REPORTER_TIME_END", a => sql += $" and L.REPORTER_TIME<={a}");
            item.HasKey("VERIFY", a => sql += $" and L.VERIFY={a}");
            item.HasDateKey("VERIFY_TIME_START", a => sql += $" and L.VERIFY_TIME>={a}");
            item.HasDateKey("VERIFY_TIME_END", a => sql += $" and L.VERIFY_TIME<={a}");
            sql += " ORDER BY  L.VOUCHERID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<账单类型>("VOUCHERTYPE", "VOUCHERTYPEMC");
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }

        public void DeleteVoucher(List<VOUCHEREntity> DeleteData)
        {
            foreach (var item in DeleteData)
            {
                VOUCHEREntity Data = DbHelper.Select(item);
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

        public string SaveVoucher(VOUCHEREntity SaveData)
        {
            var v = GetVerify(SaveData);
            if (SaveData.VOUCHERID.IsEmpty())
                SaveData.VOUCHERID = NewINC("VOUCHER");
            SaveData.STATUS = ((int)普通单据状态.未审核).ToString();
            SaveData.REPORTER = employee.Id;
            SaveData.REPORTER_NAME = employee.Name;
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            SaveData.VERIFY = employee.Id;
            v.Require(a => a.VOUCHERID);
            v.Require(a => a.VOUCHERNAME);
            v.Verify();

            using (var Tran = DbHelper.BeginTransaction())
            {
                SaveData.VOUCHER_MAKESQL?.ForEach(item =>
                {
                    GetVerify(item).Require(a => a.SQLINX);
                });
                DbHelper.Save(SaveData);

                Tran.Commit();
            }
            return SaveData.VOUCHERID;
        }

        public Tuple<dynamic, DataTable, DataTable, DataTable, DataTable> GetVoucherElement(VOUCHEREntity Data)
        {
            string sql = $@"SELECT A.* "
                        + "FROM VOUCHER A "
                        + "WHERE 1=1 ";
            if (!Data.VOUCHERID.IsEmpty())
                sql += (" AND A.VOUCHERID= " + Data.VOUCHERID);
            DataTable voucher = DbHelper.ExecuteTable(sql);
            voucher.NewEnumColumns<账单类型>("VOUCHERTYPE", "VOUCHERTYPEMC");
            voucher.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");

            string sqlvouchersql = $@"SELECT M.*" +
                " FROM VOUCHER_MAKESQL M" +
                " where  1=1";
            if (!Data.VOUCHERID.IsEmpty())
                sqlvouchersql += (" and M.VOUCHERID= " + Data.VOUCHERID);
            DataTable vouchersql = DbHelper.ExecuteTable(sqlvouchersql);

            string sqlvoucherrecord = $@"SELECT M.*" +
                " FROM VOUCHER_RECORD M" +
                " where  1=1";
            if (!Data.VOUCHERID.IsEmpty())
                sqlvoucherrecord += (" and M.VOUCHERID= " + Data.VOUCHERID);
            DataTable voucherrecord = DbHelper.ExecuteTable(sqlvoucherrecord);

            string sqlvoucherrecordpzkm = $@"SELECT M.*" +
                " FROM VOUCHER_RECORD_PZKM M" +
                " where  1=1";
            if (!Data.VOUCHERID.IsEmpty())
                sqlvoucherrecordpzkm += (" and M.VOUCHERID= " + Data.VOUCHERID);
            DataTable voucherrecordpzkm = DbHelper.ExecuteTable(sqlvoucherrecordpzkm);

            string sqlvoucherrecordzy = $@"SELECT M.*" +
                " FROM VOUCHER_RECORD_ZY M" +
                " where  1=1";
            if (!Data.VOUCHERID.IsEmpty())
                sqlvoucherrecordzy += (" and M.VOUCHERID= " + Data.VOUCHERID);
            DataTable voucherrecordzy = DbHelper.ExecuteTable(sqlvoucherrecordzy);

            return new Tuple<dynamic, DataTable, DataTable, DataTable, DataTable>(voucher.ToOneLine(), vouchersql, voucherrecord, voucherrecordpzkm,voucherrecordzy);
        }

        /// <summary>
        /// 费用调整单审核
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public string ExecVoucher(VOUCHEREntity Data)
        {
            VOUCHEREntity voucher = DbHelper.Select(Data);
            if (voucher.STATUS == ((int)普通单据状态.审核).ToString())
            {
                throw new LogicException("单据(" + Data.VOUCHERID + ")已经审核不能再次审核!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                Exec_BILL_ADJUST exec_billadjust = new Exec_BILL_ADJUST()
                {
                    p_BILLID = Data.VOUCHERID,
                    p_VERIFY = employee.Id
                };
                DbHelper.ExecuteProcedure(exec_billadjust);
                Tran.Commit();
            }
            return voucher.VOUCHERID;
        }
    }
}
