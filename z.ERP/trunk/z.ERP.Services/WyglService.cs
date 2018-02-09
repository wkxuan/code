using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.ERP.Entities;
using z.ERP.Entities.Enum;
using z.Extensions;
using z.MVC5.Results;

namespace z.ERP.Services
{
    public class WyglService : ServiceBase
    {
        internal WyglService()
        {

        }
        public DataGridResult GetEnergyreGister(SearchItem item)
        {
            string sql = $@"select * from ENERGY_REGISTER where 1=1 ";
            item.HasKey("BILLID", a => sql += $" and BILLID = '{a}'");
            item.HasKey("CHECK_DATE_START", a => sql += $" and CHECK_DATE>= to_date('{a.ToDateTime().ToLocalTime()}','YYYY-MM-DD  HH24:MI:SS')");
            item.HasKey("CHECK_DATE_END", a => sql += $" and CHECK_DATE<= to_date('{a.ToDateTime().ToLocalTime()}','YYYY-MM-DD  HH24:MI:SS')");
            sql += " order by BILLID desc";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }

        public string SaveEnergyreGister(ENERGY_REGISTEREntity SaveData)
        {
            var v = GetVerify(SaveData);

            SaveData.CHECK_DATE.ToDateTime();
            if (SaveData.BILLID.IsEmpty())
            {
                SaveData.BILLID = NewINC("ENERGY_REGISTER");
            }

            SaveData.REPORTER = employee.Id;
            SaveData.REPORTER_NAME = employee.Name;
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            SaveData.STATUS = ((int)普通单据状态.未审核).ToString();

            v.Require(a => a.BILLID);
            v.Require(a => a.CHECK_DATE);
            v.Require(a => a.YEARMONTH);

            using (var tran = DbHelper.BeginTransaction())
            {
                SaveData.ENERGY_REGISTER_ITEM.ForEach(sdb =>
                {
                    GetVerify(sdb).Require(a => a.FILEID);
                    GetVerify(sdb).Require(a => a.AMOUNT);
                });
                v.Verify();
                DbHelper.Save(SaveData);

                tran.Commit();
            }
            return SaveData.BILLID;
        }


        public object GetEnergyreGisterElement(ENERGY_REGISTEREntity Data)
        {
            string sql = $@"select * from ENERGY_REGISTER where 1=1 ";
            if (!Data.BILLID.IsEmpty())
                sql += (" and BILLID= " + Data.BILLID);
            DataTable dt = DbHelper.ExecuteTable(sql);

            string sqlitem = $@"SELECT M.*,E.FILECODE,E.FILENAME,P.CODE,P.NAME " +
                " FROM ENERGY_REGISTER_ITEM M,ENERGY_FILES E,SHOP P " +
                " where M.FILEID = E.FILEID and M.SHOPID = P.SHOPID";
            if (!Data.BILLID.IsEmpty())
                sqlitem += (" and BILLID= " + Data.BILLID);
            DataTable dtitem = DbHelper.ExecuteTable(sqlitem);

            var result = new
            {
                main = dt,
                item = new dynamic[] {
                   dtitem
                }
            };
            return result;
        }
        public void DeleteEnergyreGister(List<ENERGY_REGISTEREntity> DeleteData)
        {
            using (var Tran = DbHelper.BeginTransaction())
            {
                foreach (var mer in DeleteData)
                {
                    var v = GetVerify(mer);
                    //校验
                    DbHelper.Delete(mer);
                }
                Tran.Commit();
            }
            //using (var tran = DbHelper.BeginTransaction())
            //{
            //    foreach( var en in DeleteData)
            //    {
            //        var v = GetVerify(en);

            //        DbHelper.Delete(en);
            //    }
            //    tran.Commit();
            //}
        }
        public object GetRegister(ENERGY_FILESEntity Data)
        {
            string sql = "SELECT S.FILENAME,S.SHOPID,P.CODE SHOPDM,S.VALUE_LAST,S.PRICE FROM ENERGY_FILES S,SHOP P " +
                "  where S.SHOPID = P.SHOPID ";
            if (!Data.FILEID.IsEmpty())
                sql += (" and S.FILEID= " + Data.FILEID);

            DataTable dt = DbHelper.ExecuteTable(sql);

            

            return new
            {
                dt
            };
        }

        public void ExecData(ENERGY_REGISTEREntity Data)
        {
            using (var Tran = DbHelper.BeginTransaction())
            {
                //string sql = "update ENERGY_REGISTER set VERIFY=:VERIFY,VERIFY_NAME=:VERIFY_NAME,VERIFY_TIME=:VERIFY_TIME" +
                //    " where BILLID=:BILLID";
                //string sql = " update ENERGY_REGISTER set" +
                //    " VERIFY = "+ employee.Id +
                //    " VERIFY_NAME = " + employee.Name +
                //    " VERIFY_TIME =" + DateTime.Now.ToString() +
                //    " where BILLID = " + Data.BILLID.ToString();
                //DbHelper.ExecuteNonQuery(sql);
                Data.VERIFY = employee.Id;
                Data.VERIFY_NAME = employee.Name;
                Data.VERIFY_TIME = DateTime.Now.ToString();
                DbHelper.Update(Data);
                Tran.Commit();
            }
        }
    }
}
