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
    [DbTable("CONTRACT_RENTITEM", "合同租金明细表")]
    public partial class CONTRACT_RENTITEMEntity : EntityBase
    {
        public CONTRACT_RENTITEMEntity()
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
        /// 时间段
        /// <summary>
        [Field("时间段")]
        public string INX
        {
            get; set;
        }
        /// <summary>
        /// 年月
        /// <summary>
        [Field("年月")]
        public string YEARMONTH
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
        /// 租金
        /// <summary>
        [Field("租金")]
        public string RENTS
        {
            get; set;
        }
        /// <summary>
        /// 生成日期
        /// <summary>
        [Field("生成日期")]
        [DbType(DbType.DateTime)]
        public string CREATEDATE
        {
            get; set;
        }
        /// <summary>
        /// 清算标记
        /// <summary>
        [Field("清算标记")]
        public string QSBJ
        {
            get; set;
        }
        /// <summary>
        /// 缴费单编号
        /// <summary>
        [Field("缴费单编号")]
        public string BILLID
        {
            get; set;
        }
    }
}
