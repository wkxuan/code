using z.DBHelper.DBDomain;
using z.DGS.Entities;


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
            //double sumPayAmount = req.payList.Sum(a => a.amount);
        }




    }
}