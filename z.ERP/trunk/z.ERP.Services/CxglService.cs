using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.ERP.Entities;
using z.ERP.Entities.Enum;
using z.Extensions;
using z.MVC5.Results;

namespace z.ERP.Services
{
    public class CxglService:ServiceBase
    {
        internal CxglService()
        {

        }
        public DataGridResult SearchPromotion(SearchItem item)
        {
            string sql = $@"SELECT * FROM PROMOTION WHERE 1=1 ";
            item.HasKey("NAME", a => sql += $" and NAME LIKE '%{a}%'");
            item.HasKey("YEAR", a => sql += $" and YEAR= {a}");
            item.HasKey("CONTENT", a => sql += $" and CONTENT LIKE '%{a}%'");
            item.HasDateKey("START_DATE_START", a => sql += $" and START_DATE >= {a}");
            item.HasDateKey("START_DATE_END", a => sql += $" and START_DATE <= {a}");
            item.HasDateKey("END_DATE_START", a => sql += $" and END_DATE >= {a}");
            item.HasDateKey("END_DATE_END", a => sql += $" and END_DATE <= {a}");
            item.HasKey("STATUS", a => sql += $" and STATUS= {a}");
            sql += " ORDER BY ID DESC";
            int count;
            var dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<促销单状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }
        public PROMOTIONEntity ShowOneData(PROMOTIONEntity data)
        {
            string sql = $@"SELECT * FROM PROMOTION WHERE ID=" + data.ID;
            var res = DbHelper.ExecuteOneObject<PROMOTIONEntity>(sql);
            return res;
        }
        #region 满减方案
        public DataGridResult GetFRPLAN(SearchItem item)
        {
            string sql = $@"SELECT * FROM FR_PLAN WHERE 1=1 ";
            item.HasKey("NAME", a => sql += $" and NAME LIKE '%{a}%'");
            item.HasKey("ID", a => sql += $" and ID= {a}");
            sql += " ORDER BY ID DESC";
            int count;
            var dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<使用状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);
        }
        public string SaveFRPLAN(FR_PLANEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            if (DefineSave.ID.IsEmpty())
                DefineSave.ID = CommonService.NewINC("FR_PLAN");
            v.Require(a => a.NAME);
            v.Require(a => a.LIMIT);
            v.Require(a => a.FRTYPE);
            DefineSave.FR_PLAN_ITEM?.ForEach(sdb =>
            {
                GetVerify(sdb).Require(a => a.ID);
            });
            v.Verify();
            using (var tran = DbHelper.BeginTransaction())
            {
                DbHelper.Save(DefineSave);
                tran.Commit();
            }
            return DefineSave.ID;
        }
        public Tuple<dynamic, DataTable> GetFRPLANInfo(FR_PLANEntity Data)
        {
            string sql = $@"SELECT * FROM FR_PLAN WHERE ID={Data.ID}";

            DataTable dt = DbHelper.ExecuteTable(sql);
            dt.NewEnumColumns<使用状态>("STATUS", "STATUSMC");

            var sql1 = $@"SELECT * from FR_PLAN_ITEM WHERE ID={Data.ID} ";

            sql1 += " order by INX";
            DataTable dt1 = DbHelper.ExecuteTable(sql1);

            return new Tuple<dynamic, DataTable>(dt.ToOneLine(), dt1);
        }
        #endregion
    }
}
