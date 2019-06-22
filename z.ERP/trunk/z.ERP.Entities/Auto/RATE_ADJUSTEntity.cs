using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("RATE_ADJUST", "扣率调整单")]
    public partial class RATE_ADJUSTEntity:TableEntityBase
    {
        public RATE_ADJUSTEntity() {

        }
        public RATE_ADJUSTEntity(string id)
        {
            ID = id;
        }
        /// <summary>
        /// 主键id
        /// </summary>
        [PrimaryKey]
        [Field("ID")]
        public string ID
        {
            get; set;
        }
        /// <summary>
        /// 分店id
        /// </summary>
        [Field("分店id")]
        public string BRANCHID
        {
            get; set;
        }
        /// <summary>
        /// 调整开始时间
        /// </summary>
        [Field("开始时间")]
        [DbType(DbType.DateTime)]
        public string STARTTIME
        {
            get; set;
        }
        /// <summary>
        /// 调整结束时间
        /// </summary>
        [Field("结束时间")]
        [DbType(DbType.DateTime)]
        public string ENDTIME
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
        /// 描述
        /// <summary>
        [Field("描述")]
        public string DESCRIPTION
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
        /// 登记时间
        /// <summary>
        [Field("登记时间")]
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
    }
}
