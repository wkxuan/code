﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using z.MVC5.Models;

namespace z.ERP.Web
{
    public class DebugSettings
    {
        /// <summary>
        /// 在这里配置起始页,格式为area/controller/action
        /// 只有调试时生效
        /// </summary>
        public static LoaclePage DefaultPage = new LoaclePage
         ("HOME", "Login", "Login");
       //("HOME", "Index", "Index");
        //  ("HOME", "ChangePassword", "ChangePassword");
        //("HOME", "Default", "Default");
       // ("HOME", "DefaultNew", "DefaultNew");
    }
}
