using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.MVC5.Models;

namespace z.ERP.Web.Areas.Share.Render
{
    public class PopsRender: VueRender
    {
        public override string ControllerMothod
        {
            get
            {
                return "Pops";
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
        public string Src {
            get;
            set;
        }
        public string ParentBind {
            get;
            set;
        }
        public string ClickOk
        {
            get;
            set;
        }
        public string ClickCancel
        {
            get;
            set;
        }
        public string VisibleChange
        {
            get;
            set;
        }
        public object Param
        {
            get;
            set;
        }
        public object Ref
        {
            get;
            set;
        }
    }
}