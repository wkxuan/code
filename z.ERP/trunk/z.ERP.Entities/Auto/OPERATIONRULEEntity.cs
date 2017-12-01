/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 0:49:27
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("OPERATIONRULE", "")]
    public partial class OPERATIONRULEEntity : EntityBase
    {
        public OPERATIONRULEEntity()
        {
        }

        public OPERATIONRULEEntity(string id)
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
        public string NAME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string WYSIGN
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string PROCESSTYPE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string LADDERSIGN
        {
            get; set;
        }
    }
}
