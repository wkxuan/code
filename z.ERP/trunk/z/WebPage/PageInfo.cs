using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace z.WebPage
{
    /// <summary>
    /// 页面的信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class PageInfoAttribute : Attribute
    {
        /// <summary>
        /// 菜单号
        /// </summary>
        public string MenuId
        {
            get; set;
        }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName
        {
            get; set;
        }

    }
}
