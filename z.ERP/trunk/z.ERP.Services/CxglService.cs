using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.Encryption;
using z.ERP.Entities;
using z.ERP.Entities.Enum;
using z.Extensions;
using z.ERP.Model.Vue;
using z.Exceptions;
using z.MVC5.Results;
using z.SSO.Model;

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

        /// <summary>
        /// 赠品定义
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public DataGridResult PresentSql(SearchItem item)
        {
            string sql = $@"SELECT BRANCHID, BRANCH.NAME, ID, NAME, PRICE, STATUS
                            FROM PRESENT,BRANCH
                            WHERE BRANCH.ID=PRESENT.BRANCHID";
            sql += "  AND PRESENT.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            item.HasKey("BRANCHID", a => sql += $" and BRANCHID LIKE '%{a}%'");
            item.HasKey("ID", a => sql += $" and ID LIKE '%{a}%'");
            item.HasKey("NAME", a => sql += $" and NAME LIKE '%{a}%'");
            item.HasKey("PRICE", a => sql += $" and PRICE LIKE '%{a}%'");
            item.HasKey("STATUS", a => sql += $" and STATUS LIKE '%{a}%'");
            sql += " ORDER BY ID DESC";
            int count;
            var dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            dt.NewEnumColumns<促销单状态>("STATUS", "STATUSMC");
            return new DataGridResult(dt, count);

          

        }
        public DataGridResult Present(SearchItem item)
        {
            string sql = "";
            int count;
            var dt = DbHelper.ExecuteTable(sql, item.PageInfo, out count);
            return new DataGridResult(dt, count);
        }

        public DataTable PresentDetail(SearchItem item)
        {
            //DataGridResult sql = PresentSql(item);
            string sql = "";
            DataTable dt = DbHelper.ExecuteTable(sql);
        
            return dt;
        }
        public DataTable GetPresent(PresentEntity data)
        {
            string sql = "";
            string yTQx = GetPermissionSql(PermissionType.Category);
            
            //string sql = PresentSql(item);
            //    $@"SELECT BRANCHID, BRANCH.NAME, HEAD, TAIL, ADQRCODE, ADCONTENT
            //                    FROM PRESENT,BRANCH
            //                    WHERE BRANCH.ID=PRESENT.BRANCHID";
            //sql += "  AND TICKETINFO.BRANCHID in (" + GetPermissionSql(PermissionType.Branch) + ")";  //门店权限
            sql += " and Present.ID=" + data.ID;
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt;
        }

    }
}
