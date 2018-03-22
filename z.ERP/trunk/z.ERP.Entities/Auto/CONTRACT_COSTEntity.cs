/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:08
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("CONTRACT_COST", "合同收费项目")]
    public partial class CONTRACT_COSTEntity : TableEntityBase
    {
        public CONTRACT_COSTEntity()
        {
        }

        public CONTRACT_COSTEntity(string contractid, string inx, string termid, string startdate)
        {
            CONTRACTID = contractid;
            INX = inx;
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
        /// 序号
        /// <summary>
        [PrimaryKey]
        [Field("序号")]
        public string INX
        {
            get; set;
        }
        /// <summary>
        /// 项目ID
        /// <summary>
        [PrimaryKey]
        [Field("项目ID")]
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
        /// 收费方式
        /// <summary>
        [Field("收费方式")]
        public string SFFS
        {
            get; set;
        }
        /// <summary>
        /// 单价
        /// <summary>
        [Field("单价")]
        public string PRICE
        {
            get; set;
        }
        /// <summary>
        /// 费用
        /// <summary>
        [Field("费用")]
        public string COST
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
        /// 收费规则
        /// <summary>
        [Field("收费规则")]
        public string FEERULEID
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
