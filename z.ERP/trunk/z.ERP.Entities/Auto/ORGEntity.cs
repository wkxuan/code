/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-18 0:07:21
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6560.42822
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("ORG", "组织机构")]
    public partial class ORGEntity : EntityBase
    {
        public ORGEntity()
        {
        }

        /// <summary>
        /// 机构编码
        /// <summary>
        [Field("机构编码")]
        public string ORGID
        {
            get; set;
        }
        /// <summary>
        /// 机构代码
        /// <summary>
        [Field("机构代码")]
        public string ORGCODE
        {
            get; set;
        }
        /// <summary>
        /// 机构名称
        /// <summary>
        [Field("机构名称")]
        public string ORGNAME
        {
            get; set;
        }
        /// <summary>
        /// 机构类型
        /// <summary>
        [Field("机构类型")]
        public string ORG_TYPE
        {
            get; set;
        }
        /// <summary>
        /// 末级标志
        /// <summary>
        [Field("末级标志")]
        public string LEVEL_LAST
        {
            get; set;
        }
        /// <summary>
        /// 卖场ID
        /// <summary>
        [Field("卖场ID")]
        public string BRANCHID
        {
            get; set;
        }
        /// <summary>
        /// 作废标记
        /// <summary>
        [Field("作废标记")]
        public string VOID_FLAG
        {
            get; set;
        }
    }
}
