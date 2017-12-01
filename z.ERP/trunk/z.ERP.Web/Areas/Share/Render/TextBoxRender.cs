using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.MVC5.Models;

namespace z.ERP.Web.Areas.Share.Render
{
    public class TextBoxRender : VueRender
    {
        public override string View
        {
            get
            {
                return "TextBox";
            }
        }
        /// <summary>
        /// 水印说明
        /// </summary>
        public string Placeholder
        {
            get;
            set;
        }
    }
}