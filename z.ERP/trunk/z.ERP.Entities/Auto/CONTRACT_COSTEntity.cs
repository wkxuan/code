/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-18 0:07:19
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6560.42822
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("CONTRACT_COST", "合同收费项目")]
    public partial class CONTRACT_COSTEntity : EntityBase
    {
        public CONTRACT_COSTEntity()
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
        /// 序号
        /// <summary>
        [Field("序号")]
        public string INDEX
        {
            get; set;
        }
        /// <summary>
        /// 项目ID
        /// <summary>
        [Field("项目ID")]
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
        /// 收费方式
        /// <summary>
        [Field("收费方式")]
        public string SFFS
        {
            get; set;
        }
        /// <summary>
        /// 单价
        /// <summary>
        [Field("单价")]
        public string PRICE
        {
            get; set;
        }
        /// <summary>
        /// 费用
        /// <summary>
        [Field("费用")]
        public string COST
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
        /// 收费规则
        /// <summary>
        [Field("收费规则")]
        public string FEERULEID
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
