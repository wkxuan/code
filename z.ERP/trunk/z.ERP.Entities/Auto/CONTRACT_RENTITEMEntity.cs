/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:10
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("CONTRACT_RENTITEM", "合同租金明细表")]
    public partial class CONTRACT_RENTITEMEntity : TableEntityBase
    {
        public CONTRACT_RENTITEMEntity()
        {
        }

        public CONTRACT_RENTITEMEntity(string contractid, string inx, string yearmonth, string startdate)
        {
            CONTRACTID = contractid;
            INX = inx;
            YEARMONTH = yearmonth;
            STARTDATE = startdate;
        }

        /// <summary>
        /// 合同号
        /// <summary>
        [PrimaryKey]
        [Field("合同号")]
        public string CONTRACTID
        {
            get; set;
        }
        /// <summary>
        /// 时间段
        /// <summary>
        [PrimaryKey]
        [Field("时间段")]
        public string INX
        {
            get; set;
        }
        /// <summary>
        /// 年月
        /// <summary>
        [PrimaryKey]
        [Field("年月")]
        public string YEARMONTH
        {
            get; set;
        }
        /// <summary>
        /// 开始日期
        /// <summary>
        [PrimaryKey]
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

        /// <summary>
        /// 区间清算标记
        /// <summary>
        [Field("区间清算标记")]
        public string QJQSBJ
        {
            get; set;
        }

        
    }
}
