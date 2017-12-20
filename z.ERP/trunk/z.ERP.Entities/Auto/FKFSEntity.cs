/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/19 20:30:47
 * 生成人：书房
 * 代码生成器版本号：1.2.6562.36915
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("FKFS", "付款方式")]
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
        /// 代码
        /// <summary>
        [PrimaryKey]
        [Field("代码")]
        public string ID
        {
            get; set;
        }
        /// <summary>
        /// 名称
        /// <summary>
        [Field("名称")]
        public string NAME
        {
            get; set;
        }
    }
}
