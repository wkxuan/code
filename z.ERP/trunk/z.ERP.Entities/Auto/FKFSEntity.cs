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
    [DbTable("FKFS", "")]
    public partial class FKFSEntity : EntityBase
    {
        public FKFSEntity()
        {
        }

        public FKFSEntity(string id)
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
    }
}
