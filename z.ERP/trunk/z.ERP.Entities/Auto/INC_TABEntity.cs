﻿/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/11/24 0:48:20
 * 生成人：书房
 * 代码生成器版本号：1.2.6537.1447
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("INC_TAB", "")]
    public partial class INC_TABEntity : EntityBase
    {
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string TBLNAME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string VALUE
        {
            get; set;
        }
    }
}
