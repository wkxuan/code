/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:16
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("ORG", "组织机构")]
    public partial class ORGEntity : TableEntityBase
    {
        public ORGEntity()
        {
        }

        public ORGEntity(string orgid)
        {
            ORGID = orgid;
        }

        /// <summary>
        /// 机构编码
        /// <summary>
        [PrimaryKey]
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
