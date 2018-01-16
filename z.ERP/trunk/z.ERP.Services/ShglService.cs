﻿using System.Data;
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

        public void DeleteMerchant(MERCHANTEntity DeleteData)
        {
            var v = GetVerify(DeleteData);
            //校验
            DbHelper.Delete(DeleteData);
        }

        public string SaveMerchant(MERCHANTEntity SaveData)
        {
            var v = GetVerify(SaveData);
            if (SaveData.MERCHANTID.IsEmpty())
                SaveData.MERCHANTID = NewINC("MERCHANT");
            SaveData.STATUS = ((int)普通单据状态.未审核).ToString();
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


        public object GetMerchantElement(MERCHANTEntity Data)
        {
             //此处校验一次只能查询一个单号,校验单号必须存在
            string sql = $@"SELECT * FROM MERCHANT WHERE 1=1 ";
            if (!Data.MERCHANTID.IsEmpty())
                sql += (" AND MERCHANTID= " + Data.MERCHANTID);
            DataTable dt = DbHelper.ExecuteTable(sql);



            string sqlitem = $@"SELECT M.BRANDID,C.NAME " +
                " FROM MERCHANT_BRAND M,MERCHANT E,BRAND C " +
                " where M.MERCHANTID = E.MERCHANTID AND M.BRANDID=C.ID";
            if (!Data.MERCHANTID.IsEmpty())
                sqlitem += (" and E.MERCHANTID= " + Data.MERCHANTID);
            DataTable dtitem = DbHelper.ExecuteTable(sqlitem);

            var result = new
            {
                MERCHANTID = dt.Rows[0]["MERCHANTID"].ToString(),
                NAME = dt.Rows[0]["NAME"].ToString(),
                MERCHANT_BRAND = new dynamic[]
                {
                    new
                    {
                        BRANDID = dtitem.Rows[0]["BRANDID"].ToString(),
                        NAME = dtitem.Rows[0]["NAME"].ToString(),
                    }
                }
            };
            return result;
        }
    }
}
