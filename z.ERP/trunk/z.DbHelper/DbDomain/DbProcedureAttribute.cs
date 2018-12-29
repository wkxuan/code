using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace z.DBHelper.DBDomain
{
    /// <summary>
    /// 存储过程的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class DbProcedureAttribute : Attribute
    {
        /// <summary>
        /// 存储过程的属性
        /// </summary>
        /// <param name="name"></param>
        public DbProcedureAttribute(string name)
        {
            ProcedureName = name;
        }

        /// <summary>
        /// 存储过程
        /// 名称同类名
        /// </summary>
        public DbProcedureAttribute()
        {

        }

        /// <summary>
        /// 存储过程名字
        /// </summary>
        public string ProcedureName
        {
            get;
        }

    }
}
