using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.MVC5.Models;

namespace z.ERP.Web.Areas.Share.Render
{
    public class WindowBaseRender : VueRender
    {
        public override string ControllerMothod
        {
            get
            {
                return "WindowBase";
            }
        }



        public string Title
        {
            get;
            set;
        }
        
    }
}