using System.Data;
using z.MVC5.Results;
using z.ERP.Entities;
using System.Collections.Generic;
using z.Extensions;
using System;

namespace z.ERP.Services
{
    public class ShglService: ServiceBase
    {
        internal ShglService()
        {
        }
        public DataGridResult GetMerchant(SearchItem item)
        {
            string sql = $@"SELECT * FROM MERCHANT WHERE 1=1 ";
            item.HasKey("MERCHANTID", a => sql += $" and MERCHANTID LIKE '%{a}%'");
            item.HasKey("NAME", a => sql += $" and NAME  LIKE '%{a}%'");
            sql += " ORDER BY  MERCHANTID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public void DeleteMerchant(List<MERCHANTEntity> DeleteData)
        {
            foreach (var mer in DeleteData)
            {
                var v = GetVerify(mer);
                //校验
                DbHelper.Delete(mer);
            }
        }

        public string SaveMerchant(MERCHANTEntity SaveData)
        {
            var v = GetVerify(SaveData);
            if (SaveData.MERCHANTID.IsEmpty())
                SaveData.MERCHANTID = NewINC("MERCHANT");
            SaveData.STATUS = "0";
            SaveData.REPORTER = "1";
            SaveData.REPORTER_NAME = "测试人员";
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            v.Require(a => a.MERCHANTID);
            v.Require(a => a.NAME);
            v.IsUnique(a => a.MERCHANTID);
            v.IsUnique(a => a.NAME);
            v.Verify();

            foreach (var shpp in SaveData.MERCHANT_BRAND)
            {
                var w = GetVerify(shpp);
                shpp.MERCHANTID = SaveData.MERCHANTID;
                w.Require(a => a.MERCHANTID);
                w.Require(a => a.BRANDID);
                //DbHelper.Delete(shpp);
                //DbHelper.Insert(shpp);
            }
            DbHelper.Save(SaveData);
            return SaveData.MERCHANTID;
        }
    }
}
