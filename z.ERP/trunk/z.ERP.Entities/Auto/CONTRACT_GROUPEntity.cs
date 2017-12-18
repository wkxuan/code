/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-19 0:12:01
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6560.42822
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("CONTRACT_GROUP", "扣率组")]
    public partial class CONTRACT_GROUPEntity : EntityBase
    {
        public CONTRACT_GROUPEntity()
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
        /// 扣率组
        /// <summary>
        [Field("扣率组")]
        public string GROUPNO
        {
            get; set;
        }
        /// <summary>
        /// 基础扣率
        /// <summary>
        [Field("基础扣率")]
        public string JSKL
        {
            get; set;
        }
        /// <summary>
        /// 描述
        /// <summary>
        [Field("描述")]
        public string DESCRIPTION
        {
            get; set;
        }
    }
}
