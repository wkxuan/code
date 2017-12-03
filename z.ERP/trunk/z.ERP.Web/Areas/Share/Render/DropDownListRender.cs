using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.MVC5.Models;
using z.Results;

namespace z.ERP.Web.Areas.Share.Render
{
    public class DropDownListRender : VueRender
    {
        public DropDownListRender()
        {
            EmptyText = "---请选择---";
        }
        public override string View
        {
            get
            {
                return "BaseDropDownList";
            }
        }

        /// <summary>
        /// 数据
        /// </summary>
        public virtual List<SelectItem> Data
        {
            get;
            set;
        }

        /// <summary>
        /// 当前值
        /// </summary>
        public virtual string Value
        {
            get; set;
        }

        /// <summary>
        /// 空值显示文字,如果为空,则不显示
        /// </summary>
        public virtual string EmptyText
        {
            get;
            set;
        }
    }
}