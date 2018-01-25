﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using z.ERP.Entities;
using z.ERP.Model.Vue;
using z.Extensions;
using z.Extensiont;
using z.MVC5.Results;
using z.WebPage;

namespace z.ERP.Services
{
    public class XtglService : ServiceBase
    {
        internal XtglService()
        {
        }

        public DataGridResult GetBrandData(SearchItem item)
        {
            string sql = $@"SELECT B.*,C.CATEGORYCODE,C.CATEGORYNAME FROM BRAND B,CATEGORY C where B.CATEGORYID=C.CATEGORYID ";
            item.HasKey("ID", a => sql += $" and B.ID = '{a}'");
            item.HasKey("NAME", a => sql += $" and B.NAME LIKE '%{a}%'");
            item.HasKey("CATEGORYCODE", a => sql += $" and C.CATEGORYCODE LIKE '%{a}%'");
            item.HasKey("ADRESS", a => sql += $" and B.ADRESS LIKE '%{a}%'");
            item.HasKey("CONTACTPERSON", a => sql += $" and B.CONTACTPERSON = '{a}'");
            item.HasKey("PHONENUM", a => sql += $" and B.PHONENUM = '{a}'");
            item.HasKey("PIZ", a => sql += $" and B.PIZ = '{a}'");
            item.HasKey("QQ", a => sql += $" and B.QQ = '{a}'");
            
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public DataGridResult GetPay(SearchItem item)
        {
            string sql = $@"SELECT PAYID,NAME FROM PAY WHERE 1=1 ";
            item.HasKey("PAYID", a => sql += $" and PAYID = '{a}'");
            item.HasKey("NAME", a => sql += $" and NAME = '{a}'");
            sql += " ORDER BY  PAYID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public DataGridResult GetPayElement(SearchItem item)
        {
            string sql = $@"SELECT * FROM PAY WHERE 1=1 ";
            item.HasKey("PAYID", a => sql += $" and PAYID = '{a}'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }


        public DataGridResult GetFeeSubject(SearchItem item)
        {
            string sql = $@"SELECT TRIMID,NAME FROM FEESUBJECT  WHERE 1=1";
            item.HasKey("TRIMID", a => sql += $" and TRIMID = '{a}'");
            item.HasKey("NAME", a => sql += $" and NAME = '{a}'");
            sql += " ORDER BY  TRIMID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public DataGridResult GetFeeSubjectElement(SearchItem item)
        {
            string sql = $@"SELECT TRIMID,NAME FROM FEESUBJECT WHERE 1=1 ";
            item.HasKey("TRIMID", a => sql += $" and TRIMID = '{a}'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }


        public DataGridResult GetBranch(SearchItem item)
        {
            string sql = $@"SELECT ID,NAME FROM BRANCH WHERE 1=1";
            item.HasKey("ID,", a => sql += $" and ID = '{a}'");
            item.HasKey("NAME", a => sql += $" and NAME = '{a}'");
            sql += " ORDER BY  ID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public DataGridResult GetBranchElement(SearchItem item)
        {
            string sql = $@"select * from BRANCH where 1=1 ";
            item.HasKey("ID", a => sql += $" and ID = '{a}'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }


        public DataGridResult GetConfig(SearchItem item)
        {
            string sql = $@"select * from CONFIG where 1=1 ";
            item.HasKey("ID", a => sql += $" and ID = '{a}'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public DataGridResult GetFkfs(SearchItem item)
        {
            string sql = $@"select ID,NAME from FKFS where 1=1";
            item.HasKey("ID", a => sql += $" and ID = '{a}'");
            sql += "order by ID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetOperationrule(SearchItem item)
        {
            string sql = $@"select * from OPERATIONRULE where 1=1 ";
            item.HasKey("ID", a => sql += $" and ID = '{a}'");
            sql += "order by ID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }


        public DataGridResult GetFeeRule(SearchItem item)
        {
            string sql = $@"select * from FEERULE where 1=1 ";
            item.HasKey("ID", a => sql += $" and ID = '{a}'");
            sql += "order by ID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetLateFeeRule(SearchItem item)
        {
            string sql = $@"select A.ID,A.NAME,A.DAYS,A.AMOUNTS from LATEFEERULE A where 1=1 ";
            item.HasKey("ID", a => sql += $" and A.ID = '{a}'");
            sql += "order by A.ID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetLateFeeRuleElement(SearchItem item)
        {
            string sql = $@"select A.ID,A.NAME,A.DAYS,A.AMOUNTS from LATEFEERULE A where 1=1 ";
            item.HasKey("ID", a => sql += $" and A.ID = '{a}'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetFloor(SearchItem item)
        {
            string sql = $@"SELECT A.CODE,A.NAME FROM FLOOR A WHERE 1=1";
            item.HasKey("ID,", a => sql += $" and A.CODE = '{a}'");
            item.HasKey("NAME", a => sql += $" and A.NAME = '{a}'");
            sql += " ORDER BY  A.CODE";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public DataGridResult GetFloorElement(SearchItem item)
        {
            string sql = $@"select A.* from FLOOR A where 1=1 ";
            item.HasKey("ID", a => sql += $" and A.CODE = '{a}'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetShop(SearchItem item)
        {
            string sql = $@"SELECT A.CODE,A.NAME FROM SHOP A WHERE 1=1";
            item.HasKey("CODE,", a => sql += $" and A.CODE = '{a}'");
            item.HasKey("NAME", a => sql += $" and A.NAME = '{a}'");
            sql += " ORDER BY  A.CODE";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public DataGridResult GetShopElement(SearchItem item)
        {
            string sql = $@"select A.* from SHOP A where 1=1 ";
            item.HasKey("SHOPID", a => sql += $" and A.SHOPID = '{a}'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetEnergyFiles(SearchItem item)
        {
            string sql = $@"SELECT A.FILECODE,A.FILENAME FROM ENERGY_FILES A WHERE 1=1";
            item.HasKey("FILECODE,", a => sql += $" and A.FILECODE = '{a}'");
            item.HasKey("FILENAME", a => sql += $" and A.FILENAME = '{a}'");
            sql += " ORDER BY  A.FILECODE";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public DataGridResult GetEnergyFilesElement(SearchItem item)
        {
            string sql = $@"select A.* from ENERGY_FILES A where 1=1 ";
            item.HasKey("FILEID", a => sql += $" and A.FILEID = '{a}'");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public DataGridResult GetPeriod(SearchItem item)
        {
            string sql = $@"select YEARMONTH,to_char(DATE_START,'yyyy-mm-dd') DATE_START,to_char(DATE_END,'yyyy-mm-dd')"+
                " DATE_END from PERIOD where 1=1";
            item.HasKey("YEAR", a => sql += $" and substr(YEARMONTH,1,4) = '{a}'");
            sql += "order by YEARMONTH";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        public DataGridResult GetStaion(SearchItem item)
        {
            string sql = $@"select STATIONBH from STATION where 1=1 ";
            item.HasKey("STATIONBH", a => sql += $" and STATIONBH = '{a}'");
            sql += "order by STATIONBH";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);                        
            return new DataGridResult(dt, count);
        }
        public object GetStaionElement(STATIONEntity DefineSave)
        {
            string sql = $@"select STATIONBH,TYPE,IP from STATION where 1=1 ";
            sql += " and STATIONBH = " + DefineSave.STATIONBH.ToString();
            sql += "order by STATIONBH";
            

            DataTable dt = DbHelper.ExecuteTable(sql);

            var sqlpay = $@"SELECT S.STATIONBH,S.PAYID,P.NAME from STATION_PAY S,PAY P WHERE S.PAYID=P.PAYID ";
            sqlpay += " and STATIONBH = " + DefineSave.STATIONBH.ToString();
            sqlpay += " order by S.PAYID";
            DataTable dt1 = DbHelper.ExecuteTable(sqlpay);

            var result = new
            {
                staion = dt,
                station_pay = new dynamic[]
                {
                    dt1
                }
            };
            return result;
        }

        public object GetStaionPayList()
        {
            string sql = $@"SELECT PAYID,NAME FROM PAY  ";
            sql += "order by PAYID";

            DataTable dt = DbHelper.ExecuteTable(sql);

            var result = new
            {
                pay = dt
            };
            return result;
        }
        

        public string SaveSataion(STATIONEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            if (DefineSave.STATIONBH.IsEmpty())
                DefineSave.STATIONBH = CommonService.NewINC("STATION").PadLeft(6, '0');
            v.Require(a => a.STATIONBH);
            v.Require(a => a.TYPE);
            v.Require(a => a.IP);
            v.IsUnique(a => a.IP);

            DefineSave.STATION_PAY.ForEach(sdb =>
            {
                GetVerify(sdb).Require(a => a.PAYID);                
            });

            v.Verify();
            DbHelper.Save(DefineSave);

            return DefineSave.STATIONBH;
        }

        public void DeleteStation(STATIONEntity DeleteData)
        {
            var v = GetVerify(DeleteData);
            string sql = $@"delete from STATION_PAY where  STATIONBH="  + DeleteData.STATIONBH.ToString();
            DbHelper.ExecuteNonQuery(sql);
            //校验
            DbHelper.Delete(DeleteData);
        }

        public string SaveBrand(BRANDEntity SaveData)
        {
            var v = GetVerify(SaveData);
            if (SaveData.ID.IsEmpty())
                SaveData.ID = CommonService.NewINC("BRAND");
            SaveData.STATUS = "0";
            SaveData.REPORTER = employee.Id;
            SaveData.REPORTER_NAME = employee.Name;
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            v.Require(a => a.ID);
            v.Require(a => a.NAME);
            v.Require(a => a.CATEGORYID);
            v.IsNumber(a => a.ID);
            v.IsNumber(a => a.CATEGORYID);
            v.IsUnique(a => a.ID);
            v.IsUnique(a => a.NAME);
            v.Verify();
            DbHelper.Save(SaveData);
            return SaveData.ID;
        }

        public virtual UIResult TreeCategoryData(SearchItem item)
        {
            string sql = $@"select * from CATEGORY where 1=1 ";
            item.HasKey("CATEGORYCODE", a => sql += $" and CATEGORYCODE = '{a}' ");
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public virtual UIResult TreeCategoryList()
        {
            List<CATEGORYEntity> p = DbHelper.SelectList(new CATEGORYEntity());
            return new UIResult(TreeModel.Create(p,
                a => a.CATEGORYCODE,
                a => new TreeModel()
                {
                    code = a.CATEGORYCODE,
                    title = a.CATEGORYNAME,                    
                    expand = true
                }).ToArray());
        }

    }

}
