using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.MVC5.Models;

namespace z.ERP.Web.Areas.Share.Render
{
    public class CommonWindowRender : WindowBaseRender
    {


        public override string ControllerMothod
        {
            get
            {
                return "CommonWindow";
            }
        }

        /// <summary>
        /// 主键
        /// </summary>
        public string Id
        {
            get;
            set;
        }
    }
}