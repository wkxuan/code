﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;

namespace z.ERP.Web.Areas.Share
{
    public class ShareController : BaseController
    {
        // GET: Share/Share
        public ActionResult Index()
        {
            return View();
        }
    }
}