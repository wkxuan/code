using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.MVC5.Models;

namespace z.ERP.Web.Areas.Share.Render
{
    public class DropDownListRender : ControlRenderBase
    {
        public override string View
        {
            get
            {
                return "DropDownList";
            }
        }
    }
}