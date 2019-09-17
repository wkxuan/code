using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("PROMOTION", "促销活动主题")]
    public partial class PROMOTIONEntity : TableEntityBase
    {
        public PROMOTIONEntity()
        {
        }

        public PROMOTIONEntity(string id)
        {
            ID = id;
        }
        /// <summary>
        /// 活动id
        /// <summary>
        [PrimaryKey]
        [Field("活动id")]
        public string ID
        {
            get; set;
        }
        /// <summary>
        /// 年度
        /// <summary>
        [Field("年度")]
        public string YEAR
        {
            get; set;
        }
        /// <summary>
        /// 主题名称
        /// <summary>
        [Field("主题名称")]
        public string NAME
        {
            get; set;
        }
        /// <summary>
        /// 内容
        /// <summary>
        [Field("内容")]
        public string CONTENT
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
        /// 状态
        /// <summary>
        [Field("状态")]
        public string STATUS
        {
            get; set;
        }
    }
}

