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
    [DbTable("CONTJSKL", "合同结算扣率")]
    public partial class CONTJSKLEntity : EntityBase
    {
        public CONTJSKLEntity()
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
        /// 时间段
        /// <summary>
        [Field("时间段")]
        public string INX
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
        /// 销售金额起
        /// <summary>
        [Field("销售金额起")]
        public string SALES_START
        {
            get; set;
        }
        /// <summary>
        /// 销售金额止
        /// <summary>
        [Field("销售金额止")]
        public string SALES_END
        {
            get; set;
        }
        /// <summary>
        /// 结算扣率
        /// <summary>
        [Field("结算扣率")]
        public string JSKL
        {
            get; set;
        }
    }
}
