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
    [DbTable("FEERULE", "收费规则")]
    public partial class FEERULEEntity : EntityBase
    {
        public FEERULEEntity()
        {
        }

        /// <summary>
        /// 收费规则编号
        /// <summary>
        [Field("收费规则编号")]
        public string ID
        {
            get; set;
        }
        /// <summary>
        /// 收费规则名称
        /// <summary>
        [Field("收费规则名称")]
        public string NAME
        {
            get; set;
        }
        /// <summary>
        /// 缴费截至日
        /// <summary>
        [Field("缴费截至日")]
        public string UP_DATE
        {
            get; set;
        }
        /// <summary>
        /// 缴费周期
        /// <summary>
        [Field("缴费周期")]
        public string PAY_CYCLE
        {
            get; set;
        }
        /// <summary>
        /// 缴费截至周期
        /// <summary>
        [Field("缴费截至周期")]
        public string PAY_UP_CYCLE
        {
            get; set;
        }
        /// <summary>
        /// 提前周期
        /// <summary>
        [Field("提前周期")]
        public string ADVANCE_CYCLE
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
        /// 出单日期
        /// <summary>
        [Field("出单日期")]
        public string FEE_DAY
        {
            get; set;
        }
    }
}
