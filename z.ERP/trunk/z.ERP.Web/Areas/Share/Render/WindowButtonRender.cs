using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.MVC5.Models;

namespace z.ERP.Web.Areas.Share.Render
{
    public class WindowButtonRender : ButtonRender
    {
        public override string ControllerMothod
        {
            get
            {
                return "WindowButton";
            }
        }

        //public WindowRender render
        //{
        //    get;
        //    set;
        //}
    }
}