using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.SSO;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

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
            //var box1data = service.DefaultDataService.Box1Data();
            var box2data = service.DefaultDataService.Box2Data();
            var box3data = service.DefaultDataService.Box3Data(type);
            var box6data = service.DefaultDataService.Box6Data(type);
            return new UIResult(
                new
                {
                    //box1data = box1data,
                    box2data = box2data,
                    box3data = box3data,
                    box6data = box6data,
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