using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace z.WebPage
{
    /// <summary>
    /// 操作的特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class OperationAttribute : Attribute
    {
        /// <summary>
        /// 一个操作特性
        /// </summary>
        /// <param name="operationid">操作id</param>
        public OperationAttribute(string operationid)
        {
            OperationId = operationid;
        }
        /// <summary>
        /// 操作id
        /// </summary>
        public string OperationId
        {
            get;
            set;
        }
    }
}
