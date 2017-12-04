using System;
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
            //("XTGL", "BRAND", "Brand");
            //("XTGL", "test", "List");
              //("HTGL", "ZLHT", "List");
               ("XTGL", "PAY", "Pay");
    }
}
