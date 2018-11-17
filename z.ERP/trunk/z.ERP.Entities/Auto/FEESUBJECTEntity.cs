/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:12
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("FEESUBJECT", "收费项目")]
    public partial class FEESUBJECTEntity : TableEntityBase
    {
        public FEESUBJECTEntity()
        {
        }

        public FEESUBJECTEntity(string trimid)
        {
            TRIMID = trimid;
        }

        /// <summary>
        /// 收费项目编号
        /// <summary>
        [PrimaryKey]
        [Field("收费项目编号")]
        public string TRIMID
        {
            get; set;
        }
        /// <summary>
        /// 收费项目名称
        /// <summary>
        [Field("收费项目名称")]
        public string NAME
        {
            get; set;
        }
        /// <summary>
        /// 拼音码
        /// <summary>
        [Field("拼音码")]
        public string PYM
        {
            get; set;
        }
        /// <summary>
        /// 收费项目类型
        /// <summary>
        [Field("收费项目类型")]
        public string TYPE
        {
            get; set;
        }
        /// <summary>
        /// 成本费用标记
        /// <summary>
        [Field("成本费用标记")]
        public string ACCOUNT
        {
            get; set;
        }
        /// <summary>
        /// 现金货扣标记
        /// <summary>
        [Field("现金货扣标记")]
        public string DEDUCTION
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

        /// <summary>
        /// 科目代码
        /// <summary>
        [Field("科目代码")]
        public string SUBJECT_CODE
        {
            get; set;
        }


        [Field("通知单生成方式")]
        public string SCFS_TZD
        {
            get; set;
        }
    }
}
