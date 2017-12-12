using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace z.MVC5.Models
{
    public abstract class ControlRenderBase
    {

        /// <summary>
        /// 控件控制器
        /// </summary>
        public virtual string ControllerName
        {
            get
            {
                return "Share";
            }
        }
        /// <summary>
        /// 控件名称
        /// </summary>
        public abstract string ControllerMothod
        {
            get;
        }

        /// <summary>
        /// 控件ID
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// 可用性
        /// </summary>
        public bool Enable
        {
            get
            {
                return _enable;
            }
            set
            {
                _enable = value;
            }
        }

        /// <summary>
        /// 可用
        /// </summary>
        private bool _enable = true;
    }
}
