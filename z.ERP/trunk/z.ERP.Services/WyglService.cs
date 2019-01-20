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






        public DataGridResult GetWlMerchant(SearchItem item)
        {
            string sql = $@"SELECT * FROM WL_MERCHANT WHERE 1=1 ";
            item.HasKey("MERCHANTID", a => sql += $" and MERCHANTID LIKE '%{a}%'");
            item.HasKey("MERCHANTNAME", a => sql += $" and NAME  LIKE '%{a}%'");
            item.HasKey("SH", a => sql += $" and SH LIKE '%{a}%'");
            item.HasKey("BANK", a => sql += $" and BANK LIKE '%{a}%'");
            item.HasArrayKey("STATUS", a => sql += $" and STATUS in ( { a.SuperJoin(",", b => "'" + b + "'") } ) ");
            sql += " ORDER BY  MERCHANTID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }


        public void WLDeleteMerchant(List<WL_MERCHANTEntity> DeleteData)
        {
            foreach (var mer in DeleteData)
            {
                WL_MERCHANTEntity Data = DbHelper.Select(mer);
                if (Data.STATUS == ((int)普通单据状态.审核).ToString())
                {
                    throw new LogicException("物料供应商(" + Data.NAME + ")已经审核不能删除!");
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


        public string SaveWlMerchant(WL_MERCHANTEntity SaveData)
        {
            var v = GetVerify(SaveData);
            if (SaveData.MERCHANTID.IsEmpty())
            {
                SaveData.MERCHANTID = NewINC("WL_MERCHANT").PadLeft(4, '0');  //暂定6位
                SaveData.STATUS = ((int)普通单据状态.未审核).ToString();
            }
            else
            {
                WL_MERCHANTEntity mer = DbHelper.Select(SaveData);
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
            DbHelper.Save(SaveData);
            return SaveData.MERCHANTID;
        }



        public Tuple<dynamic> GetWlMerchantElement(WL_MERCHANTEntity Data)
        {
            if (Data.MERCHANTID.IsEmpty())
            {
                throw new LogicException("请确认商户编号!");
            }
            string sql = $@"SELECT * FROM WL_MERCHANT WHERE 1=1 ";
            if (!Data.MERCHANTID.IsEmpty())
                sql += (" AND MERCHANTID= " + Data.MERCHANTID);
            DataTable merchant = DbHelper.ExecuteTable(sql);

            merchant.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");


            return new Tuple<dynamic>(merchant.ToOneLine());
        }

        public string ExecWLMerchantData(WL_MERCHANTEntity Data)
        {
            WL_MERCHANTEntity mer = DbHelper.Select(Data);
            if (mer.STATUS == ((int)普通单据状态.审核).ToString())
            {
                throw new LogicException("物料供应商(" + Data.NAME + ")已经审核不能再次审核!");
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




        public DataGridResult GetWlGoods(SearchItem item)
        {
            string sql = $@"SELECT B.*,A.NAME GHSNAME";
            sql += @" FROM WL_MERCHANT A,WL_GOODS B WHERE B.MERCHANTID=B.MERCHANTID AND B.verify IS NOT NULL ";
            item.HasKey("MERCHANTID", a => sql += $" and A.MERCHANTID LIKE '%{a}%'");
            item.HasKey("GOODSDM", a => sql += $" and A.GOODSDM LIKE '%{a}%'");
            sql += " ORDER BY  GOODSDM DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }


        public DataGridResult GetWlGoodsStock(SearchItem item)
        {
            string sql = $@"SELECT B.GOODSDM,B.NAME,B.STATUS,C.TAXINPRICE,B.USEPRICE,C.QTY CANQTY,";
            sql += @" A.NAME GHSNAME,A.MERCHANTID,B.GOODSID";
            sql += @" FROM WL_MERCHANT A,WL_GOODS B,WL_GOODSSTOCK C";
            sql += @" WHERE B.MERCHANTID=B.MERCHANTID AND B.GOODSID=C.GOODSID AND C.QTY>0";
            item.HasKey("MERCHANTID", a => sql += $" and A.MERCHANTID LIKE '%{a}%'");
            item.HasKey("GOODSDM", a => sql += $" and A.GOODSDM LIKE '%{a}%'");
            sql += " ORDER BY C.QTY";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }

        public Tuple<dynamic> GetWlGoodsElement(WL_GOODSEntity Data)
        {
            if (Data.GOODSDM.IsEmpty() && (Data.GOODSID.IsEmpty()))
            {
                throw new LogicException("请确认物料编号!");
            }
            string sql = $@"SELECT * FROM WL_GOODS WHERE 1=1 ";
            if (!Data.GOODSDM.IsEmpty())
                sql += (" AND GOODSDM= " + Data.GOODSDM);
            if (!Data.GOODSID.IsEmpty())
                sql += (" AND GOODSID= " + Data.GOODSID);
            DataTable goods = DbHelper.ExecuteTable(sql);

            goods.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");


            return new Tuple<dynamic>(goods.ToOneLine());
        }


        public void WLDeleteGoods(List<WL_GOODSEntity> DeleteData)
        {
            foreach (var mer in DeleteData)
            {
                WL_GOODSEntity Data = DbHelper.Select(mer);
                if (Data.STATUS == ((int)普通单据状态.审核).ToString())
                {
                    throw new LogicException("物料(" + Data.NAME + ")已经审核不能删除!");
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


        public string SaveWlGoods(WL_GOODSEntity SaveData)
        {
            var v = GetVerify(SaveData);
            if (SaveData.GOODSDM.IsEmpty())
            {
                var id = NewINC("WL_GOODS");
                SaveData.GOODSID = id;
                SaveData.GOODSDM = id.PadLeft(4, '0');
                SaveData.STATUS = ((int)普通单据状态.未审核).ToString();
            }
            else
            {
                WL_GOODSEntity mer = DbHelper.Select(SaveData);
                SaveData.VERIFY = mer.VERIFY;
                SaveData.VERIFY_NAME = mer.VERIFY_NAME;
                SaveData.VERIFY_TIME = mer.VERIFY_TIME;
            }
            SaveData.REPORTER = employee.Id;
            SaveData.REPORTER_NAME = employee.Name;
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            v.Require(a => a.GOODSDM);
            v.Require(a => a.NAME);
            v.IsUnique(a => a.GOODSDM);
            v.IsUnique(a => a.NAME);
            v.Verify();
            DbHelper.Save(SaveData);
            return SaveData.GOODSDM;
        }


        public string ExecWLGoodsData(WL_GOODSEntity Data)
        {
            WL_GOODSEntity mer = DbHelper.Select(Data);
            if (mer.STATUS == ((int)普通单据状态.审核).ToString())
            {
                throw new LogicException("物料(" + Data.NAME + ")已经审核不能再次审核!");
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
            return mer.GOODSDM;
        }


        public DataGridResult GetWlInStock(SearchItem item)
        {
            string sql = $@"SELECT B.*,A.NAME ";
            sql += @" FROM WL_MERCHANT A,WLINSTOCK B WHERE B.MERCHANTID=B.MERCHANTID ";
            item.HasKey("MERCHANTID", a => sql += $" and A.MERCHANTID LIKE '%{a}%'");
            item.HasKey("NAME", a => sql += $" and A.NAME LIKE '%{a}%'");
            item.HasKey("REPORTER_NAME", a => sql += $" and A.REPORTER_NAME LIKE '%{a}%'");
            item.HasKey("VERIFY_NAME", a => sql += $" and A.VERIFY_NAME LIKE '%{a}%'");
            sql += " ORDER BY  B.BILLID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }


        public DataGridResult GetWlOutStock(SearchItem item)
        {
            string sql = $@"SELECT B.*,A.NAME ";
            sql += @" FROM WL_MERCHANT A,WLOUTSTOCK B WHERE B.MERCHANTID=B.MERCHANTID ";
            item.HasKey("MERCHANTID", a => sql += $" and A.MERCHANTID LIKE '%{a}%'");
            item.HasKey("NAME", a => sql += $" and A.NAME LIKE '%{a}%'");
            item.HasKey("REPORTER_NAME", a => sql += $" and A.REPORTER_NAME LIKE '%{a}%'");
            item.HasKey("VERIFY_NAME", a => sql += $" and A.VERIFY_NAME LIKE '%{a}%'");
            sql += " ORDER BY  B.BILLID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }


        public DataGridResult GetWlUses(SearchItem item)
        {
            string sql = $@"SELECT B.*,A.NAME ";
            sql += @" FROM WL_MERCHANT A,WLUSES B WHERE B.MERCHANTID=B.MERCHANTID ";
            item.HasKey("MERCHANTID", a => sql += $" and A.MERCHANTID LIKE '%{a}%'");
            item.HasKey("NAME", a => sql += $" and A.NAME LIKE '%{a}%'");
            item.HasKey("REPORTER_NAME", a => sql += $" and A.REPORTER_NAME LIKE '%{a}%'");
            item.HasKey("VERIFY_NAME", a => sql += $" and A.VERIFY_NAME LIKE '%{a}%'");
            sql += " ORDER BY  B.BILLID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }


        public Tuple<dynamic, DataTable> GetWlInStockElement(WLINSTOCKEntity Data)
        {
            if (Data.BILLID.IsEmpty())
            {
                throw new LogicException("请确认单号!");
            }
            string sql = $@"SELECT B.*,A.NAME";
            sql += " FROM WL_MERCHANT A,WLINSTOCK B WHERE B.MERCHANTID=B.MERCHANTID ";
            sql += (" AND B.BILLID= " + Data.BILLID);
            DataTable InStock = DbHelper.ExecuteTable(sql);
            if (!InStock.IsNotNull())
            {
                throw new LogicException("找不到物料购进单!");
            }

            InStock.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");

            string sqlitem = $@"SELECT A.*,B.GOODSDM,B.NAME,A.TAXINPRICE,B.USEPRICE " +
                             " FROM  WLINSTOCKITETM A,WL_GOODS B  " +
                             " where A.GOODSID = B.GOODSID  ";
            sqlitem += (" and A.BILLID= " + Data.BILLID);
            DataTable InStockItem = DbHelper.ExecuteTable(sqlitem);


            return new Tuple<dynamic, DataTable>(
                InStock.ToOneLine(),
                InStockItem
            );
        }



        public Tuple<dynamic, DataTable> GetWlOutStockElement(WLOUTSTOCKEntity Data)
        {
            if (Data.BILLID.IsEmpty())
            {
                throw new LogicException("请确认单号!");
            }
            string sql = $@"SELECT B.*,A.NAME";
            sql += " FROM WL_MERCHANT A,WLOUTSTOCK B WHERE B.MERCHANTID=B.MERCHANTID ";
            sql += (" AND B.BILLID= " + Data.BILLID);
            DataTable OutStock = DbHelper.ExecuteTable(sql);
            if (!OutStock.IsNotNull())
            {
                throw new LogicException("找不到物料购进单!");
            }

            OutStock.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");

            string sqlitem = $@"SELECT A.*,B.GOODSDM,B.NAME,B.USEPRICE " +
                             " FROM  WLOUTSTOCKITETM A,WL_GOODS B  " +
                             " where A.GOODSID = B.GOODSID  ";
            sqlitem += (" and A.BILLID= " + Data.BILLID);
            DataTable OutStockItem = DbHelper.ExecuteTable(sqlitem);


            return new Tuple<dynamic, DataTable>(
                OutStock.ToOneLine(),
                OutStockItem
            );
        }


        public void WLDeleteInStock(List<WLINSTOCKEntity> DeleteData)
        {
            foreach (var mer in DeleteData)
            {
                WLINSTOCKEntity Data = DbHelper.Select(mer);
                if (Data.STATUS == ((int)普通单据状态.审核).ToString())
                {
                    throw new LogicException("物料购进单单号(" + Data.BILLID + ")已经审核不能删除!");
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

        public void WLDeleteOutStock(List<WLOUTSTOCKEntity> DeleteData)
        {
            foreach (var mer in DeleteData)
            {
                WLOUTSTOCKEntity Data = DbHelper.Select(mer);
                if (Data.STATUS == ((int)普通单据状态.审核).ToString())
                {
                    throw new LogicException("物料购进冲红单单号(" + Data.BILLID + ")已经审核不能删除!");
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


        public string SaveWlInStock(WLINSTOCKEntity SaveData)
        {
            var v = GetVerify(SaveData);
            if (SaveData.BILLID.IsEmpty())
            {
                SaveData.BILLID = NewINC("WLINSTOCK");
                SaveData.STATUS = ((int)普通单据状态.未审核).ToString();
            }
            else
            {
                WLINSTOCKEntity mer = DbHelper.Select(SaveData);
                SaveData.VERIFY = mer.VERIFY;
                SaveData.VERIFY_NAME = mer.VERIFY_NAME;
                SaveData.VERIFY_TIME = mer.VERIFY_TIME;
            }
            SaveData.REPORTER = employee.Id;
            SaveData.REPORTER_NAME = employee.Name;
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            v.Require(a => a.MERCHANTID);
            v.Verify();

            using (var Tran = DbHelper.BeginTransaction())
            {
                DbHelper.Save(SaveData);

                Tran.Commit();
            }
            return SaveData.BILLID;
        }



        public string SaveWlOutStock(WLOUTSTOCKEntity SaveData)
        {
            var v = GetVerify(SaveData);
            if (SaveData.BILLID.IsEmpty())
            {
                SaveData.BILLID = NewINC("WLOUTSTOCK");
                SaveData.STATUS = ((int)普通单据状态.未审核).ToString();
            }
            else
            {
                WLOUTSTOCKEntity mer = DbHelper.Select(SaveData);
                SaveData.VERIFY = mer.VERIFY;
                SaveData.VERIFY_NAME = mer.VERIFY_NAME;
                SaveData.VERIFY_TIME = mer.VERIFY_TIME;
            }
            SaveData.REPORTER = employee.Id;
            SaveData.REPORTER_NAME = employee.Name;
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            v.Require(a => a.MERCHANTID);
            v.Verify();

            using (var Tran = DbHelper.BeginTransaction())
            {
                DbHelper.Save(SaveData);

                Tran.Commit();
            }
            return SaveData.BILLID;
        }

        public string ExecWlInStock(WLINSTOCKEntity Data)
        {
            WLINSTOCKEntity mer = DbHelper.Select(Data);
            if (mer.STATUS == ((int)普通单据状态.审核).ToString())
            {
                throw new LogicException("物料购进单(" + Data.BILLID + ")已经审核不能再次审核!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                mer.VERIFY = employee.Id;
                mer.VERIFY_NAME = employee.Name;
                mer.VERIFY_TIME = DateTime.Now.ToString();
                mer.STATUS = ((int)普通单据状态.审核).ToString();
                DbHelper.Save(mer);

                WLINSTOCKITETMEntity item = new WLINSTOCKITETMEntity();
                item.BILLID = Data.BILLID;
                List<WLINSTOCKITETMEntity> itemStock = DbHelper.SelectList(item);
                foreach (var items in itemStock)
                {
                    //更新库存表 先求总金额,再求总数量,计算新的含税采购价
                    WL_GOODSSTOCKEntity goodsstock = new WL_GOODSSTOCKEntity();

                    //WL_GOODSEntity wlgoods = new WL_GOODSEntity();

                    goodsstock.GOODSID = items.GOODSID;


                    // wlgoods.GOODSID = items.GOODSID;

                    // WL_GOODSEntity goods = DbHelper.Select(wlgoods);

                    WL_GOODSSTOCKEntity goodsstockdata = DbHelper.Select(goodsstock);
                    if (goodsstockdata != null)
                    {
                        goodsstock.QTY = (goodsstockdata.QTY.ToDouble()
                            + items.QUANTITY.ToDouble()).ToString();

                        goodsstock.TAXAMOUNT = (goodsstockdata.TAXAMOUNT.ToDouble()
                            + Math.Round(items.TAXINPRICE.ToDouble() * items.QUANTITY.ToDouble(),
                            2, MidpointRounding.AwayFromZero)).ToString();

                        goodsstock.TAXINPRICE =
                            (Math.Round(goodsstock.TAXAMOUNT.ToDouble() / goodsstock.QTY.ToDouble(),
                            2, MidpointRounding.AwayFromZero)).ToString();
                    }
                    else
                    {
                        goodsstock.QTY = items.QUANTITY;
                        goodsstock.TAXINPRICE = items.TAXINPRICE;
                        goodsstock.TAXAMOUNT =
                            Math.Round(items.TAXINPRICE.ToDouble() * items.QUANTITY.ToDouble(),
                            2, MidpointRounding.AwayFromZero).ToString();

                    }
                    DbHelper.Save(goodsstock);
                }

                Tran.Commit();
            }
            return mer.BILLID;
        }

        public string ExecWlOutStock(WLOUTSTOCKEntity Data)
        {
            WLOUTSTOCKEntity mer = DbHelper.Select(Data);
            if (mer.STATUS == ((int)普通单据状态.审核).ToString())
            {
                throw new LogicException("物料购进单(" + Data.BILLID + ")已经审核不能再次审核!");
            }



            using (var Tran = DbHelper.BeginTransaction())
            {
                mer.VERIFY = employee.Id;
                mer.VERIFY_NAME = employee.Name;
                mer.VERIFY_TIME = DateTime.Now.ToString();
                mer.STATUS = ((int)普通单据状态.审核).ToString();
                DbHelper.Save(mer);

                WLOUTSTOCKITETMEntity outitem = new WLOUTSTOCKITETMEntity();
                outitem.BILLID = Data.BILLID;

                List<WLOUTSTOCKITETMEntity> itemStock = DbHelper.SelectList(outitem);
                foreach (var items in itemStock)
                {
                    //减少库存数量,并且重写计算库存单价
                    WL_GOODSSTOCKEntity goodsstock = new WL_GOODSSTOCKEntity();

                    goodsstock.GOODSID = items.GOODSID;

                    WL_GOODSSTOCKEntity goodsstockdata = DbHelper.Select(goodsstock);

                    if (goodsstockdata.QTY.ToDouble() < items.QUANTITY.ToDouble())
                    {
                        throw new LogicException("有冲红数量大于可冲红数量!");
                    }
                    goodsstockdata.QTY = (goodsstockdata.QTY.ToDouble() - items.QUANTITY.ToDouble()).ToString();
                    goodsstockdata.TAXAMOUNT = (Math.Round(goodsstockdata.TAXAMOUNT.ToDouble()
                        - (items.QUANTITY.ToDouble() * items.TAXINPRICE.ToDouble()),
                        2, MidpointRounding.AwayFromZero)).ToString();
                    goodsstockdata.TAXINPRICE = (Math.Round(
                        goodsstockdata.TAXAMOUNT.ToDouble() / goodsstockdata.QTY.ToDouble(),
                        2, MidpointRounding.AwayFromZero)).ToString();
                    DbHelper.Save(goodsstockdata);
                }

                Tran.Commit();
            }
            return mer.BILLID;
        }



        public Tuple<dynamic, DataTable> GetWlUsersElement(WLUSESEntity Data)
        {
            if (Data.BILLID.IsEmpty())
            {
                throw new LogicException("请确认单号!");
            }
            string sql = $@"SELECT B.*,A.NAME";
            sql += " FROM WL_MERCHANT A,WLUSES B WHERE B.MERCHANTID=B.MERCHANTID ";
            sql += (" AND B.BILLID= " + Data.BILLID);
            DataTable user = DbHelper.ExecuteTable(sql);
            if (!user.IsNotNull())
            {
                throw new LogicException("找不到物料领用单!");
            }

            user.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");

            string sqlitem = $@"SELECT A.*,B.GOODSDM,B.NAME,B.USEPRICE " +
                             " FROM  WLUSESITETM A,WL_GOODS B  " +
                             " where A.GOODSID = B.GOODSID  ";
            sqlitem += (" and A.BILLID= " + Data.BILLID);
            DataTable userItem = DbHelper.ExecuteTable(sqlitem);


            return new Tuple<dynamic, DataTable>(
                user.ToOneLine(),
                userItem
            );
        }


        public void WLDeleteWlUser(List<WLUSESEntity> DeleteData)
        {
            foreach (var mer in DeleteData)
            {
                WLUSESEntity Data = DbHelper.Select(mer);
                if (Data.STATUS == ((int)普通单据状态.审核).ToString())
                {
                    throw new LogicException("物料购领用单单号(" + Data.BILLID + ")已经审核不能删除!");
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


        public string SaveWlUsers(WLUSESEntity SaveData)
        {
            var v = GetVerify(SaveData);
            if (SaveData.BILLID.IsEmpty())
            {
                SaveData.BILLID = NewINC("WLUSES");
                SaveData.STATUS = ((int)普通单据状态.未审核).ToString();
            }
            else
            {
                WLUSESEntity mer = DbHelper.Select(SaveData);
                SaveData.VERIFY = mer.VERIFY;
                SaveData.VERIFY_NAME = mer.VERIFY_NAME;
                SaveData.VERIFY_TIME = mer.VERIFY_TIME;
            }
            SaveData.REPORTER = employee.Id;
            SaveData.REPORTER_NAME = employee.Name;
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            v.Require(a => a.MERCHANTID);
            v.Verify();

            using (var Tran = DbHelper.BeginTransaction())
            {
                DbHelper.Save(SaveData);

                Tran.Commit();
            }
            return SaveData.BILLID;
        }


        public string ExecWlUses(WLUSESEntity Data)
        {
            WLUSESEntity mer = DbHelper.Select(Data);
            if (mer.STATUS == ((int)普通单据状态.审核).ToString())
            {
                throw new LogicException("物料领用单(" + Data.BILLID + ")已经审核不能再次审核!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                mer.VERIFY = employee.Id;
                mer.VERIFY_NAME = employee.Name;
                mer.VERIFY_TIME = DateTime.Now.ToString();
                mer.STATUS = ((int)普通单据状态.审核).ToString();
                DbHelper.Save(mer);

                WLUSESITETMEntity outitem = new WLUSESITETMEntity();
                outitem.BILLID = Data.BILLID;

                List<WLUSESITETMEntity> itemStock = DbHelper.SelectList(outitem);

                foreach (var items in itemStock)
                {
                    //减少库存数量,并且重写计算库存单价
                    WL_GOODSSTOCKEntity goodsstock = new WL_GOODSSTOCKEntity();

                    goodsstock.GOODSID = items.GOODSID;

                    WL_GOODSSTOCKEntity goodsstockdata = DbHelper.Select(goodsstock);

                    if (goodsstockdata.QTY.ToDouble() < items.QUANTITY.ToDouble())
                    {
                        throw new LogicException("有领用数量大于可冲红数量!");
                    }
                    goodsstockdata.QTY = (goodsstockdata.QTY.ToDouble() - items.QUANTITY.ToDouble()).ToString();
                    goodsstockdata.TAXAMOUNT = (Math.Round(goodsstockdata.TAXAMOUNT.ToDouble()
                        - (items.QUANTITY.ToDouble() * goodsstockdata.TAXINPRICE.ToDouble()),
                        2, MidpointRounding.AwayFromZero)).ToString();
                    goodsstockdata.TAXINPRICE = (Math.Round(
                        goodsstockdata.TAXAMOUNT.ToDouble() / goodsstockdata.QTY.ToDouble(),
                        2, MidpointRounding.AwayFromZero)).ToString();
                    DbHelper.Save(goodsstockdata);
                }

                Tran.Commit();
            }
            return mer.BILLID;
        }


        public Tuple<dynamic, DataTable> GetWLUSESElement(WLUSESEntity Data)
        {
            if (Data.BILLID.IsEmpty())
            {
                throw new LogicException("请确认单号!");
            }
            string sql = $@"SELECT B.*,A.NAME";
            sql += " FROM WL_MERCHANT A,WLUSES B WHERE B.MERCHANTID=B.MERCHANTID ";
            sql += (" AND B.BILLID= " + Data.BILLID);
            DataTable main = DbHelper.ExecuteTable(sql);
            if (!main.IsNotNull())
            {
                throw new LogicException("找不到物料领用单!");
            }

            main.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");

            string sqlitem = $@"SELECT A.*,B.GOODSDM,B.NAME,B.USEPRICE " +
                             " FROM  WLUSESITETM A,WL_GOODS B  " +
                             " where A.GOODSID = B.GOODSID  ";
            sqlitem += (" and A.BILLID= " + Data.BILLID);
            DataTable item = DbHelper.ExecuteTable(sqlitem);


            return new Tuple<dynamic, DataTable>(
                main.ToOneLine(),
                item
            );
        }


        public Tuple<dynamic, DataTable> GetWlCheckElement(WLCHECKEntity Data)
        {
            if (Data.BILLID.IsEmpty())
            {
                throw new LogicException("请确认单号!");
            }
            string sql = $@"SELECT B.*,A.NAME";
            sql += " FROM WL_MERCHANT A,WLCHECK B WHERE B.MERCHANTID=B.MERCHANTID ";
            sql += (" AND B.BILLID= " + Data.BILLID);
            DataTable main = DbHelper.ExecuteTable(sql);
            if (!main.IsNotNull())
            {
                throw new LogicException("找不到损溢单!");
            }

            main.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");

            string sqlitem = $@"SELECT A.*,B.GOODSDM,B.NAME,B.USEPRICE " +
                             " FROM  WLCHECKITEM A,WL_GOODS B  " +
                             " where A.GOODSID = B.GOODSID  ";
            sqlitem += (" and A.BILLID= " + Data.BILLID);
            DataTable item = DbHelper.ExecuteTable(sqlitem);


            return new Tuple<dynamic, DataTable>(
                main.ToOneLine(),
                item
            );
        }



        public void WLDeleteWLCheck(List<WLCHECKEntity> DeleteData)
        {
            foreach (var mer in DeleteData)
            {
                WLCHECKEntity Data = DbHelper.Select(mer);
                if (Data.STATUS == ((int)普通单据状态.审核).ToString())
                {
                    throw new LogicException("物料损溢单单号(" + Data.BILLID + ")已经审核不能删除!");
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


        public string SaveWLCheck(WLCHECKEntity SaveData)
        {
            var v = GetVerify(SaveData);
            if (SaveData.BILLID.IsEmpty())
            {
                SaveData.BILLID = NewINC("WLCHECK");
                SaveData.STATUS = ((int)普通单据状态.未审核).ToString();
            }
            else
            {
                WLCHECKEntity mer = DbHelper.Select(SaveData);
                SaveData.VERIFY = mer.VERIFY;
                SaveData.VERIFY_NAME = mer.VERIFY_NAME;
                SaveData.VERIFY_TIME = mer.VERIFY_TIME;
            }
            SaveData.REPORTER = employee.Id;
            SaveData.REPORTER_NAME = employee.Name;
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            v.Require(a => a.MERCHANTID);
            v.Verify();

            using (var Tran = DbHelper.BeginTransaction())
            {
                DbHelper.Save(SaveData);

                Tran.Commit();
            }
            return SaveData.BILLID;
        }

        public string ExecWLCheck(WLCHECKEntity Data)
        {
            WLCHECKEntity mer = DbHelper.Select(Data);
            if (mer.STATUS == ((int)普通单据状态.审核).ToString())
            {
                throw new LogicException("物料损溢单(" + Data.BILLID + ")已经审核不能再次审核!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                mer.VERIFY = employee.Id;
                mer.VERIFY_NAME = employee.Name;
                mer.VERIFY_TIME = DateTime.Now.ToString();
                mer.STATUS = ((int)普通单据状态.审核).ToString();
                DbHelper.Save(mer);

                WLCHECKITEMEntity outitem = new WLCHECKITEMEntity();
                outitem.BILLID = Data.BILLID;

                List<WLCHECKITEMEntity> itemStock = DbHelper.SelectList(outitem);

                foreach (var items in itemStock)
                {
                    WL_GOODSSTOCKEntity goodsstock = new WL_GOODSSTOCKEntity();

                    goodsstock.GOODSID = items.GOODSID;

                    WL_GOODSSTOCKEntity goodsstockdata = DbHelper.Select(goodsstock);
                    if (goodsstockdata != null)
                    {
                        if (goodsstockdata.QTY.ToDouble() < items.QUANTITY.ToDouble())
                        {
                            throw new LogicException("报损数量不能大于账面数量!");
                        }
                        goodsstockdata.QTY = (goodsstockdata.QTY.ToDouble() - items.QUANTITY.ToDouble()).ToString();
                        goodsstockdata.TAXAMOUNT = (Math.Round(goodsstockdata.TAXAMOUNT.ToDouble()
                            - (items.QUANTITY.ToDouble() * goodsstockdata.TAXINPRICE.ToDouble()),
                            2, MidpointRounding.AwayFromZero)).ToString();
                        goodsstockdata.TAXINPRICE = (Math.Round(
                            goodsstockdata.TAXAMOUNT.ToDouble() / goodsstockdata.QTY.ToDouble(),
                            2, MidpointRounding.AwayFromZero)).ToString();
                        DbHelper.Save(goodsstockdata);
                    }
                }

                Tran.Commit();
            }
            return mer.BILLID;
        }


        public DataGridResult GetWlCheck(SearchItem item)
        {
            string sql = $@"SELECT B.*,A.NAME ";
            sql += @" FROM WL_MERCHANT A,WLCHECK B WHERE B.MERCHANTID=B.MERCHANTID ";
            item.HasKey("MERCHANTID", a => sql += $" and A.MERCHANTID LIKE '%{a}%'");
            item.HasKey("NAME", a => sql += $" and A.NAME LIKE '%{a}%'");
            item.HasKey("REPORTER_NAME", a => sql += $" and A.REPORTER_NAME LIKE '%{a}%'");
            item.HasKey("VERIFY_NAME", a => sql += $" and A.VERIFY_NAME LIKE '%{a}%'");
            sql += " ORDER BY  B.BILLID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }

        public DataGridResult WLSrchStock(SearchItem item)
        {
            string sql = $@"SELECT A.MERCHANTID,A.NAME,C.GOODSDM,C.NAME GOODSNAME, ";
            sql += @" B.QTY,B.TAXAMOUNT";
            sql += @" FROM WL_MERCHANT A,WL_GOODSSTOCK B,WL_GOODS C";
            sql += @" WHERE A.MERCHANTID = C.MERCHANTID AND B.GOODSID=C.GOODSID ";
            item.HasKey("MERCHANTID", a => sql += $" and A.MERCHANTID LIKE '{a}%'");
            item.HasKey("NAME", a => sql += $" and A.NAME LIKE '{a}%'");
            item.HasKey("GOODSDM", a => sql += $" and C.GOODSDM LIKE '{a}%'");
            item.HasKey("GOODSNAME", a => sql += $" and C.NAME LIKE '{a}%'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }



        public DataGridResult GetWLSETTLE(SearchItem item)
        {
            string sql = $@"SELECT B.*,A.NAME ";
            sql += @" FROM WL_MERCHANT A,WLSETTLE B WHERE B.MERCHANTID=B.MERCHANTID ";
            item.HasKey("MERCHANTID", a => sql += $" and A.MERCHANTID LIKE '%{a}%'");
            item.HasKey("NAME", a => sql += $" and A.NAME LIKE '%{a}%'");
            item.HasKey("REPORTER_NAME", a => sql += $" and A.REPORTER_NAME LIKE '%{a}%'");
            item.HasKey("VERIFY_NAME", a => sql += $" and A.VERIFY_NAME LIKE '%{a}%'");
            sql += " ORDER BY  B.BILLID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }



        public DataGridResult GetWlGoodsDjxx(SearchItem item)
        {
            string sql = $@"SELECT B.GOODSID,B.GOODSDM,B.NAME,B.STATUS,C.TAXINPRICE,C.DH,C.LX,C.QUANTITY,";
            sql += @" A.NAME GHSNAME,A.MERCHANTID";
            sql += @" FROM WL_MERCHANT A,WL_GOODS B,WLSTOCK_DJXX C";
            sql += @" WHERE B.MERCHANTID=B.MERCHANTID AND B.GOODSID=C.GOODSID ";
            item.HasKey("MERCHANTID", a => sql += $" and A.MERCHANTID LIKE '%{a}%'");
            item.HasKey("GOODSDM", a => sql += $" and A.GOODSDM LIKE '%{a}%'");
            sql += @" and not exists(SELECT 1 FROM WLSETTLEITEM M where M.DH=C.DH and M.LX=C.LX and M.GOODSID=C.GOODSID)";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            dt.NewEnumColumns<业务类型单据>("LX", "LXMC");
            return new DataGridResult(dt, count);
        }

        public Tuple<dynamic, DataTable> GetWLSETTLEElement(WLSETTLEEntity Data)
        {
            if (Data.BILLID.IsEmpty())
            {
                throw new LogicException("请确认单号!");
            }
            string sql = $@"SELECT B.*,A.NAME";
            sql += " FROM WL_MERCHANT A,WLSETTLE B WHERE B.MERCHANTID=B.MERCHANTID ";
            sql += (" AND B.BILLID= " + Data.BILLID);
            DataTable mian = DbHelper.ExecuteTable(sql);
            if (!mian.IsNotNull())
            {
                throw new LogicException("找不到物料结算单!");
            }

            mian.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");

            string sqlitem = $@"SELECT A.*,B.GOODSDM,B.NAME " +
                             " FROM  WLSETTLEITEM A,WL_GOODS B  " +
                             " where A.GOODSID = B.GOODSID  ";
            sqlitem += (" and A.BILLID= " + Data.BILLID);
            DataTable item = DbHelper.ExecuteTable(sqlitem);
            item.NewEnumColumns<业务类型单据>("LX", "LXMC");
            return new Tuple<dynamic, DataTable>(
                mian.ToOneLine(),
                item
            );
        }


        public void WLDeleteWLSETTLE(List<WLSETTLEEntity> DeleteData)
        {
            foreach (var mer in DeleteData)
            {
                WLSETTLEEntity Data = DbHelper.Select(mer);
                if (Data.STATUS == ((int)普通单据状态.审核).ToString())
                {
                    throw new LogicException("物料结算单单号(" + Data.BILLID + ")已经审核不能删除!");
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


        public string SaveWLSETTLE(WLSETTLEEntity SaveData)
        {
            var v = GetVerify(SaveData);
            if (SaveData.BILLID.IsEmpty())
            {
                SaveData.BILLID = NewINC("WLSETTLE");
                SaveData.STATUS = ((int)普通单据状态.未审核).ToString();
            }
            else
            {
                WLSETTLEEntity mer = DbHelper.Select(SaveData);
                SaveData.VERIFY = mer.VERIFY;
                SaveData.VERIFY_NAME = mer.VERIFY_NAME;
                SaveData.VERIFY_TIME = mer.VERIFY_TIME;
            }
            SaveData.REPORTER = employee.Id;
            SaveData.REPORTER_NAME = employee.Name;
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            v.Require(a => a.MERCHANTID);
            v.Verify();

            using (var Tran = DbHelper.BeginTransaction())
            {
                DbHelper.Save(SaveData);

                Tran.Commit();
            }
            return SaveData.BILLID;
        }


        public string ExecWLSETTLE(WLSETTLEEntity Data)
        {
            WLSETTLEEntity mer = DbHelper.Select(Data);
            if (mer.STATUS == ((int)普通单据状态.审核).ToString())
            {
                throw new LogicException("物料结算单(" + Data.BILLID + ")已经审核不能再次审核!");
            }
            mer.VERIFY = employee.Id;
            mer.VERIFY_NAME = employee.Name;
            mer.VERIFY_TIME = DateTime.Now.ToString();
            mer.STATUS = ((int)普通单据状态.审核).ToString();
            DbHelper.Save(mer);
            return mer.BILLID;
        }

        public Tuple<dynamic, DataTable> GetMarchinArearDetail(MARCHINAREAREntity Data)
        {
            if (Data.BILLID.IsEmpty())
            {
                throw new LogicException("请确认记录编号!");
            }
            string sql = $@"select R.*,M.MERCHANTID,M.NAME from MARCHINAREAR R,CONTRACT T,MERCHANT M where R.CONTRACTID =T.CONTRACTID and T.MERCHANTID=M.MERCHANTID";
            if (!Data.BILLID.IsEmpty())
                sql += (" and BILLID= " + Data.BILLID);
            DataTable dt = DbHelper.ExecuteTable(sql);

            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");

            string sqlitem = $@"SELECT M.*,P.CODE,P.NAME,P.AREA_BUILD " +
                " FROM MARCHINAREARITEM M,SHOP P " +
                " where  M.SHOPID = P.SHOPID";
            if (!Data.BILLID.IsEmpty())
                sqlitem += (" and BILLID= " + Data.BILLID);
            DataTable dtitem = DbHelper.ExecuteTable(sqlitem);

            return new Tuple<dynamic, DataTable>(dt.ToOneLine(), dtitem);
        }
        public string SaveMarchInArear(MARCHINAREAREntity SaveData)
        {
            var v = GetVerify(SaveData);

            SaveData.MARCHINDATE.ToDateTime();
            if (SaveData.BILLID.IsEmpty())
            {
                SaveData.BILLID = NewINC("MARCHINAREAR");
            }

            SaveData.REPORTER = employee.Id;
            SaveData.REPORTER_NAME = employee.Name;
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            SaveData.STATUS = ((int)普通单据状态.未审核).ToString();

            v.Require(a => a.BILLID);
            v.Require(a => a.BRANCHID);
            v.Require(a => a.CONTRACTID);
            v.Require(a => a.MARCHINDATE);            

            using (var tran = DbHelper.BeginTransaction())
            {
                SaveData.MARCHINAREARITEM.ForEach(sdb =>
                {                    
                    GetVerify(sdb).Require(a => a.SHOPID);
                });
                v.Verify();
                DbHelper.Save(SaveData);

                tran.Commit();
            }
            return SaveData.BILLID;
        }

        public void DeleteMarchInArear(List<MARCHINAREAREntity> DeleteData)
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
        }

        public string ExecMarchInArearData(MARCHINAREAREntity Data)
        {
            MARCHINAREAREntity brand = DbHelper.Select(Data);
            if (brand.STATUS == ((int)普通单据状态.审核).ToString())
            {
                throw new LogicException("单据(" + Data.BILLID + ")已经审核不能再次审核!");
            }

            using (var Tran = DbHelper.BeginTransaction())
            {
                brand.VERIFY = employee.Id;
                brand.VERIFY_NAME = employee.Name;
                brand.VERIFY_TIME = DateTime.Now.ToString();
                brand.STATUS = ((int)普通单据状态.审核).ToString();
                DbHelper.Save(brand);
                Notes(nameof(MARCHINAREAREntity), brand.BILLID, $"已审核");
                Tran.Commit();
            }
            return brand.BILLID;
        }

        public object GetMarchInArearElement(MARCHINAREAREntity Data)
        {
            string sql = $@"select R.*,M.MERCHANTID,M.NAME from MARCHINAREAR R,CONTRACT T,MERCHANT M where R.CONTRACTID =T.CONTRACTID and T.MERCHANTID=M.MERCHANTID ";
            if (!Data.BILLID.IsEmpty())
                sql += (" and BILLID= " + Data.BILLID);
            DataTable dt = DbHelper.ExecuteTable(sql);

            string sqlitem = $@"SELECT M.*,P.CODE,P.NAME,P.AREA_BUILD " +
                " FROM MARCHINAREARITEM M,SHOP P " +
                " where M.SHOPID = P.SHOPID";
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

        public DataGridResult GetMarchinArear(SearchItem item)
        {
            string sql = $@"select * from MARCHINAREAR where 1=1 ";
            item.HasKey("BILLID", a => sql += $" and BILLID = '{a}'");
            item.HasKey("REPORTER", a => sql += $" and REPORTER = '{a}'");
            item.HasKey("VERIFY", a => sql += $" and VERIFY = '{a}'");
            item.HasArrayKey("STATUS", a => sql += $" and STATUS in ( { a.SuperJoin(",", b => "'" + b + "'") } ) ");
            item.HasKey("MARCHINDATE_START", a => sql += $" and MARCHINDATE>= to_date('{a.ToDateTime().ToLocalTime()}','YYYY-MM-DD  HH24:MI:SS')");
            item.HasKey("MARCHINDATE_END", a => sql += $" and MARCHINDATE<= to_date('{a.ToDateTime().ToLocalTime()}','YYYY-MM-DD  HH24:MI:SS')");
            sql += " order by BILLID desc";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }

        public object GetContract(CONTRACTEntity Data)
        {
            string sql = $@"select T.MERCHANTID,S.NAME SHMC,T.BRANCHID,T.STYLE,T.JXSL*100 JXSL,T.XXSL*100 XXSL from CONTRACT T,MERCHANT S where T.MERCHANTID=S.MERCHANTID ";
            if (!Data.CONTRACTID.IsEmpty())
                sql += (" and T.CONTRACTID= " + Data.CONTRACTID);
            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.NewEnumColumns<核算方式>("STYLE", "STYLEMC");

            string sql_shop = $@"SELECT P.SHOPID,S.CODE,S.NAME,S.AREA_BUILD"
                + " FROM CONTRACT_SHOP P,SHOP S "
                + " WHERE  P.SHOPID = S.SHOPID ";
            if (!Data.CONTRACTID.IsEmpty())
                sql_shop += (" and P.CONTRACTID= " + Data.CONTRACTID);
            sql_shop += " order by S.CODE";
            DataTable shop = DbHelper.ExecuteTable(sql_shop);
            var result = new
            {
                contract = dt,
                shop = shop
            };
            return result;
        }

        public Tuple<dynamic, DataTable> GetOpenBusinessDetail(OPENBUSINESSEntity Data)
        {
            if (Data.BILLID.IsEmpty())
            {
                throw new LogicException("请确认记录编号!");
            }
            string sql = $@"select R.*,M.MERCHANTID,M.NAME from OPENBUSINESS R,CONTRACT T,MERCHANT M where R.CONTRACTID =T.CONTRACTID and T.MERCHANTID=M.MERCHANTID  ";
            if (!Data.BILLID.IsEmpty())
                sql += (" and BILLID= " + Data.BILLID);
            DataTable dt = DbHelper.ExecuteTable(sql);

            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");

            string sqlitem = $@"SELECT M.*,P.CODE,P.NAME,P.AREA_BUILD " +
                " FROM OPENBUSINESSITEM M,SHOP P " +
                " where  M.SHOPID = P.SHOPID";
            if (!Data.BILLID.IsEmpty())
                sqlitem += (" and BILLID= " + Data.BILLID);
            DataTable dtitem = DbHelper.ExecuteTable(sqlitem);

            return new Tuple<dynamic, DataTable>(dt.ToOneLine(), dtitem);
        }
        public string SaveOpenBusiness(OPENBUSINESSEntity SaveData)
        {
            var v = GetVerify(SaveData);

            SaveData.OPENDATE.ToDateTime();
            if (SaveData.BILLID.IsEmpty())
            {
                SaveData.BILLID = NewINC("OPENBUSINESS");
            }

            SaveData.REPORTER = employee.Id;
            SaveData.REPORTER_NAME = employee.Name;
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            SaveData.STATUS = ((int)普通单据状态.未审核).ToString();

            v.Require(a => a.BILLID);
            v.Require(a => a.BRANCHID);
            v.Require(a => a.CONTRACTID);
            v.Require(a => a.OPENDATE);

            using (var tran = DbHelper.BeginTransaction())
            {
                SaveData.OPENBUSINESSITEM.ForEach(sdb =>
                {
                    GetVerify(sdb).Require(a => a.SHOPID);
                });
                v.Verify();
                DbHelper.Save(SaveData);
                tran.Commit();
            }
            return SaveData.BILLID;
        }

        public void DeleteOpenBusiness(List<OPENBUSINESSEntity> DeleteData)
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
        }

        public string ExecOpenBusinessData(OPENBUSINESSEntity Data)
        {
            OPENBUSINESSEntity brand = DbHelper.Select(Data);
            if (brand.STATUS == ((int)普通单据状态.审核).ToString())
            {
                throw new LogicException("单据(" + Data.BILLID + ")已经审核不能再次审核!");
            }

            using (var Tran = DbHelper.BeginTransaction())
            {
                brand.VERIFY = employee.Id;
                brand.VERIFY_NAME = employee.Name;
                brand.VERIFY_TIME = DateTime.Now.ToString();
                brand.STATUS = ((int)普通单据状态.审核).ToString();
                DbHelper.Save(brand);
                Notes(nameof(OPENBUSINESSEntity), brand.BILLID, $"已审核");
                Tran.Commit();
            }
            return brand.BILLID;
        }

        public object GetOpenBusinessElement(OPENBUSINESSEntity Data)
        {
            string sql = $@"select R.*,M.MERCHANTID,M.NAME SHMC from OPENBUSINESS R,CONTRACT T,MERCHANT M where R.CONTRACTID =T.CONTRACTID and T.MERCHANTID=M.MERCHANTID ";
            if (!Data.BILLID.IsEmpty())
                sql += (" and BILLID= " + Data.BILLID);
            DataTable dt = DbHelper.ExecuteTable(sql);

            string sqlitem = $@"SELECT M.*,P.CODE,P.NAME,P.AREA_BUILD " +
                " FROM OPENBUSINESSITEM M,SHOP P " +
                " where M.SHOPID = P.SHOPID";
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

        public DataGridResult GetOpenBusiness(SearchItem item)
        {
            string sql = $@"select * from OPENBUSINESS where 1=1 ";
            item.HasKey("BILLID", a => sql += $" and BILLID = '{a}'");
            item.HasKey("REPORTER", a => sql += $" and REPORTER = '{a}'");
            item.HasKey("VERIFY", a => sql += $" and VERIFY = '{a}'");
            item.HasArrayKey("STATUS", a => sql += $" and STATUS in ( { a.SuperJoin(",", b => "'" + b + "'") } ) ");
            item.HasKey("OPENDATE_START", a => sql += $" and OPENDATE>= to_date('{a.ToDateTime().ToLocalTime()}','YYYY-MM-DD  HH24:MI:SS')");
            item.HasKey("OPENDATE_END", a => sql += $" and OPENDATE<= to_date('{a.ToDateTime().ToLocalTime()}','YYYY-MM-DD  HH24:MI:SS')");
            sql += " order by BILLID desc";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }
    }
}
