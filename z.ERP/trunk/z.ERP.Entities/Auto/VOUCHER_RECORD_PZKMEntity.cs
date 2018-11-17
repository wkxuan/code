using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("VOUCHER_RECORD_PZKM", "凭证分录凭证科目")]
    public partial class VOUCHER_RECORD_PZKMEntity: TableEntityBase
    {
        public VOUCHER_RECORD_PZKMEntity()
        {
        }

        public VOUCHER_RECORD_PZKMEntity(string voucherid, string recordid, string inx)
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
        [PrimaryKey]
        [Field("分录编号")]
        public string RECORDID
        {
            get; set;
        }
        /// <summary>
        /// 科目级次
        /// </summary>
        [PrimaryKey]
        [Field("科目级次")]
        public string INX
        {
            get; set;
        }
        /// <summary>
        /// 科目代码
        /// </summary>
        [Field("科目代码")]
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
