
using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("SPLCDEFD", "审批流程定义")]
    public partial class SPLCDEFDEntity : TableEntityBase
    {
        public SPLCDEFDEntity()
        {
        }

        public SPLCDEFDEntity(string billid)
        {
            BILLID = billid;
        }

        /// <summary>
        /// 代码
        /// <summary>
        [PrimaryKey]
        [Field("单号")]
        public string BILLID
        {
            get; set;
        }
        [PrimaryKey]
        [Field("菜单")]
        public string MENUID
        {
            get; set;
        }

        [Field("录入员")]
        public string REPORTER
        {
            get; set;
        }
        /// <summary>
        /// 录入员名称
        /// <summary>
        [Field("录入员名称")]
        public string REPORTER_NAME
        {
            get; set;
        }
        /// <summary>
        /// 录入时间
        /// <summary>
        [Field("录入时间")]
        [DbType(DbType.DateTime)]
        public string REPORTER_TIME
        {
            get; set;
        }
        /// <summary>
        /// 确认人
        /// <summary>
        [Field("确认人")]
        public string VERIFY
        {
            get; set;
        }
        /// <summary>
        /// 确认人名称
        /// <summary>
        [Field("确认人名称")]
        public string VERIFY_NAME
        {
            get; set;
        }
        /// <summary>
        /// 确认时间
        /// <summary>
        [Field("确认时间")]
        [DbType(DbType.DateTime)]
        public string VERIFY_TIME
        {
            get; set;
        }
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
