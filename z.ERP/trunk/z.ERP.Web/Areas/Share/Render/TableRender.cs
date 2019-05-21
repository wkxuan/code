using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace z.ERP.Web.Areas.Share.Render
{
    public class TableRender : VueRender
    {
        public override string ControllerMothod
        {
            get
            {
                return "Table";
            }
        }

        public string Height
        {
            get;
            set;
        }
        public string Size
        {
            get;
            set;
        }
        public string Border
        {
            get;
            set;
        }

        public string Columns
        {
            get;
            set;
        }
        public string Data
        {
            get;
            set;
        }
        public string Ref
        {
            get;
            set;
        }
    }
}