﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.SSO;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.Home.Index
{
    public class IndexController : BaseController
    {
        // GET: Home/Index
        public ActionResult Index()
        {
            return View();
        }

        public UIResult GetMenu(MENUEntity data)
        {
            string host = Request.Url.Host;
            return service.HomeService.GetMenuNew(data, host);
        }
        public UIResult AllTopData()
        {           
            var boxDclrwdata = service.DefaultDataService.BoxDclrwData();  //待处理任务            
            return new UIResult(
                new
                {
                    dclrwdata = boxDclrwdata,
                    dclrwcount= boxDclrwdata.Rows.Count>0 ? boxDclrwdata.Rows.Count:0,
                }
                );
        }
    }
}