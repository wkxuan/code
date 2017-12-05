using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.Results;

namespace z.ERP.Web.Areas.Share.Render
{
    public class CheckBoxListRender : VueRender
    {
        public override string ControllerMothod
        {
            get
            {
                return "CheckBoxList";
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
        /// 多选
        /// </summary>
        public virtual bool MultiSelect
        {
            get;
            set;
        }

        public virtual string[] DisableValue
        {
            get;
            set;
        }
    }
}