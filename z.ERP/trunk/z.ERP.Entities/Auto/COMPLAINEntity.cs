using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Auto
{
    [DbTable("COMPLAIN", "店铺投诉")]
    public partial class COMPLAINEntity : TableEntityBase
    {
        public COMPLAINEntity()
        {
        }
        public COMPLAINEntity(string billid)
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
        /// 分店ID
        /// <summary>
        [Field("分店ID")]
        public string BRANCHID
        {
            get; set;
        }
        /// <summary>
        /// 被投诉部门
        /// <summary>
        [Field("被投诉部门")]
        public string DEPTID
        {
            get; set;
        }
        /// <summary>
        /// 被投诉店铺
        /// <summary>
        [Field("被投诉店铺")]
        public string SHOPID
        {
            get; set;
        }
        /// <summary>
        /// 被投诉品牌
        /// <summary>
        [Field("被投诉品牌")]
        public string BRANDID
        {
            get; set;
        }
        /// <summary>
        /// 被投诉人
        /// <summary>
        [Field("被投诉人")]
        public string PERSON_NAME
        {
            get; set;
        }
        /// <summary>
        /// 投诉处理部门
        /// <summary>
        [Field("投诉处理部门")]
        public string CPLAINDEPT
        {
            get; set;
        }
        /// <summary>
        /// 投诉类型
        /// <summary>
        [Field("投诉类型")]
        public string CPLAINTYPE
        {
            get; set;
        }
        /// <summary>
        /// 投诉日期
        /// <summary>
        [Field("投诉日期")]
        [DbType(DbType.DateTime)]
        public string CPLAINDATE
        {
            get; set;
        }
        /// <summary>
        /// 投诉人
        /// <summary>
        [Field("投诉人")]
        public string CPLAINPERSON_NAME
        {
            get; set;
        }
        /// <summary>
        /// 投诉人电话
        /// <summary>
        [Field("投诉人电话")]
        public string CPLAINPHONE
        {
            get; set;
        }
        /// <summary>
        /// 投诉内容
        /// <summary>
        [Field("投诉内容")]
        public string CPLAINTEXT
        {
            get; set;
        }
        /// <summary>
        /// 处理结果
        /// <summary>
        [Field("处理结果")]
        public string DISPOSERESULT
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
        /// 审核时间
        /// <summary>
        [Field("审核时间")]
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
    }
}
