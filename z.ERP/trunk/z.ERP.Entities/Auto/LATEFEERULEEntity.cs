/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:14
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("LATEFEERULE", "滞纳金规则")]
    public partial class LATEFEERULEEntity : TableEntityBase
    {
        public LATEFEERULEEntity()
        {
        }

        public LATEFEERULEEntity(string id)
        {
            ID = id;
        }

        /// <summary>
        /// 收费规则编号
        /// <summary>
        [PrimaryKey]
        [Field("收费规则编号")]
        public string ID
        {
            get; set;
        }
        /// <summary>
        /// 收费规则名称
        /// <summary>
        [Field("收费规则名称")]
        public string NAME
        {
            get; set;
        }
        /// <summary>
        /// 宽限天数
        /// <summary>
        [Field("宽限天数")]
        public string DAYS
        {
            get; set;
        }
        /// <summary>
        /// 宽限金额
        /// <summary>
        [Field("宽限金额")]
        public string AMOUNTS
        {
            get; set;
        }

        [Field("滞纳金比例")]
        public string RATIO
        {
            get; set;
        }
    }
}
