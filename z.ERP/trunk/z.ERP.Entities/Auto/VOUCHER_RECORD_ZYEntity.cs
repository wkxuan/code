using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities.Auto
{
    [DbTable("VOUCHER_RECORD_ZY", "凭证分录摘要")]
    public partial class VOUCHER_RECORD_ZYEntity: TableEntityBase
    {
        public VOUCHER_RECORD_ZYEntity()
        {
        }

        public VOUCHER_RECORD_ZYEntity(string voucherid, string recordid,string inx)
        {
            VOUCHERID = voucherid;
            RECORDID = recordid;
            INX = inx;
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
        [Field("分录编号")]
        public string RECORDID
        {
            get; set;
        }
        /// <summary>
        /// 摘要级次
        /// </summary>
        [Field("摘要级次")]
        public string INX
        {
            get; set;
        }
        /// <summary>
        /// 摘要文本
        /// </summary>
        [Field("摘要文本")]
        public string DESCRIPTION
        {
            get; set;
        }
        /// <summary>
        /// 取值字段
        /// </summary>
        [Field("取值字段")]
        public string SQLCOLTORECORD
        {
            get; set;
        }
        /// <summary>
        /// 是否使用分录sql   1 使用，0 不使用
        /// </summary>
        [Field("是否使用分录sql")]
        public string SQLBJ
        {
            get; set;
        }
    }
}
