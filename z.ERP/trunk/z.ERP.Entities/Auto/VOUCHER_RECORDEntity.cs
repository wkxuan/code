/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:12
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("VOUCHER_RECORD", "凭证分录")]
    public partial class VOUCHER_RECORDEntity : TableEntityBase
    {
        public VOUCHER_RECORDEntity()
        {
        }

        public VOUCHER_RECORDEntity(string voucherid, string recordid)
        {
            VOUCHERID = voucherid;
            RECORDID = recordid;
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
        /// 分录编号
        /// <summary>
        [PrimaryKey]
        [Field("分录编号")]
        public string RECORDID
        {
            get; set;
        }
        /// <summary>
        /// SQL语句
        /// </summary>
        [Field("分录名称")]
        public string RECORDNAME
        {
            get; set;
        }
        /// <summary>
        /// SQL编号
        /// </summary>
        [Field("SQL编号")]
        public string SQLINX
        {
            get; set;
        }
        [Field("类型")]
        public string TYPE
        {
            get; set;
        }
        /// <summary>
        /// 借方贷方金额关键字
        /// </summary>
        [Field("借方贷方金额关键字")]
        public string SQLCOLTORECORD
        {
            get; set;
        }
        /// <summary>
        /// 商户关键字
        /// </summary>
        [Field("商户关键字")]
        public string SQLCOLTOMERCHANT
        {
            get; set;
        }
        /// <summary>
        /// 用户关键字
        /// </summary>
        [Field("用户关键字")]
        public string SQLCOLTOUSER
        {
            get; set;
        }
        /// <summary>
        /// 部门关键字
        /// </summary>
        [Field("部门关键字")]
        public string SQLCOLTOORG
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        [Field("ACCOUNTPROJECT")]
        public string ACCOUNTPROJECT
        {
            get; set;
        }
    }
}
