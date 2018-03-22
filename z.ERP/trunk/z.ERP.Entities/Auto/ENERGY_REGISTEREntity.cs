/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:11
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("ENERGY_REGISTER", "能源设备登记(抄表)")]
    public partial class ENERGY_REGISTEREntity : TableEntityBase
    {
        public ENERGY_REGISTEREntity()
        {
        }

        public ENERGY_REGISTEREntity(string billid)
        {
            BILLID = billid;
        }

        /// <summary>
        /// 单号
        /// <summary>
        [PrimaryKey]
        [Field("单号")]
        public string BILLID
        {
            get; set;
        }
        /// <summary>
        /// 抄表日期
        /// <summary>
        [Field("抄表日期")]
        [DbType(DbType.DateTime)]
        public string CHECK_DATE
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
        /// 状态
        /// <summary>
        [Field("状态")]
        public string STATUS
        {
            get; set;
        }
        /// <summary>
        /// 登记人
        /// <summary>
        [Field("登记人")]
        public string REPORTER
        {
            get; set;
        }
        /// <summary>
        /// 登记人名称
        /// <summary>
        [Field("登记人名称")]
        public string REPORTER_NAME
        {
            get; set;
        }
        /// <summary>
        /// 登记日期
        /// <summary>
        [Field("登记日期")]
        [DbType(DbType.DateTime)]
        public string REPORTER_TIME
        {
            get; set;
        }
        /// <summary>
        /// 审核人
        /// <summary>
        [Field("审核人")]
        public string VERIFY
        {
            get; set;
        }
        /// <summary>
        /// 审核人名称
        /// <summary>
        [Field("审核人名称")]
        public string VERIFY_NAME
        {
            get; set;
        }
        /// <summary>
        /// 审核日期
        /// <summary>
        [Field("审核日期")]
        [DbType(DbType.DateTime)]
        public string VERIFY_TIME
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
