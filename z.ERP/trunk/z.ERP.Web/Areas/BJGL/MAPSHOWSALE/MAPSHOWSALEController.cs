using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.BJGL.MAPSHOWSALE
{
    public class MAPSHOWSALEController : BaseController
    {
        public ActionResult MAPSHOWSALE()
        {
            ViewBag.Title = "布局销售信息";
            return View();
        }
        public UIResult GetInitMAPDATA(string floorid, string shopstatus, string starttime, string endtime)
        {
            var datas = service.DpglService.GetInitMAPDATA(floorid, shopstatus);
            var saledata = service.DpglService.GETFLOORSALELIST(floorid, starttime, endtime);
            return new UIResult(
                new
                {
                    floorInfo = datas.Item1,
                    labelArray = datas.Item2,
                    salelist = saledata
                }
                );
        }
        public UIResult GetSHOPSALE(string shopid,string starttime,string endtime,string type)
        {
            var data = service.DpglService.GetSHOPSALE(shopid, starttime,endtime);
            var datas= service.DpglService.GetSHOPSALEPERCENT(shopid, starttime, endtime,type);
            var shopsalepercent= service.DefaultDataService.EchartData(datas, "NAME", "VALUE");
            return new UIResult(
                new
                {
                    shopdata = data.Item1,
                    shopsalelistY = data.Item2.AsEnumerable().Select<DataRow, decimal>(x => Convert.ToDecimal(x["AMOUNT"])).ToList<decimal>(),
                    shopsalelistX = data.Item2.AsEnumerable().Select<DataRow, string>(x => Convert.ToString(x["RQS"])).ToList<string>(),
                    shopsalepercent = shopsalepercent
                });
        }
        public UIResult GetTypeChange(string shopid, string starttime, string endtime, string type) {
            var datas = service.DpglService.GetSHOPSALEPERCENT(shopid, starttime, endtime, type);
            var shopsalepercent = service.DefaultDataService.EchartData(datas, "NAME", "VALUE");
            return new UIResult(
                new
                {
                    shopsalepercent = shopsalepercent
                });
        }
    }
}