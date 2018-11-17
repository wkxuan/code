using System;
using System.Collections.Generic;
using System.Data;
using z.ERP.Entities;
using z.ERP.Entities.Enum;
using z.Exceptions;
using z.Extensions;
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
            item.HasKey("REPORTER", a => sql += $" and REPORTER = '{a}'");
            item.HasKey("VERIFY", a => sql += $" and VERIFY = '{a}'");
            item.HasArrayKey("STATUS", a => sql += $" and STATUS in ( { a.SuperJoin(",", b => "'" + b + "'") } ) ");
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

        public string ExecData(ENERGY_REGISTEREntity Data)
        {
            ENERGY_REGISTEREntity brand = DbHelper.Select(Data);
            if (brand.STATUS == ((int)普通单据状态.审核).ToString())
            {
                throw new LogicException("单据(" + Data.BILLID + ")已经审核不能再次审核!");
            }

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
                brand.VERIFY = employee.Id;
                brand.VERIFY_NAME = employee.Name;
                brand.VERIFY_TIME = DateTime.Now.ToString();
                brand.STATUS = ((int)普通单据状态.审核).ToString();
                DbHelper.Save(brand);
                Notes(nameof(ENERGY_REGISTEREntity), brand.BILLID, $"已审核");
                Tran.Commit();
            }
            return brand.BILLID;
        }

        public Tuple<dynamic, DataTable> GetRegisterDetail(ENERGY_REGISTEREntity Data)
        {
            if (Data.BILLID.IsEmpty())
            {
                throw new LogicException("请确认记录编号!");
            }
            string sql = $@"select * from ENERGY_REGISTER where 1=1 ";
            if (!Data.BILLID.IsEmpty())
                sql += (" and BILLID= " + Data.BILLID);
            DataTable dt = DbHelper.ExecuteTable(sql);

            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");

            string sqlitem = $@"SELECT M.*,E.FILECODE,E.FILENAME,P.CODE,P.NAME " +
                " FROM ENERGY_REGISTER_ITEM M,ENERGY_FILES E,SHOP P " +
                " where M.FILEID = E.FILEID and M.SHOPID = P.SHOPID";
            if (!Data.BILLID.IsEmpty())
                sqlitem += (" and BILLID= " + Data.BILLID);
            DataTable dtitem = DbHelper.ExecuteTable(sqlitem);

            return new Tuple<dynamic, DataTable>(dt.ToOneLine(), dtitem);
        }

        public DataGridResult GetComplainDept(SearchItem item)
        {
            string sql = $@"select ID,NAME from COMPLAINDEPT where 1=1";
            item.HasKey("ID", a => sql += $" and ID = '{a}'");
            sql += "order by ID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public DataGridResult GetComplainType(SearchItem item)
        {
            string sql = $@"select ID,NAME from COMPLAINTYPE where 1=1";
            item.HasKey("ID", a => sql += $" and ID = '{a}'");
            sql += "order by ID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
    }
}
