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

        public Tuple<dynamic, DataTable> GetGoodsDetail(GOODSEntity Data)
        {
            string sql = $@"select * from GOODS where 1=1 ";
            if (!Data.GOODSDM.IsEmpty())
                sql += (" and GOODSDM= " + Data.GOODSDM);
            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            dt.NewEnumColumns<商品类型>("TYPE", "TYPEMC");
            dt.NewEnumColumns<核算方式>("STYLE", "STYLEMC");
            string sqlshop = $@"SELECT G.*,S.CODE,Y.CATEGORYCODE,Y.CATEGORYNAME " +
                "  FROM GOODS_SHOP G,SHOP S,CATEGORY Y  " +
                "  WHERE G.SHOPID=S.SHOPID AND G.CATEGORYID= Y.CATEGORYID";
            if (!Data.GOODSID.IsEmpty())
                sqlshop += (" and G.GOODSID= " + Data.GOODSID);
            DataTable dtshop = DbHelper.ExecuteTable(sqlshop);

            return new Tuple<dynamic, DataTable>(dt.ToOneLine(), dtshop);
        }

        public string SaveGoods (GOODSEntity SaveData)
        {
            var v = GetVerify(SaveData);
            var spbmcd = 6;
            //定义随机数,条码末尾校验位
            Random sj = new Random();
            if (SaveData.GOODSID.IsEmpty())
            {
                SaveData.GOODSID = CommonService.NewINC("GOODSID");
                SaveData.GOODSDM = CommonService.NewINC("GOODSDM").PadLeft(spbmcd, '0');
            }
            if (SaveData.BARCODE.IsEmpty())
            {
                var szFormatBarcode = "21{0:D" + spbmcd + "}{1:D" + (10 - Convert.ToInt32(spbmcd)) + "}";
                var barCode = string.Format(szFormatBarcode, SaveData.GOODSDM, 0);
                SaveData.BARCODE = (barCode + sj.Next(10)).ToString();
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

            SaveData.JXSL = (SaveData.JXSL.ToDouble() / 100).ToString();
            SaveData.XXSL = (SaveData.XXSL.ToDouble() / 100).ToString();
            SaveData.REPORTER = employee.Id;
            SaveData.REPORTER_NAME = employee.Name;
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            SaveData.STATUS = ((int)普通单据状态.未审核).ToString();

            SaveData.GOODS_SHOP.ForEach(sdb =>
            {
                GetVerify(sdb).Require(a => a.GOODSID);
                GetVerify(sdb).Require(a => a.BRANCHID);
                GetVerify(sdb).Require(a => a.SHOPID);                
                GetVerify(sdb).Require(a => a.CATEGORYID);
            });
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
            string sql = $@"select G.*,M.NAME SHMC from GOODS G,MERCHANT M where G.MERCHANTID=M.MERCHANTID ";
            if (!Data.GOODSID.IsEmpty())
                sql += (" and G.GOODSID= " + Data.GOODSID);
            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.Rows[0]["JXSL"] = dt.Rows[0]["JXSL"].ToString().ToDouble() * 100;
            dt.Rows[0]["XXSL"] = dt.Rows[0]["XXSL"].ToString().ToDouble() * 100;
            dt.NewEnumColumns<核算方式>("STYLE", "STYLEMC");

            string sqlshop = $@"SELECT G.*,S.CODE,Y.CATEGORYCODE,Y.CATEGORYNAME " +
                "  FROM GOODS_SHOP G,SHOP S,CATEGORY Y  " +
                "  WHERE G.SHOPID=S.SHOPID AND G.CATEGORYID= Y.CATEGORYID";
            if (!Data.GOODSID.IsEmpty())
                sqlshop += (" and G.GOODSID= " + Data.GOODSID);
            DataTable dtshop = DbHelper.ExecuteTable(sqlshop);

            var result = new
            {
                goods = dt,
                goods_shop = new dynamic[] {
                   dtshop
                }
            };
            return result;
        }

        public object GetContract(CONTRACTEntity Data)
        {
            string sql = $@"select T.MERCHANTID,S.NAME SHMC,T.STYLE from CONTRACT T,MERCHANT S where T.MERCHANTID=S.MERCHANTID ";
            if (!Data.CONTRACTID.IsEmpty())
                sql += (" and T.CONTRACTID= " + Data.CONTRACTID);
            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.NewEnumColumns<核算方式>("STYLE", "STYLEMC");

            string sql_shop = $@"SELECT P.SHOPID,P.CATEGORYID,S.CODE,Y.CATEGORYCODE,Y.CATEGORYNAME,T.BRANCHID "
                + " FROM CONTRACT_SHOP P,SHOP S,CATEGORY Y,CONTRACT T " 
                + " WHERE  P.SHOPID=S.SHOPID and P.CATEGORYID=Y.CATEGORYID and P.CONTRACTID = T.CONTRACTID";
            if (!Data.CONTRACTID.IsEmpty())
                sql += (" and P.CONTRACTID= " + Data.CONTRACTID);
            sql += " order by S.CODE";
            DataTable shop = DbHelper.ExecuteTable(sql_shop);

            var result = new
            {
                contract = dt,
                shop = shop
            };

            return result;
        }



    }
}
