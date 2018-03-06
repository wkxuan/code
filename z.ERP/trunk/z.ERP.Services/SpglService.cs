using System;
using System.Collections.Generic;
using System.Data;
using z.ERP.Entities;
using z.ERP.Entities.Enum;
using z.Extensions;
using z.MVC5.Results;

namespace z.ERP.Services
{
    public class SpglService: ServiceBase
    {
        internal SpglService()
        {

        }

        public DataGridResult GetGoods(SearchItem item)
        {
            string sql = $@"SELECT * FROM GOODS G WHERE 1=1 ";
            item.HasKey("GOODSDM", a => sql += $" and G.GOODSDM = '{a}'");
            item.HasKey("BARCODE", a => sql += $" and G.BARCODE={a}");
            item.HasKey("NAME", a => sql += $" and G.NAME  LIKE '%{a}%'");
            item.HasKey("PYM", a => sql += $" and G.PYM  LIKE '%{a}%'");
            item.HasKey("CONTRACTID", a => sql += $" and G.CONTRACTID={a}");
            item.HasKey("MERCHANTID", a => sql += $" and G.MERCHANTID={a}");
            item.HasKey("KINDID", a => sql += $" and G.KINDID={a}");
            item.HasKey("TYPE", a => sql += $" and G.TYPE={a}");
            sql += " ORDER BY  GOODSDM DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }

        public Tuple<dynamic> GetGoodsDetail(GOODSEntity Data)
        {
            string sql = $@"select * from GOODS where 1=1 ";
            if (!Data.GOODSDM.IsEmpty())
                sql += (" and GOODSDM= " + Data.GOODSDM);
            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            dt.NewEnumColumns<商品类型>("TYPE", "TYPEMC");
            return new Tuple<dynamic>(dt.ToOneLine());
        }

        public string SaveGoods (GOODSEntity SaveData)
        {
            var v = GetVerify(SaveData);
            if (SaveData.GOODSID.IsEmpty())
            {
                SaveData.GOODSID = CommonService.NewINC("GOODSID");
                SaveData.GOODSDM = CommonService.NewINC("GOODSDM");
            }
                
            v.Require(a => a.GOODSDM);
            v.IsUnique(a => a.GOODSDM);
            v.Require(a => a.TYPE);
            v.Require(a => a.NAME);
            v.Require(a => a.BARCODE);
            v.Require(a => a.CONTRACTID);
            v.Require(a => a.MERCHANTID);
            v.Require(a => a.STYLE);
            v.Require(a => a.JXSL);
            v.Require(a => a.XXSL);

            SaveData.REPORTER = employee.Id;
            SaveData.REPORTER_NAME = employee.Name;
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            SaveData.STATUS = ((int)普通单据状态.未审核).ToString();


            v.Verify();
            DbHelper.Save(SaveData);

            return SaveData.GOODSDM;
        }
        
        public void DeleteGoods(List<GOODSEntity> DeleteData)
        {
            using (var Tran = DbHelper.BeginTransaction())
            {
                foreach (var goods in DeleteData)
                {
                    var v = GetVerify(goods);
                    //校验
                    DbHelper.Delete(goods);
                }
                Tran.Commit();
            }
        }
        public object ShowOneEdit(GOODSEntity Data)
        {
            string sql = $@"select * from GOODS where 1=1 ";
            if (!Data.GOODSID.IsEmpty())
                sql += (" and GOODSID= " + Data.GOODSID);
            DataTable dt = DbHelper.ExecuteTable(sql);

            var result = new
            {
                goods = dt,
            };
            return result;
        }
        

    }
}
