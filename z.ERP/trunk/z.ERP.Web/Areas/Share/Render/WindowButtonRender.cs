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

        /// <summary>
        /// 对应窗体绑定对象
        /// </summary>
        public string WindowModel
        {
            get;
            set;
        }
        
    }
}