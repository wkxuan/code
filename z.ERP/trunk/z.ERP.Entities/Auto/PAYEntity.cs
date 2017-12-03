/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 16:59:01
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("PAY", "")]
    public partial class PAYEntity : EntityBase
    {
        public PAYEntity()
        {
        }

        public PAYEntity(string payid)
        {
            PAYID = payid;
        }

        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string PAYID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string CODE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string NAME
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
        public string VOID_FLAG
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string FK
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string JF
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string ZLFS
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string FLAG
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string CREATE_TIME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string UPDATE_TIME
        {
            get; set;
        }
    }
}
