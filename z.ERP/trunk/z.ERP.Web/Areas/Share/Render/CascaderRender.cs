using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace z.ERP.Web.Areas.Share.Render
{
    public class CascaderRender: VueRender
    {
        public override string ControllerMothod
        {
            get
            {
                return "Cascader";
            }
        }

        public string Change
        {
            get;
            set;
        }
        public string Data
        {
            get;
            set;
        }
    }
}