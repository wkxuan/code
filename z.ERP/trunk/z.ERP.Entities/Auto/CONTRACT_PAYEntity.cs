/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/11 22:18:58
 * 生成人：书房
 * 代码生成器版本号：1.2.6562.36915
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("CONTRACT_PAY", "合同收款方式")]
    public partial class CONTRACT_PAYEntity : EntityBase
    {
        public CONTRACT_PAYEntity()
        {
        }

        public CONTRACT_PAYEntity(string contractid, string payid, string termid, string startdate)
        {
            CONTRACTID = contractid;
            PAYID = payid;
            TERMID = termid;
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
        /// 收款方式
        /// <summary>
        [PrimaryKey]
        [Field("收款方式")]
        public string PAYID
        {
            get; set;
        }
        /// <summary>
        /// 收费项目
        /// <summary>
        [PrimaryKey]
        [Field("收费项目")]
        public string TERMID
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
        /// 扣款比例
        /// <summary>
        [Field("扣款比例")]
        public string KL
        {
            get; set;
        }
        /// <summary>
        /// 滞纳规则
        /// <summary>
        [Field("滞纳规则")]
        public string ZNGZID
        {
            get; set;
        }
    }
}
