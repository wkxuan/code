using z.DBHelper.DBDomain;
using z.DGS.Entities;
using System;
using System.Linq;
using System.Data;
using z.Extensions;


namespace z.DGS.Services
{


    public class DgsService : ServiceBase
    {

        internal DgsService()
        {

        }

        public string Test()
        {
            return DbHelper.ExecuteTable("select Ip from STATION where TYPE=3  and STATIONBH= {{code}}", new zParameter("code", employee.Id)).Rows[0][0].ToString();
        }



        public void SaleGather(SaleGatherReq req)
        {
            double sumPayAmount = req.payList.Sum(a => a.payAmount);

            if(sumPayAmount != req.amount)
            {
                throw new Exception("销售金额不等于支付明细合计");
            }


            string sql = $"select 1 from salegather where POSNO='{employee.PlatformId}' and DEALID={req.dealId} ";

            DataTable Dt = DbHelper.ExecuteTable(sql);

            if (Dt.IsNotNull())
            {
                throw new Exception("小票号[" + req.dealId.ToString() + "]已存在");
            }

            string[] sqlarr = new string[1 + req.payList.Count];

            sqlarr[0] =  "  insert into salegather(posno,dealid,saletime,amount)";
            sqlarr[0] += $" values('{employee.PlatformId}',{req.dealId},";

            if (!String.IsNullOrEmpty(req.saleTime))
                sqlarr[0] += $"to_date('{req.saleTime}','yyyy-mm-dd HH24:MI:SS'),";
            else
                sqlarr[0] += "sysdate,";

            sqlarr[0] += $"{req.amount})";

            int j = 0;

            for (int i = 1; i <= req.payList.Count; i++)
            {
                sqlarr[i] = "insert into salegather_pay(posno,dealid,paytype,payamount)";
                sqlarr[i] += $"values('{employee.PlatformId}',{req.dealId},{req.payList[j].payType},";
                sqlarr[i] += $"{req.payList[j].payAmount})";
                j++;
            }

            try
            {
                using (var Tran = DbHelper.BeginTransaction())
                {
                    DbHelper.ExecuteNonQuery(sqlarr);
                    Tran.Commit();
                }
            }
            catch (Exception e)
            {

                throw new Exception("写数据发生异常:" + e);
            }


        }
    }
}