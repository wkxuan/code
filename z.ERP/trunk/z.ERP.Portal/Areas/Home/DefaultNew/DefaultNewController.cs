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
        public UIResult BoxData(string type)
        {
            var box1data = service.DefaultDataService.Box1Data();   //经营总贶
            var box2data = service.DefaultDataService.Box2Data();   //店铺出租状态
            var box3data = service.DefaultDataService.Box3Data(type);   //店铺经营榜
            var box6data = service.DefaultDataService.Box6Data(type);    //业态经营榜
            var boxDclrwdata = service.DefaultDataService.BoxDclrwData();  //待处理任务
            //echartData
            var Echart1data = service.DefaultDataService.Box3Data("1");
            DataView dv = Echart1data.DefaultView;   //排行需要正序
            dv.Sort = "NO DESC";
            Echart1data = dv.ToTable();
            var Echart2Numberdata = service.DefaultDataService.EchartData(box2data, "TYPE","NUMBERS");
            var Echart2Areadata = service.DefaultDataService.EchartData(box2data, "TYPE", "AREA");
            var Echart3data = service.DefaultDataService.Echart3Data();
            return new UIResult(
                new
                {
                    box1data = box1data,
                    box2data = box2data,
                    box3data = box3data,
                    box6data = box6data,
                    boxDclrwdata = boxDclrwdata,
                    //echartData
                    Echart1Xdata = Echart1data.AsEnumerable().Select<DataRow, decimal>(x => Convert.ToDecimal(x["AMOUNT"])).ToList<decimal>(),
                    Echart1Ydata = Echart1data.AsEnumerable().Select<DataRow, string>(x => Convert.ToString(x["SHOPNAME"])).ToList<string>(),
                    Echart2Numberdata= Echart2Numberdata,
                    Echart2Areadata= Echart2Areadata,
                    Echart3Xdata = Echart3data.AsEnumerable().Select<DataRow, string>(x => Convert.ToString(x["TIME"])).ToList<string>(),
                    Echart3Ydata = Echart3data.AsEnumerable().Select<DataRow, decimal>(x => Convert.ToDecimal(x["AMOUNT"])).ToList<decimal>(),
                }
                );
        }
        public UIResult Box3Data(string type)
        {
            return new UIResult(service.DefaultDataService.Box3Data(type));
        }
        public UIResult Box6Data(string type)
        {
            return new UIResult(service.DefaultDataService.Box6Data(type));
        }
    }
}