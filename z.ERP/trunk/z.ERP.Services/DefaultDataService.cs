using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.MVC5.Results;
using z.SSO.Model;

namespace z.ERP.Services
{
    public class DefaultDataService: ServiceBase
    {
        internal DefaultDataService()
        {
        }
        /// <summary>
        /// 模块1数据
        /// </summary>
        /// <returns></returns>
        public DataTable Box1Data(string branchid) {
            string sql = "";
            if (branchid=="") {   //查询所有权限门店数据
                sql = @"SELECT '昨日' TIME,NVL(AMOUNT,0) AMOUNT,NVL(AMOUNTTB,0) AMOUNTTB,NVL(AMOUNTHB,0) AMOUNTHB FROM (
                            SELECT distinct A.BRANCHID,A1.AMOUNT,ROUND(A1.AMOUNT/A2.AMOUNT,2) AMOUNTHB,ROUND(A1.AMOUNT/A3.AMOUNT,2) AMOUNTTB FROM CONTRACT_SUMMARY A 
                            LEFT JOIN (SELECT BRANCHID,NVL(SUM(AMOUNT),0) AMOUNT FROM CONTRACT_SUMMARY WHERE TRUNC(RQ)=TRUNC(SYSDATE-1) AND BRANCHID IN (" + GetPermissionSql(PermissionType.Branch) + @") GROUP BY BRANCHID) A1 ON A.BRANCHID=A1.BRANCHID
                            LEFT JOIN (SELECT BRANCHID,NVL(SUM(AMOUNT),0) AMOUNT FROM CONTRACT_SUMMARY WHERE TRUNC(RQ)=TRUNC(SYSDATE-2) AND BRANCHID IN (" + GetPermissionSql(PermissionType.Branch) + @") GROUP BY BRANCHID) A2 ON A.BRANCHID=A2.BRANCHID
                            LEFT JOIN (SELECT BRANCHID,NVL(SUM(AMOUNT),0) AMOUNT FROM CONTRACT_SUMMARY WHERE to_char(RQ, 'yyyy-MM-dd')= to_char(add_Months(sysdate-1, -12), 'yyyy-MM-dd') AND BRANCHID IN (" + GetPermissionSql(PermissionType.Branch) + @") GROUP BY BRANCHID) A3 ON A.BRANCHID=A3.BRANCHID
                            )
                            UNION ALL
                            SELECT '本月' TIME,NVL(AMOUNT,0) AMOUNT,NVL(AMOUNTTB,0) AMOUNTTB,NVL(AMOUNTHB,0) AMOUNTHB FROM (
                            SELECT distinct A.BRANCHID,A1.AMOUNT,ROUND(A1.AMOUNT/A2.AMOUNT,2) AMOUNTHB,ROUND(A1.AMOUNT/A3.AMOUNT,2) AMOUNTTB FROM CONTRACT_SUMMARY A 
                            LEFT JOIN (SELECT BRANCHID,NVL(SUM(AMOUNT),0) AMOUNT FROM CONTRACT_SUMMARY WHERE to_char(RQ, 'yyyy-MM')=to_char(sysdate, 'yyyy-MM') AND BRANCHID IN (" + GetPermissionSql(PermissionType.Branch) + @") GROUP BY BRANCHID) A1 ON A.BRANCHID=A1.BRANCHID
                            LEFT JOIN (SELECT BRANCHID,NVL(SUM(AMOUNT),0) AMOUNT FROM CONTRACT_SUMMARY WHERE to_char(RQ, 'yyyy-MM')=to_char(add_Months(sysdate, -1), 'yyyy-MM') AND BRANCHID IN (" + GetPermissionSql(PermissionType.Branch) + @") GROUP BY BRANCHID) A2 ON A.BRANCHID=A2.BRANCHID
                            LEFT JOIN (SELECT BRANCHID,NVL(SUM(AMOUNT),0) AMOUNT FROM CONTRACT_SUMMARY WHERE to_char(RQ, 'yyyy-MM')= to_char(add_Months(sysdate, -12), 'yyyy-MM') AND BRANCHID IN (" + GetPermissionSql(PermissionType.Branch) + @") GROUP BY BRANCHID) A3 ON A.BRANCHID=A3.BRANCHID
                            )
                            UNION ALL
                            SELECT '上月' TIME,NVL(AMOUNT,0) AMOUNT,NVL(AMOUNTTB,0) AMOUNTTB,NVL(AMOUNTHB,0) AMOUNTHB FROM (
                            SELECT distinct A.BRANCHID,A1.AMOUNT,ROUND(A1.AMOUNT/A2.AMOUNT,2) AMOUNTHB,ROUND(A1.AMOUNT/A3.AMOUNT,2) AMOUNTTB FROM CONTRACT_SUMMARY A 
                            LEFT JOIN (SELECT BRANCHID,NVL(SUM(AMOUNT),0) AMOUNT FROM CONTRACT_SUMMARY WHERE to_char(RQ, 'yyyy-MM')=to_char(add_Months(sysdate, -1), 'yyyy-MM') AND BRANCHID IN (" + GetPermissionSql(PermissionType.Branch) + @") GROUP BY BRANCHID) A1 ON A.BRANCHID=A1.BRANCHID
                            LEFT JOIN (SELECT BRANCHID,NVL(SUM(AMOUNT),0) AMOUNT FROM CONTRACT_SUMMARY WHERE to_char(RQ, 'yyyy-MM')=to_char(add_Months(sysdate, -2), 'yyyy-MM') AND BRANCHID IN (" + GetPermissionSql(PermissionType.Branch) + @") GROUP BY BRANCHID) A2 ON A.BRANCHID=A2.BRANCHID
                            LEFT JOIN (SELECT BRANCHID,NVL(SUM(AMOUNT),0) AMOUNT FROM CONTRACT_SUMMARY WHERE to_char(RQ, 'yyyy-MM')= to_char(add_Months(sysdate, -13), 'yyyy-MM') AND BRANCHID IN (" + GetPermissionSql(PermissionType.Branch) + @") GROUP BY BRANCHID) A3 ON A.BRANCHID=A3.BRANCHID
                            )
                            ";
            }
            else { 
                sql = @"SELECT '昨日' TIME,NVL(AMOUNT,0) AMOUNT,NVL(AMOUNTTB,0) AMOUNTTB,NVL(AMOUNTHB,0) AMOUNTHB FROM (
                            SELECT distinct A.BRANCHID,A1.AMOUNT,ROUND(A1.AMOUNT/A2.AMOUNT,2) AMOUNTHB,ROUND(A1.AMOUNT/A3.AMOUNT,2) AMOUNTTB FROM CONTRACT_SUMMARY A 
                            LEFT JOIN (SELECT BRANCHID,NVL(SUM(AMOUNT),0) AMOUNT FROM CONTRACT_SUMMARY WHERE TRUNC(RQ)=TRUNC(SYSDATE-1) AND BRANCHID=" + branchid+ @" GROUP BY BRANCHID) A1 ON A.BRANCHID=A1.BRANCHID
                            LEFT JOIN (SELECT BRANCHID,NVL(SUM(AMOUNT),0) AMOUNT FROM CONTRACT_SUMMARY WHERE TRUNC(RQ)=TRUNC(SYSDATE-2) AND BRANCHID=" + branchid + @" GROUP BY BRANCHID) A2 ON A.BRANCHID=A2.BRANCHID
                            LEFT JOIN (SELECT BRANCHID,NVL(SUM(AMOUNT),0) AMOUNT FROM CONTRACT_SUMMARY WHERE to_char(RQ, 'yyyy-MM-dd')= to_char(add_Months(sysdate-1, -12), 'yyyy-MM-dd') AND BRANCHID=" + branchid + @" GROUP BY BRANCHID) A3 ON A.BRANCHID=A3.BRANCHID
                            )
                            UNION ALL
                            SELECT '本月' TIME,NVL(AMOUNT,0) AMOUNT,NVL(AMOUNTTB,0) AMOUNTTB,NVL(AMOUNTHB,0) AMOUNTHB FROM (
                            SELECT distinct A.BRANCHID,A1.AMOUNT,ROUND(A1.AMOUNT/A2.AMOUNT,2) AMOUNTHB,ROUND(A1.AMOUNT/A3.AMOUNT,2) AMOUNTTB FROM CONTRACT_SUMMARY A 
                            LEFT JOIN (SELECT BRANCHID,NVL(SUM(AMOUNT),0) AMOUNT FROM CONTRACT_SUMMARY WHERE to_char(RQ, 'yyyy-MM')=to_char(sysdate, 'yyyy-MM') AND BRANCHID=" + branchid + @" GROUP BY BRANCHID) A1 ON A.BRANCHID=A1.BRANCHID
                            LEFT JOIN (SELECT BRANCHID,NVL(SUM(AMOUNT),0) AMOUNT FROM CONTRACT_SUMMARY WHERE to_char(RQ, 'yyyy-MM')=to_char(add_Months(sysdate, -1), 'yyyy-MM') AND BRANCHID=" + branchid + @" GROUP BY BRANCHID) A2 ON A.BRANCHID=A2.BRANCHID
                            LEFT JOIN (SELECT BRANCHID,NVL(SUM(AMOUNT),0) AMOUNT FROM CONTRACT_SUMMARY WHERE to_char(RQ, 'yyyy-MM')= to_char(add_Months(sysdate, -12), 'yyyy-MM') AND BRANCHID=" + branchid + @" GROUP BY BRANCHID) A3 ON A.BRANCHID=A3.BRANCHID
                            )
                            UNION ALL
                            SELECT '上月' TIME,NVL(AMOUNT,0) AMOUNT,NVL(AMOUNTTB,0) AMOUNTTB,NVL(AMOUNTHB,0) AMOUNTHB FROM (
                            SELECT distinct A.BRANCHID,A1.AMOUNT,ROUND(A1.AMOUNT/A2.AMOUNT,2) AMOUNTHB,ROUND(A1.AMOUNT/A3.AMOUNT,2) AMOUNTTB FROM CONTRACT_SUMMARY A 
                            LEFT JOIN (SELECT BRANCHID,NVL(SUM(AMOUNT),0) AMOUNT FROM CONTRACT_SUMMARY WHERE to_char(RQ, 'yyyy-MM')=to_char(add_Months(sysdate, -1), 'yyyy-MM') AND BRANCHID=" + branchid + @" GROUP BY BRANCHID) A1 ON A.BRANCHID=A1.BRANCHID
                            LEFT JOIN (SELECT BRANCHID,NVL(SUM(AMOUNT),0) AMOUNT FROM CONTRACT_SUMMARY WHERE to_char(RQ, 'yyyy-MM')=to_char(add_Months(sysdate, -2), 'yyyy-MM') AND BRANCHID=" + branchid + @" GROUP BY BRANCHID) A2 ON A.BRANCHID=A2.BRANCHID
                            LEFT JOIN (SELECT BRANCHID,NVL(SUM(AMOUNT),0) AMOUNT FROM CONTRACT_SUMMARY WHERE to_char(RQ, 'yyyy-MM')= to_char(add_Months(sysdate, -13), 'yyyy-MM') AND BRANCHID=" + branchid + @" GROUP BY BRANCHID) A3 ON A.BRANCHID=A3.BRANCHID
                            )
                            ";
            }
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt;
        }
        /// <summary>
        /// 模块2数据
        /// </summary>
        /// <returns></returns>
        public DataTable Box2Data(string branchid)
        {
            string sql = @"SELECT (CASE RENT_STATUS  WHEN 2 THEN '正在经营' WHEN 1 THEN '闲置招租' end)TYPE,COUNT(1) NUMBERS,SUM(A.AREA_RENTABLE) AREA
                              FROM SHOP A  WHERE BRANCHID  IN (" + GetPermissionSql(PermissionType.Branch) + @")";
            if (branchid != "") {
                sql += " AND BRANCHID=" + branchid ;
            }
            sql+=" GROUP BY RENT_STATUS";

            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt;
        }
        /// <summary>
        /// 模块3数据
        /// </summary>
        /// <param name="type">数据日期 1.昨日2.本月3.上月</param>
        /// <returns></returns>
        public DataTable Box3Data(string type,string branchid)
        {
            string sql = "";
            if (type == "1")
            {
                sql = @"SELECT * FROM (
                            select row_number() over(order by SUM(AMOUNT) desc) NO,B.NAME SHOPNAME,S.AREA_RENTABLE AREA,SUM(AMOUNT) AMOUNT
                             from CONTRACT_SUMMARY A,BRAND B,SHOP S
                             WHERE A.BRANDID=B.ID AND A.SHOPID=S.SHOPID AND TO_CHAR(RQ,'yyyy-MM-dd')=TO_CHAR(SYSDATE-1,'yyyy-MM-dd') AND A.BRANCHID IN (" + GetPermissionSql(PermissionType.Branch) + ")";
                if (branchid != "")
                {
                    sql += " AND A.BRANCHID=" + branchid;
                }
                sql += @" GROUP BY B.NAME,S.AREA_RENTABLE) Z
                      WHERE ROWNUM <=15";
            }
            else if (type == "2")
            {
                sql = @"SELECT * FROM (
                            select row_number() over(order by SUM(AMOUNT) desc) NO,B.NAME SHOPNAME,S.AREA_RENTABLE AREA,SUM(AMOUNT) AMOUNT
                             from CONTRACT_SUMMARY A,BRAND B,SHOP S
                             WHERE A.BRANDID=B.ID AND A.SHOPID=S.SHOPID AND TO_CHAR(RQ,'yyyy-MM')=TO_CHAR(SYSDATE,'yyyy-MM') AND A.BRANCHID IN (" + GetPermissionSql(PermissionType.Branch) + ")";
                if (branchid != "")
                {
                    sql += " AND A.BRANCHID=" + branchid;
                }
                sql += @" GROUP BY B.NAME,S.AREA_RENTABLE) Z
                             WHERE ROWNUM <=15";
            }
            else if (type == "3")
            {    //
                sql = @"SELECT* FROM (select row_number() over(order by SUM(AMOUNT) desc) NO, B.NAME SHOPNAME, S.AREA_RENTABLE AREA, SUM(AMOUNT) AMOUNT
                             from CONTRACT_SUMMARY A, BRAND B,SHOP S
                             WHERE A.BRANDID=B.ID AND A.SHOPID=S.SHOPID AND TO_CHAR(RQ, 'yyyy-MM') = TO_CHAR(ADD_MONTHS(SYSDATE, -1), 'yyyy-MM') AND A.BRANCHID  IN (" + GetPermissionSql(PermissionType.Branch) + ")";
                if (branchid != "")
                {
                    sql += " AND A.BRANCHID=" + branchid;
                }
                sql += @" GROUP BY B.NAME, S.AREA_RENTABLE) Z
                              WHERE ROWNUM <= 15";
            }
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt;
        }
        /// <summary>
        /// 模块4数据
        /// </summary>
        /// <param name="type">数据日期 1.昨日2.本月3.上月</param>
        /// <returns></returns>
        public DataTable Box4Data(string type, string branchid)
        {
            string sql = "";
            if (type == "1") {
                sql = @"SELECT * FROM (
                        select row_number() over(order by SUM(AMOUNT) desc) NO,C.CATEGORYNAME AREANAME,SUM(B.AREA_RENTABLE) AREA,SUM(AMOUNT) AMOUNT
                         from CONTRACT_SUMMARY A,SHOP B,CATEGORY C
                         WHERE A.SHOPID=B.SHOPID AND B.CATEGORYID=C.CATEGORYID AND TO_CHAR(RQ,'yyyy-MM-dd')=TO_CHAR(SYSDATE-1,'yyyy-MM-dd')  AND A.BRANCHID  IN (" + GetPermissionSql(PermissionType.Branch) + ")";
                if (branchid != "")
                {
                    sql += " AND A.BRANCHID=" + branchid;
                }
                sql += @" GROUP BY C.CATEGORYNAME) Z
                         WHERE ROWNUM <=10";
            }
            else if (type == "2")
            {
                sql = @"SELECT * FROM (
                        select row_number() over(order by SUM(AMOUNT) desc) NO,C.CATEGORYNAME AREANAME,SUM(B.AREA_RENTABLE) AREA,SUM(AMOUNT) AMOUNT
                         from CONTRACT_SUMMARY A,SHOP B,CATEGORY C
                         WHERE A.SHOPID=B.SHOPID AND B.CATEGORYID=C.CATEGORYID AND TO_CHAR(RQ,'yyyy-MM')=TO_CHAR(SYSDATE,'yyyy-MM')  AND A.BRANCHID  IN (" + GetPermissionSql(PermissionType.Branch) + ")";
                if (branchid != "")
                {
                    sql += " AND A.BRANCHID=" + branchid;
                }
                sql += @" GROUP BY C.CATEGORYNAME) Z
                         WHERE ROWNUM <=10";
            }
            else if (type == "3")
            {
                sql = @"SELECT * FROM (
                        select row_number() over(order by SUM(AMOUNT) desc) NO,C.CATEGORYNAME AREANAME,SUM(B.AREA_RENTABLE) AREA,SUM(AMOUNT) AMOUNT
                         from CONTRACT_SUMMARY A,SHOP B,CATEGORY C
                         WHERE A.SHOPID=B.SHOPID AND B.CATEGORYID=C.CATEGORYID AND TO_CHAR(RQ,'yyyy-MM')=TO_CHAR(ADD_MONTHS(SYSDATE, -1),'yyyy-MM')  AND A.BRANCHID  IN (" + GetPermissionSql(PermissionType.Branch) + ")";
                if (branchid != "")
                {
                    sql += " AND A.BRANCHID=" + branchid;
                }
                sql += @" GROUP BY C.CATEGORYNAME) Z
                         WHERE ROWNUM <=10";
            }
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt;
        }

        public DataTable BoxDclrwData()
        {
            var sql = " select B.MENUID,M.NAME,C.NAME BRANCHMC ,B.URL,count(*) COUNT,min(BILLID) BILLID,P.ID PLATFORMID ,P.DOMAIN DOMAIN  from BILLSTATUS B,MENU M,BRANCH C,PLATFORM P " +
                "  where B.MENUID=M.ID and B.BRABCHID =C.ID AND P.ID=M.PLATFORMID " +
                " and exists (select 1 from USER_ROLE U,ROLE_MENU N where U.ROLEID = N.ROLEID and N.MENUID = M.ID and U.USERID =  " + employee.Id +") "+
                "  group by B.MENUID,M.NAME,C.NAME,B.URL,P.ID,P.DOMAIN ";
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt;
        }
        public DataTable EchartData(DataTable data,string value,string value1) {
            DataTable dt = new DataTable();
            if (data.Rows.Count > 0)
            {
                dt.Columns.Add("value", typeof(string));
                dt.Columns.Add("name", typeof(string));
                foreach (DataRow item in data.Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["name"] = item[value];
                    dr["value"] = item[value1];
                    dt.Rows.Add(dr);
                }
            }
            else {
                dt = data;
            }
            return dt;
        }

        public DataTable Echart3Data(string branchid) {
            string sql = @" SELECT to_char(RQ, 'yyyy-MM-dd') TIME, SUM(NVL(AMOUNT,0)) AMOUNT FROM  CONTRACT_SUMMARY WHERE TRUNC(RQ)<TRUNC(SYSDATE) and TRUNC(RQ)>=TRUNC(SYSDATE-30) ";
            if (branchid != "")
            {
                sql += " AND BRANCHID=" + branchid;
            }
            sql += @" GROUP BY RQ ORDER by RQ";

            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt;
        }
    }
}
