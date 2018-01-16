using System.Data;
using z.MVC5.Results;
using z.ERP.Entities;
using System.Collections.Generic;
using z.Extensions;
using System;
using z.ERP.Entities.Enum;

namespace z.ERP.Services
{
    public class DpglService : ServiceBase
    {
        internal DpglService()
        {
        }
        public DataGridResult GetAssetChange(SearchItem item)
        {
            string sql = $@"SELECT * FROM ASSETCHANGE WHERE 1=1 ";
            item.HasKey("BILLID", a => sql += $" and BILLID LIKE '%{a}%'");
            item.HasKey("BRANCHID", a => sql += $" and BRANCHID  LIKE '%{a}%'");
            sql += " ORDER BY  BILLID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public void DeleteAssetChange(ASSETCHANGEEntity DeleteData)
        {
            var v = GetVerify(DeleteData);
            //校验
            DbHelper.Delete(DeleteData);
        }

        public string SaveMerchant(ASSETCHANGEEntity SaveData)
        {
            var v = GetVerify(SaveData);
            if (SaveData.BILLID.IsEmpty())
                SaveData.BILLID = NewINC("ASSETCHANGE");
            SaveData.STATUS = ((int)普通单据状态.未审核).ToString();
            SaveData.REPORTER = employee.Id;
            SaveData.REPORTER_NAME = employee.Name;
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            v.Require(a => a.BILLID);
            v.Require(a => a.CHANGE_TYPE);
            v.IsUnique(a => a.BRANCHID);
            v.Verify();

            //SaveData.ASSETCHANGEITEM.ForEach(dpxx =>
            //{
            //    GetVerify(dpxx).Require(a => a.ASSETID);
            //});
            DbHelper.Save(SaveData);
            return SaveData.BILLID;
        }


        public object GetAssetChangeElement(ASSETCHANGEEntity Data)
        {
             //此处校验一次只能查询一个单号,校验单号必须存在
            string sql = $@"SELECT * FROM ASSETCHANGE WHERE 1=1 ";
            if (!Data.BILLID.IsEmpty())
                sql += (" AND BILLID= " + Data.BILLID);
            DataTable dt = DbHelper.ExecuteTable(sql);



            string sqlitem = $@"SELECT M.ASSETID,P.CODE " +
                " FROM ASSETCHANGEITEM M,SHOP P " +
                " where M.SEETID = P.SHOPID ";
            if (!Data.BILLID.IsEmpty())
                sqlitem += (" and M.BILLID= " + Data.BILLID);
            DataTable dtitem = DbHelper.ExecuteTable(sqlitem);

            var result = new
            {
                main = dt,
            };
            return result;
        }
    }
}
