/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:12
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("VOUCHER_MAKESQL", "凭证SQL")]
    public partial class VOUCHER_MAKESQLEntity : TableEntityBase
    {
        public VOUCHER_MAKESQLEntity()
        {
        }

        public VOUCHER_MAKESQLEntity(string voucherid, string sqlinx)
        {
            VOUCHERID = voucherid;
            SQLINX = sqlinx;
        }

        /// <summary>
        /// 凭证号
        /// <summary>
        [PrimaryKey]
        [Field("凭证号")]
        public string VOUCHERID
        {
            get; set;
        }
        /// <summary>
        /// SQLINX
        /// <summary>
        [PrimaryKey]
        [Field("SQLINX")]
        public string SQLINX
        {
            get; set;
        }
        /// <summary>
        /// SQL语句
        /// </summary>
        [Field("SQL语句")]
        public string MAKESQL
        {
            get; set;
        }
        /// <summary>
        /// 执行类型
        /// </summary>
        [Field("执行类型")]
        public string EXESQLTYPE
        {
            get; set;
        }
    }
}
