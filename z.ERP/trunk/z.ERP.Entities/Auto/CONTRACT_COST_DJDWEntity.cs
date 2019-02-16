/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:08
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */

using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("CONTRACT_COST_DJDW", "合同收费项目")]
    public partial class CONTRACT_COST_DJDWEntity : TableEntityBase
    {
        public CONTRACT_COST_DJDWEntity()
        {
        }

        public CONTRACT_COST_DJDWEntity(string contractid, string tremid)
        {
            CONTRACTID = contractid;
            TREMID = tremid;
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
        /// 项目ID
        /// <summary>
        [PrimaryKey]
        [Field("项目ID")]
        public string TREMID
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
    }
}
