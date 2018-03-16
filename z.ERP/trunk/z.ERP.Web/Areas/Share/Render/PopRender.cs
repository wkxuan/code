using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.MVC5.Models;

namespace z.ERP.Web.Areas.Share.Render
{
    public class PopRender: VueRender
    {
        public override string ControllerMothod
        {
            get
            {
                return "Pop";
            }
        }
        public string CallBack
        {
            get;
            set;
        }

        public string Width {
            get;
            set;
        }
        public string ParentObj {
            get;
            set;
        }
        public string Caption {
            get;
            set;
        }
        public string PopLx {
            get;
            set;
        }
        public string ParentBind {
            get;
            set;
        }
    }
}