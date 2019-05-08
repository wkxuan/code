using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.MVC5.Results;

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
        public DataTable Box1Data() {
            string sql = @"";

            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt;
        }
        /// <summary>
        /// 模块2数据
        /// </summary>
        /// <returns></returns>
        public DataTable Box2Data()
        {
            string sql = @"SELECT (CASE RENT_STATUS  WHEN 1 THEN '正在经营' WHEN 2 THEN '闲置招租' end)TYPE,COUNT(1) NUMBERS,SUM(A.AREA_RENTABLE) AREA
                              FROM SHOP A
                              GROUP BY RENT_STATUS";

            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt;
        }
        /// <summary>
        /// 模块3数据
        /// </summary>
        /// <param name="type">数据日期 1.昨日2.本月3.上月</param>
        /// <returns></returns>
        public DataTable Box3Data(string type)
        {
            string sql = "";
            if (type == "1")
            {
                sql = @"SELECT * FROM (
                            select row_number() over(order by SUM(AMOUNT) desc) NO,B.NAME SHOPNAME,B.AREA_RENTABLE AREA,SUM(AMOUNT) AMOUNT
                             from CONTRACT_SUMMARY A,SHOP B
                             WHERE A.SHOPID=B.SHOPID AND TO_CHAR(RQ,'yyyy-MM-dd')=TO_CHAR(SYSDATE-1,'yyyy-MM-dd')
                             GROUP BY B.NAME,B.AREA_RENTABLE) Z
                             WHERE ROWNUM <=10";
            }
            else if (type == "2")
            {
                sql = @"SELECT * FROM (
                            select row_number() over(order by SUM(AMOUNT) desc) NO,B.NAME SHOPNAME,B.AREA_RENTABLE AREA,SUM(AMOUNT) AMOUNT
                             from CONTRACT_SUMMARY A,SHOP B
                             WHERE A.SHOPID=B.SHOPID AND TO_CHAR(RQ,'yyyy-MM')=TO_CHAR(SYSDATE,'yyyy-MM')
                             GROUP BY B.NAME,B.AREA_RENTABLE) Z
                             WHERE ROWNUM <=10";
            }
            else if (type == "3")
            {    //
                sql = @"SELECT* FROM (select row_number() over(order by SUM(AMOUNT) desc) NO, B.NAME SHOPNAME, B.AREA_RENTABLE AREA, SUM(AMOUNT) AMOUNT
                             from CONTRACT_SUMMARY A, SHOP B
                             WHERE A.SHOPID = B.SHOPID AND TO_CHAR(RQ, 'yyyy-MM') = TO_CHAR(ADD_MONTHS(SYSDATE, -1), 'yyyy-MM')
                             GROUP BY B.NAME, B.AREA_RENTABLE) Z
                              WHERE ROWNUM <= 10";
            }
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt;
        }
        /// <summary>
        /// 模块6数据
        /// </summary>
        /// <param name="type">数据日期 1.昨日2.本月3.上月</param>
        /// <returns></returns>
        public DataTable Box6Data(string type)
        {
            string sql = "";
            if (type == "1") {
                sql = @"SELECT * FROM (
                        select row_number() over(order by SUM(AMOUNT) desc) NO,C.CATEGORYNAME AREANAME,SUM(B.AREA_RENTABLE) AREA,SUM(AMOUNT) AMOUNT
                         from CONTRACT_SUMMARY A,SHOP B,CATEGORY C
                         WHERE A.SHOPID=B.SHOPID AND B.CATEGORYID=C.CATEGORYID AND TO_CHAR(RQ,'yyyy-MM-dd')=TO_CHAR(SYSDATE-1,'yyyy-MM-dd')
                         GROUP BY C.CATEGORYNAME) Z
                         WHERE ROWNUM <=10";
            }
            else if (type == "2")
            {
                sql = @"SELECT * FROM (
                        select row_number() over(order by SUM(AMOUNT) desc) NO,C.CATEGORYNAME AREANAME,SUM(B.AREA_RENTABLE) AREA,SUM(AMOUNT) AMOUNT
                         from CONTRACT_SUMMARY A,SHOP B,CATEGORY C
                         WHERE A.SHOPID=B.SHOPID AND B.CATEGORYID=C.CATEGORYID AND TO_CHAR(RQ,'yyyy-MM')=TO_CHAR(SYSDATE,'yyyy-MM')
                         GROUP BY C.CATEGORYNAME) Z
                         WHERE ROWNUM <=10";
            }
            else if (type == "3")
            {
                sql = @"SELECT * FROM (
                        select row_number() over(order by SUM(AMOUNT) desc) NO,C.CATEGORYNAME AREANAME,SUM(B.AREA_RENTABLE) AREA,SUM(AMOUNT) AMOUNT
                         from CONTRACT_SUMMARY A,SHOP B,CATEGORY C
                         WHERE A.SHOPID=B.SHOPID AND B.CATEGORYID=C.CATEGORYID AND TO_CHAR(RQ,'yyyy-MM')=TO_CHAR(ADD_MONTHS(SYSDATE, -1),'yyyy-MM')
                         GROUP BY C.CATEGORYNAME) Z
                         WHERE ROWNUM <=10";
            }
            DataTable dt = DbHelper.ExecuteTable(sql);
            return dt;
        }
    }
}
