using System.Data;
using z.MVC5.Results;
using z.ERP.Entities;
using System.Collections.Generic;
using z.Extensions;
using System;
using z.ERP.Entities.Enum;
using z.Exceptions;
using System.Linq;

namespace z.ERP.Services
{
    public class ShglService : ServiceBase
    {
        internal ShglService()
        {
        }
        /// <summary>
        /// 列表页的查询
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public DataGridResult GetMerchant(SearchItem item)
        {
            string sql = $@"SELECT * FROM MERCHANT WHERE 1=1 ";
            item.HasKey("MERCHANTID", a => sql += $" and MERCHANTID = '{a}'");
            item.HasKey("NAME", a => sql += $" and NAME  LIKE '%{a}%'");
            item.HasKey("SH", a => sql += $" and SH={a}");
            item.HasKey("BANK", a => sql += $" and BANK={a}");
            sql += " ORDER BY  MERCHANTID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }

        /// <summary>
        /// 列表页的删除,可以批量删除
        /// </summary>
        /// <param name="DeleteData"></param>
        public void DeleteMerchant(List<MERCHANTEntity> DeleteData)
        {
            foreach (var mer in DeleteData)
            {
                MERCHANTEntity Data = DbHelper.Select(mer);
                if (Data.STATUS == ((int)普通单据状态.审核).ToString())
                {
                    throw new LogicException("商户(" + Data.NAME + ")已经审核不能删除!");
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

        /// <summary>
        /// 编辑页的保存
        /// </summary>
        /// <param name="SaveData"></param>
        /// <returns></returns>
        public string SaveMerchant(MERCHANTEntity SaveData)
        {
            //因为商户可以审核之后还可以修改所以这里要分开处理通常如何判断处理应该便可
            //MERCHANTEntity mer = DbHelper.Select(SaveData);
            //if (mer.STATUS == ((int)普通单据状态.审核).ToString()) {
            //    throw new LogicException("商户(" + SaveData.NAME + ")已经审核!");
            //}
            var v = GetVerify(SaveData);
            if (SaveData.MERCHANTID.IsEmpty())
            {
                SaveData.MERCHANTID = NewINC("MERCHANT");
                SaveData.STATUS = ((int)普通单据状态.未审核).ToString();
            }
            else
            {
                //这里这样写是不是麻烦了?(界面上显示审核人信息)
                MERCHANTEntity mer = DbHelper.Select(SaveData);
                SaveData.VERIFY = mer.VERIFY;
                SaveData.VERIFY_NAME = mer.VERIFY_NAME;
                SaveData.VERIFY_TIME = mer.VERIFY_TIME;
            }
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
        /// <summary>
        /// 从列表页编辑跳转到编辑页数据的展示查询
        /// 这里需要和详情页数据展示保持一直如何处理????(已经OK)
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public Tuple<dynamic, DataTable> GetMerchantElement(MERCHANTEntity Data)
        {
            if (Data.MERCHANTID.IsEmpty())
            {
                throw new LogicException("请确认商户编号!");
            }
            string sql = $@"SELECT * FROM MERCHANT WHERE 1=1 ";
            if (!Data.MERCHANTID.IsEmpty())
                sql += (" AND MERCHANTID= " + Data.MERCHANTID);
            DataTable merchant = DbHelper.ExecuteTable(sql);

             merchant.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");

            string sqlitem = $@"SELECT M.BRANDID,C.NAME,D.CATEGORYCODE,D.CATEGORYNAME " +
                " FROM MERCHANT_BRAND M,MERCHANT E,BRAND C,CATEGORY D " +
                " where M.MERCHANTID = E.MERCHANTID AND M.BRANDID=C.ID AND  C.CATEGORYID = D.CATEGORYID ";
            if (!Data.MERCHANTID.IsEmpty())
                sqlitem += (" and E.MERCHANTID= " + Data.MERCHANTID);
            DataTable merchantBrand = DbHelper.ExecuteTable(sqlitem);

            //var result = new
            //{
            //    merchant = 1,//merchant.ToOneLine(),
            //    merchantBrand = merchantBrand
            //};
            return new Tuple<dynamic, DataTable>(merchant.ToOneLine(), merchantBrand);
        }

    

        /// <summary>
        /// 详情页的审核
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public string ExecData(MERCHANTEntity Data)
        {
            MERCHANTEntity mer = DbHelper.Select(Data);
            if (mer.STATUS == ((int)普通单据状态.审核).ToString())
            {
                throw new LogicException("商户(" + Data.NAME + ")已经审核不能再次审核!");
            }
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

    }
}
