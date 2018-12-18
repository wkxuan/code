using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace z.DBHelper.DBDomain
{
    /// <summary>
    /// 存储过程字段的描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
        Inherited = true, AllowMultiple = false)]
    public class ProcedureFieldAttribute : Attribute
    {
        public ProcedureFieldAttribute(string fieldname)
        {
            Fieldname = fieldname;
        }
        public ProcedureFieldAttribute(string fieldname, int size)
        {
            Fieldname = fieldname;
            Size = size;
        }
        public ProcedureFieldAttribute(string fieldname, int size, ParameterDirection direction)
        {
            Fieldname = fieldname;
            Size = size;
            Direction = direction;
        }
        public string Fieldname;
        public ParameterDirection Direction = ParameterDirection.InputOutput;
        public int Size = 100;
    }
}
