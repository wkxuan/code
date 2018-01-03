using System.Data;
using z.MVC5.Results;
using z.ERP.Entities;
using System.Collections.Generic;
using z.Extensions;
using System;
using z.ERP.Entities.Enum;

namespace z.ERP.Services
{
    public class ShglService : ServiceBase
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
            SaveData.STATUS = ((int)物业标记.保底租金).ToString();
            SaveData.REPORTER = employee.Id;
            SaveData.REPORTER_NAME = employee.Name;
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            v.Require(a => a.MERCHANTID);
            v.Require(a => a.NAME);
            v.IsUnique(a => a.MERCHANTID);
            v.IsUnique(a => a.NAME);
            v.Verify();

            SaveData.MERCHANT_BRAND.ForEach(shpp =>
            {
                GetVerify(shpp).Require(a => a.BRANDID);
            });
            DbHelper.Save(SaveData);
            return SaveData.MERCHANTID;
        }
    }
}
