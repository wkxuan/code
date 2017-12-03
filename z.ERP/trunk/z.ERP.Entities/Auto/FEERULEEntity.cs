/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 16:58:57
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("FEERULE", "")]
    public partial class FEERULEEntity : EntityBase
    {
        public FEERULEEntity()
        {
        }

        public FEERULEEntity(string id)
        {
            ID = id;
        }

        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string ID
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
        public string UP_DATE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string PAY_CYCLE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string PAY_UP_CYCLE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string ADVANCE_CYCLE
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
