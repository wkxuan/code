using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.MVC5.Models;

namespace z.ERP.Web.Areas.Share.Render
{
    public class ButtonRender : VueRender
    {
        public override string ControllerMothod
        {
            get
            {
                return "Button";
            }
        }
        /// <summary>
        /// 按钮
        /// </summary>
        public string Click
        {
            get;
            set;
        }
        /// <summary>
        /// 文本
        /// </summary>
        public string Text
        {
            get;
            set;
        }
    }
}