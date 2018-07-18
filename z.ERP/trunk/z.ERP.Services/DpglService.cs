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
    public class DpglService : ServiceBase
    {
        internal DpglService()
        {
        }
        public DataGridResult SearchFloor(SearchItem item)
        {
            string sql = $@"SELECT A.ID,A.CODE,A.NAME FROM FLOOR A WHERE 1=1";
            item.HasKey("ID,", a => sql += $" and A.ID = {a}");
            item.HasKey("CODE,", a => sql += $" and A.CODE = '{a}'");
            item.HasKey("NAME", a => sql += $" and A.NAME = '{a}'");
            item.HasKey("BRANCHID", a => sql += $" and A.BRANCHID = {a}");
            sql += " ORDER BY  A.ID";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        /// <summary>
        /// 可返回一行楼层记录或符合条件的所有记录
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Tuple<dynamic,DataTable> GetFloor(FLOOREntity Data)
        {
            string sql = $@"select A.*,B.ORGIDCASCADER from FLOOR A,ORG B where A.ORGID=B.ORGID(+) ";
            if (!Data.ID.IsEmpty())
                sql += (" AND A.ID= " + Data.ID);
            if (!Data.CODE.IsEmpty())
                sql += (" AND A.CODE= " + Data.CODE);
            if (!Data.NAME.IsEmpty())
                sql += (" AND A.NAME like %" + Data.NAME+"%");
            DataTable floor = DbHelper.ExecuteTable(sql);
            return new Tuple<dynamic,DataTable>(floor.ToOneLine(),floor);
        }
        public DataGridResult SearchShop(SearchItem item)
        {
            string sql = $@"SELECT  A.*,A.CODE SHOPCODE,A.AREA_BUILD AREA,B.CATEGORYCODE,B.CATEGORYNAME,D.NAME BRANCHNAME,F.NAME FLOORNAME " +
                   "  FROM SHOP A,CATEGORY B,ORG C,BRANCH D,FLOOR F "
                   + " WHERE  A.CATEGORYID = B.CATEGORYID(+) and A.ORGID=C.ORGID(+)"
                   + " and  A.BRANCHID = D.ID and A.FLOORID=F.ID";
            item.HasKey("CODE", a => sql += $" and A.CODE like '%{a}%'");
            item.HasKey("NAME", a => sql += $" and A.NAME like '%{a}%'");
            item.HasKey("BRANCHID", a => sql += $" and A.BRANCHID = '{a}'");
            item.HasKey("FLOORID", a => sql += $" and A.FLOORID = '{a}'");
            sql += " ORDER BY  A.CODE";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }
        /// <summary>
        /// 可返回一行店铺记录或符合条件的所有记录
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Tuple<dynamic, DataTable> GetShop(SHOPEntity Data)
        {
            string sql = $@"SELECT  A.*,B.CATEGORYCODE,B.CATEGORYNAME,B.CATEGORYIDCASCADER,A.AREA_BUILD AREA,C.ORGIDCASCADER " +
                   "  FROM SHOP A,CATEGORY B,ORG C WHERE  A.CATEGORYID = B.CATEGORYID(+) "
                   + " and A.ORGID=C.ORGID(+)";
            if (!Data.SHOPID.IsEmpty())
                sql += (" AND A.SHOPID= " + Data.SHOPID);
            if (!Data.CODE.IsEmpty())
                sql += (" AND A.CODE= " + Data.CODE);
            if (!Data.NAME.IsEmpty())
                sql += (" AND A.NAME like %" + Data.NAME + "%");
            DataTable shop = DbHelper.ExecuteTable(sql);
            return new Tuple<dynamic, DataTable>(shop.ToOneLine(), shop);
        }
        public object GetOneShop(SHOPEntity Data)
        {
            string sql = " SELECT  A.* FROM SHOP A " +
                "  WHERE  1=1 ";
            if (!Data.SHOPID.IsEmpty())
                sql += (" and A.SHOPID= " + Data.SHOPID);
            DataTable dt = DbHelper.ExecuteTable(sql);
            return new
            {
                dt = dt.ToOneLine()
            };
        }
        public DataGridResult GetAssetChangeList(SearchItem item)
        {
            string sql = $@"SELECT A.*,B.NAME BRANCHNAME FROM ASSETCHANGE A,BRANCH B WHERE A.BRANCHID=B.ID ";
            item.HasKey("CHANGE_TYPE", a => sql += $" and CHANGE_TYPE = '{a}'");
            item.HasKey("STATUS", a => sql += $" and A.STATUS = '{a}'");
            item.HasKey("BRANCHID", a => sql += $" and A.BRANCHID  = '{a}'");
            item.HasKey("DISCRIPTION", a => sql += $" and A.DISCRIPTION  LIKE '%{a}%'");
            sql += " ORDER BY  BILLID DESC";
            int count;
            DataTable dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }

        public void DeleteAssetChange(List<ASSETCHANGEEntity> DeleteData)
        {
            foreach (var item in DeleteData)
            {
                ASSETCHANGEEntity Data = DbHelper.Select(item);
                if (Data.STATUS == ((int)普通单据状态.审核).ToString())
                {
                    throw new LogicException("已经审核不能删除!");
                }
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                foreach (var item in DeleteData)
                {
                    DbHelper.Delete(item);
                }
                Tran.Commit();
            }
        }

        public string SaveAssetChange(ASSETCHANGEEntity SaveData)
        {
            var v = GetVerify(SaveData);
            if (SaveData.BILLID.IsEmpty())
                SaveData.BILLID = NewINC("ASSETCHANGE");
            SaveData.STATUS = ((int)普通单据状态.未审核).ToString();
            SaveData.REPORTER = employee.Id;
            SaveData.REPORTER_NAME = employee.Name;
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            SaveData.VERIFY = employee.Id;
            v.Require(a => a.BILLID);
            v.Require(a => a.CHANGE_TYPE);
            v.Require(a => a.BRANCHID);
            v.Verify();

            using (var Tran = DbHelper.BeginTransaction())
            {
                SaveData.ASSETCHANGEITEM2?.ForEach(newasset =>
               {
                   GetVerify(newasset).Require(a => a.ASSETID);
               });
                DbHelper.Save(SaveData);

                Tran.Commit();
            }
            return SaveData.BILLID;
        }


        public Tuple<dynamic, DataTable, DataTable> GetAssetChangeElement(ASSETCHANGEEntity Data)
        {
            //此处校验一次只能查询一个单号,校验单号必须存在
            string sql = $@"SELECT A.*,B.NAME BRANCHNAME FROM ASSETCHANGE A,BRANCH B  WHERE A.BRANCHID=B.ID ";
            if (!Data.BILLID.IsEmpty())
                sql += (" AND BILLID= " + Data.BILLID);
            DataTable assetchange = DbHelper.ExecuteTable(sql);
            assetchange.NewEnumColumns<普通单据状态>("STATUS", "STATUSMC");

            string sqlitem = $@"SELECT M.*,P.CODE " +
                " FROM ASSETCHANGEITEM M，SHOP P " +
                " where M.ASSETID=P.SHOPID ";
            if (!Data.BILLID.IsEmpty())
                sqlitem += (" and M.BILLID= " + Data.BILLID);
            DataTable assetchangeitem = DbHelper.ExecuteTable(sqlitem);

            string sqlitem2 = $@"SELECT M.*,P.CODE " +
                " FROM ASSETCHANGEITEM2 M，SHOP P " +
                " where M.ASSETID=P.SHOPID ";
            if (!Data.BILLID.IsEmpty())
                sqlitem2 += (" and M.BILLID= " + Data.BILLID);
            DataTable assetchangeitem2 = DbHelper.ExecuteTable(sqlitem2);

            return new Tuple<dynamic, DataTable, DataTable>(assetchange.ToOneLine(), assetchangeitem, assetchangeitem2);
        }
        /// <summary>
        /// 资产变更审核
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public string ExecData(ASSETCHANGEEntity Data)
        {
            ASSETCHANGEEntity assetchange = DbHelper.Select(Data);
            if (assetchange.STATUS == ((int)普通单据状态.审核).ToString())
            {
                throw new LogicException("单据(" + Data.BILLID + ")已经审核不能再次审核!");
            }
            using (var Tran = DbHelper.BeginTransaction())
            {
                assetchange.VERIFY = employee.Id;
                assetchange.VERIFY_NAME = employee.Name;
                assetchange.VERIFY_TIME = DateTime.Now.ToString();
                assetchange.STATUS = ((int)普通单据状态.审核).ToString();
                DbHelper.Save(assetchange);
                Tran.Commit();
            }
            return assetchange.BILLID;
        }


    }
}
