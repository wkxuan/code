using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace z.DBHelper.DBDomain
{
    public class zParameter
    {
        public zParameter()
        {
        }

        public zParameter(string name, object value, DbType? type = null)
        {
            Name = name;
            Value = value;
            if (type.HasValue)
                Type = type.Value;
        }

        public string Name
        {
            get;
            set;
        }

        public DbType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }
        private DbType type = DbType.String;

        public object Value
        {
            get;
            set;
        }

        public bool IsArray
        {
            get
            {
                return Value.GetType().IsArray;
            }
        }
        
    }
}
