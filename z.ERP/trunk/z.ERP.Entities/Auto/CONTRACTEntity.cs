/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 0:49:23
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("CONTRACT", "")]
    public partial class CONTRACTEntity : EntityBase
    {
        public CONTRACTEntity()
        {
        }

        public CONTRACTEntity(string contractid)
        {
            CONTRACTID = contractid;
        }

        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string CONTRACTID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string CONTRACT_OLD
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string STYLE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string HTLX
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string CONTRACTID_PAPER
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string ORGID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string CONT_START
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string CONT_END
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string QZRQ
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string AREAR
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string AREA_BUILD
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string JXSL
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string XXSL
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string BRANCHID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string MERCHANTID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string FEERULE_RENT
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string FEERULE_WYF
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string ZNID_RENT
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string ZNID_WYF
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string OPERATERULE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string STANDARD
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string DESCRIPTION
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string REPORTER
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string REPORTER_NAME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string REPORTER_TIME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string VERIFY
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string VERIFY_NAME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string VERIFY_TIME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string APPROVE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string APPROVE_NAME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string APPROVE_TIME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string INITINATE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string INITINATE_NAME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string INITINATE_TIME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string TERMINATE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string TERMINATE_NAME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string TERMINATE_TIME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string STATUS
        {
            get; set;
        }
    }
}
