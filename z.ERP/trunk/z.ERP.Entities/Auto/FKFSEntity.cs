/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-19 0:12:02
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6560.42822
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

        /// <summary>
        /// 代码
        /// <summary>
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
