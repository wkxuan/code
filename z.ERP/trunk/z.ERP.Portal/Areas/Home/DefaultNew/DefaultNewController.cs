using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.SSO;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;
using System.Data;

namespace z.ERP.Portal.Areas.Home.Default
{
    public class DefaultNewController : BaseController
    {
        public ActionResult DefaultNew()
        {
            return View();
        }
        public UIResult BoxData(string branchid)
        {
            var branch = service.DataService.branch();
            var box1data = service.DefaultDataService.Box1Data(branchid);   //经营总贶
            var box2data = service.DefaultDataService.Box2Data(branchid);   //店铺出租状态
            var box3data = service.DefaultDataService.Box3Data("1", branchid);   //店铺经营榜    //type=1 初始化默认昨日数据
            var box4data = service.DefaultDataService.Box4Data("1", branchid);    //业态经营榜   //type=1 初始化默认昨日数据
            //echartData
            var Echart1data = box3data;
            DataView dv = Echart1data.DefaultView;   //排行需要正序
            dv.Sort = "NO DESC";
            Echart1data = dv.ToTable();
            var Echart2Numberdata = service.DefaultDataService.EchartData(box2data, "TYPE", "NUMBERS");
            var Echart2Areadata = service.DefaultDataService.EchartData(box2data, "TYPE", "AREA");
            var Echart3data = service.DefaultDataService.Echart3Data(branchid);
            return new UIResult(
                new
                {
                    branchList = branch,
                    box1data = box1data,
                    box2data = box2data,
                    box3data = box3data,
                    box4data = box4data,
                    //echartData
                    Echart1Xdata = Echart1data.AsEnumerable().Select<DataRow, decimal>(x => Convert.ToDecimal(x["AMOUNT"])).ToList<decimal>(),
                    Echart1Ydata = Echart1data.AsEnumerable().Select<DataRow, string>(x => Convert.ToString(x["SHOPNAME"])).ToList<string>(),
                    Echart2Numberdata = Echart2Numberdata,
                    Echart2Areadata = Echart2Areadata,
                    Echart3Xdata = Echart3data.AsEnumerable().Select<DataRow, string>(x => Convert.ToString(x["TIME"])).ToList<string>(),
                    Echart3Ydata = Echart3data.AsEnumerable().Select<DataRow, decimal>(x => Convert.ToDecimal(x["AMOUNT"])).ToList<decimal>(),
                }
                );
        }
        public UIResult Box3Data(string type,string branchid)
        {
            return new UIResult(service.DefaultDataService.Box3Data(type, branchid));
        }
        public UIResult Box4Data(string type, string branchid)
        {
            return new UIResult(service.DefaultDataService.Box4Data(type,branchid));
        }
    }
}