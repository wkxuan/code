/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/11/20 0:06:12
 * 生成人：书房
 * 代码生成器版本号：1.1.6533.181
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Domain.Auto
{
    [DbTable("HT")]
    public class HTInfo : DomainBase
    {
        /// <summary>
        /// 
        /// <summary>
        public string HTH
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string TYPE
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
        public string GHDWDM
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string QDSJ
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string HTYXQ_START
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string HTYXQ_END
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string ZXQK
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string YXBJ
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string STATUS
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string HSFS
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string HTDM
        {
            get; set;
        }
    }
}
