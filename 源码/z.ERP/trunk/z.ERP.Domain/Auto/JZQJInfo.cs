/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/11/20 0:06:13
 * 生成人：书房
 * 代码生成器版本号：1.1.6533.181
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Domain.Auto
{
    [DbTable("JZQJ")]
    public class JZQJInfo : DomainBase
    {
        /// <summary>
        /// 
        /// <summary>
        public string YEARMONTH
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string DEPTID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string KSRQ
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string JSRQ
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string SHOPID
        {
            get; set;
        }
    }
}
