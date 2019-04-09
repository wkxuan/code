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

        //是否不可见 初始值false
        public bool Invisible
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
        public string Type
        {
            get;
            set;
        }
        public string Icon
        {
            get;
            set;
        }

        public string PermissionKey
        {
            get;
            set;
        }

        public bool HasPermission
        {
            get;
            set;
        }
        public string Size
        {
            get;
            set;
        }
    }
}