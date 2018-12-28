using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using z;
using z.Extensions;

namespace z.DBHelper.DBDomain
{
    /// <summary>
    /// 所有数据操作类的基类
    /// </summary>
    public class ProcedureEntityBase : EntityBase
    {
        public ProcedureEntityBase()
        {
        }

        /// <summary>
        /// 获取存储过程名字
        /// </summary>
        /// <returns></returns>
        public string GetProcedureName()
        {
            return this.GetAttribute<DbProcedureAttribute>()?.ProcedureName;
        }

        /// <summary>
        /// 获取指定字段的属性
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PropertyInfo GetProcedureField(string name)
        {
            return GetType()
                .GetProperties()
                .FirstOrDefault(a => a.GetAttribute<ProcedureFieldAttribute>()?.Fieldname == name);
        }

        /// <summary>
        /// 获取所有字段
        /// </summary>
        /// <returns></returns>
        public PropertyInfo[] GetAllProcedureField()
        {
            return GetType().GetProperties()
                   .Where(a => a.GetAttribute<ProcedureFieldAttribute>() != null)
                   .ToArray();
        }
    }

}
