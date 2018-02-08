using System.Data;
using z.MVC5.Results;
using z.ERP.Entities;
using System.Collections.Generic;
using z.Extensions;
using System;
using z.ERP.Entities.Enum;
using z.Exceptions;


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
            item.HasKey("MERCHANTID", a => sql += $" and MERCHANTID = '{a}'");
            item.HasKey("NAME", a => sql += $" and NAME  LIKE '%{a}%'");
            item.HasKey("SH",a=>sql+=$" and SH={a}");
            item.HasKey("BANK", a => sql += $" and BANK={a}");
            sql += " ORDER BY  MERCHANTID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS","STATUSMC");
            return new DataGridResult(dt, count);
        }

        public void DeleteMerchant(List<MERCHANTEntity> DeleteData)
        {
            foreach (var mer in DeleteData)
            {
                MERCHANTEntity Data = DbHelper.Select(mer);
                if (Data.STATUS == ((int)普通单据状态.审核).ToString()) {
                    throw new LogicException("已经审核不能删除!");
                }
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                foreach (var mer in DeleteData)
                {
                    DbHelper.Delete(mer);
                }
                Tran.Commit();
            }
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
            using (var Tran = DbHelper.BeginTransaction())
            {
                SaveData.MERCHANT_BRAND?.ForEach(shpp =>
                {
                    GetVerify(shpp).Require(a => a.BRANDID);
                });
                DbHelper.Save(SaveData);

                Tran.Commit();
            }
            return SaveData.MERCHANTID;
        }
        

        public object GetMerchantElement(MERCHANTEntity Data)
        {
            //此处校验一次只能查询一个单号,校验单号必须存在
            string sql = $@"SELECT * FROM MERCHANT WHERE 1=1 ";
            if (!Data.MERCHANTID.IsEmpty())
                sql += (" AND MERCHANTID= " + Data.MERCHANTID);
            DataTable merchant = DbHelper.ExecuteTable(sql);

            string sqlitem = $@"SELECT M.BRANDID,C.NAME,D.CATEGORYCODE,D.CATEGORYNAME " +
                " FROM MERCHANT_BRAND M,MERCHANT E,BRAND C,CATEGORY D " +
                " where M.MERCHANTID = E.MERCHANTID AND M.BRANDID=C.ID AND  C.CATEGORYID = D.CATEGORYID ";
            if (!Data.MERCHANTID.IsEmpty())
                sqlitem += (" and E.MERCHANTID= " + Data.MERCHANTID);
            DataTable merchantBrand = DbHelper.ExecuteTable(sqlitem);

            var result = new
            {
                merchant,
                merchantBrand = new dynamic[] {
                   merchantBrand
                }
            };
            return result;
        }


        public string ExecData(MERCHANTEntity Data)
        {
            MERCHANTEntity mer = DbHelper.Select(Data);
            using (var Tran = DbHelper.BeginTransaction())
            {
                mer.VERIFY = employee.Id;
                mer.VERIFY_NAME = employee.Name;
                mer.VERIFY_TIME = DateTime.Now.ToString();
                mer.STATUS = ((int)普通单据状态.审核).ToString();
                DbHelper.Save(mer);
                Tran.Commit();
            }
            return mer.MERCHANTID;
        }


        public object GetBrand(BRANDEntity Data)
        {
            string sql = " SELECT  A.NAME,B.CATEGORYCODE,B.CATEGORYNAME FROM BRAND A,CATEGORY B " +
                "  WHERE  A.CATEGORYID = B.CATEGORYID ";
            if (!Data.ID.IsEmpty())
                sql += (" and A.ID= " + Data.ID);
            DataTable dt = DbHelper.ExecuteTable(sql);
            return new
            {
                dt
            };
        }
    }
}
