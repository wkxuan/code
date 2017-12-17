/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-18 0:07:20
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6560.42822
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("CONTRACT_UPDATE", "合同变更启动")]
    public partial class CONTRACT_UPDATEEntity : EntityBase
    {
        public CONTRACT_UPDATEEntity()
        {
        }

        /// <summary>
        /// 合同号
        /// <summary>
        [Field("合同号")]
        public string CONTRACTID
        {
            get; set;
        }
        /// <summary>
        /// 合同号OLD
        /// <summary>
        [Field("合同号OLD")]
        public string CONTRACTID_OLD
        {
            get; set;
        }
        /// <summary>
        /// 启动人
        /// <summary>
        [Field("启动人")]
        public string INITINATE
        {
            get; set;
        }
        /// <summary>
        /// 启动名称
        /// <summary>
        [Field("启动名称")]
        public string INITINATE_NAME
        {
            get; set;
        }
        /// <summary>
        /// 启动时间
        /// <summary>
        [Field("启动时间")]
        [DbType(DbType.DateTime)]
        public string INITINATE_TIME
        {
            get; set;
        }
    }
}
