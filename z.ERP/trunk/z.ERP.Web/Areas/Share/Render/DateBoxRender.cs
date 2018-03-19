using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.MVC5.Models;

namespace z.ERP.Web.Areas.Share.Render
{
    public class DateBoxRender : VueRender
    {
        public override string ControllerMothod
        {
            get
            {
                return "DateBox";
            }
        }
        public string Change
        {
            get;
            set;
        }
        public string Type
        {
            get;
            set;
        }
    }
}