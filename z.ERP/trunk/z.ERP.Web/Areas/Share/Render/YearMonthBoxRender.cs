using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.MVC5.Models;

namespace z.ERP.Web.Areas.Share.Render
{
    public class YearMonthBoxRender : VueRender
    {
        public override string ControllerMothod
        {
            get
            {
                return "YearMonthBox";
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