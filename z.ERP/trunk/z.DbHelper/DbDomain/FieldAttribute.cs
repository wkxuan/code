﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace z.DBHelper.DBDomain
{
    /// <summary>
    /// 字段的描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
        Inherited = true, AllowMultiple = false)]
    public class FieldAttribute : Attribute
    {
        public string Fieldname;
        public FieldAttribute(string fieldname)
        {
            Fieldname = fieldname;
        }
    }
}
