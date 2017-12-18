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
    [DbTable("CONTRACT_PAY", "合同收款方式")]
    public partial class CONTRACT_PAYEntity : EntityBase
    {
        public CONTRACT_PAYEntity()
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
        /// 收款方式
        /// <summary>
        [Field("收款方式")]
        public string PAYID
        {
            get; set;
        }
        /// <summary>
        /// 收费项目
        /// <summary>
        [Field("收费项目")]
        public string TERMID
        {
            get; set;
        }
        /// <summary>
        /// 开始日期
        /// <summary>
        [Field("开始日期")]
        [DbType(DbType.DateTime)]
        public string STARTDATE
        {
            get; set;
        }
        /// <summary>
        /// 结束日期
        /// <summary>
        [Field("结束日期")]
        [DbType(DbType.DateTime)]
        public string ENDDATE
        {
            get; set;
        }
        /// <summary>
        /// 扣款比例
        /// <summary>
        [Field("扣款比例")]
        public string KL
        {
            get; set;
        }
        /// <summary>
        /// 滞纳规则
        /// <summary>
        [Field("滞纳规则")]
        public string ZNGZID
        {
            get; set;
        }
    }
}
