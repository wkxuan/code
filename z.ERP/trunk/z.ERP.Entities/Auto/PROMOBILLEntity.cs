using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("PROMOBILL", "促销单")]
    public partial class PROMOBILLEntity : TableEntityBase
    {
        public PROMOBILLEntity()
        {
        }

        public PROMOBILLEntity(string id)
        {
            BILLID = id;
        }
        /// <summary>
        /// 促销单id
        /// <summary>
        [PrimaryKey]
        [Field("促销单id")]
        public string BILLID
        {
            get; set;
        }
        /// <summary>
        /// 促销类型
        /// <summary>
        [Field("促销类型")]
        public string PROMOTYPE
        {
            get; set;
        }
        /// <summary>
        /// 门店id
        /// <summary>
        [Field("门店id")]
        public string BRANCHID
        {
            get; set;
        }
        /// <summary>
        /// 营销活动id
        /// <summary>
        [Field("营销活动id")]
        public string PROMOTIONID
        {
            get; set;
        }
        /// <summary>
        /// 开始日期
        /// <summary>
        [Field("开始日期")]
        [DbType(DbType.DateTime)]
        public string START_DATE
        {
            get; set;
        }
        /// <summary>
        /// 结束日期
        /// <summary>
        [Field("结束日期")]
        [DbType(DbType.DateTime)]
        public string END_DATE
        {
            get; set;
        }
        /// <summary>
        /// 促销周期
        /// <summary>
        [Field("促销周期")]
        public string WEEK
        {
            get; set;
        }
        /// <summary>
        /// 开始时间
        /// <summary>
        [Field("开始时间")]
        public string START_TIME
        {
            get; set;
        }
        /// <summary>
        /// 结束时间
        /// <summary>
        [Field("结束时间")]
        public string END_TIME
        {
            get; set;
        }
        /// <summary>
        /// 录入人
        /// <summary>
        [Field("录入人")]
        public string REPORTER
        {
            get; set;
        }
        /// <summary>
        /// 录入人名称
        /// <summary>
        [Field("录入人名称")]
        public string REPORTER_NAME
        {
            get; set;
        }
        /// <summary>
        /// 录入人时间
        /// <summary>
        [Field("录入人时间")]
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
        /// 审核时间
        /// <summary>
        [Field("审核时间")]
        [DbType(DbType.DateTime)]
        public string VERIFY_TIME
        {
            get; set;
        }
        /// <summary>
        /// 启动人
        /// <summary>
        [Field("启动人")]
        public string INITINATE
        {
            get; set;
        }
        /// <summary>
        /// 启动人名称
        /// <summary>
        [Field("启动人名称")]
        public string INITINATE_NAME
        {
            get; set;
        }
        /// <summary>
        /// 启动时间
        /// <summary>
        [Field("启动时间")]
        [DbType(DbType.DateTime)]
        public string INITINATE_TIME
        {
            get; set;
        }
        /// <summary>
        /// 终止人
        /// <summary>
        [Field("终止人")]
        public string TERMINATE
        {
            get; set;
        }
        /// <summary>
        /// 终止人名称
        /// <summary>
        [Field("终止人名称")]
        public string TERMINATE_NAME
        {
            get; set;
        }
        /// <summary>
        /// 终止时间
        /// <summary>
        [Field("终止时间")]
        [DbType(DbType.DateTime)]
        public string TERMINATE_TIME
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
    }
}

