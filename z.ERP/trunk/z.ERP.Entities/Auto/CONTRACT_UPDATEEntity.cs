/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/11 22:18:58
 * 生成人：书房
 * 代码生成器版本号：1.2.6562.36915
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

        public CONTRACT_UPDATEEntity(string contractid, string contractid_old)
        {
            CONTRACTID = contractid;
            CONTRACTID_OLD = contractid_old;
        }

        /// <summary>
        /// 合同号
        /// <summary>
        [PrimaryKey]
        [Field("合同号")]
        public string CONTRACTID
        {
            get; set;
        }
        /// <summary>
        /// 合同号OLD
        /// <summary>
        [PrimaryKey]
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
