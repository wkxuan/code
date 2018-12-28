using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    public partial class VOUCHEREntity
    {
        [ForeignKey(nameof(VOUCHERID), nameof(VOUCHER_MAKESQLEntity.VOUCHERID))]
        public List<VOUCHER_MAKESQLEntity> VOUCHER_MAKESQL
        {
            get;
            set;
        }

        [ForeignKey(nameof(VOUCHERID), nameof(VOUCHER_RECORDEntity.VOUCHERID))]
        public List<VOUCHER_RECORDEntity> VOUCHER_RECORD
        {
            get;
            set;
        }

        [ForeignKey(nameof(VOUCHERID), nameof(VOUCHER_RECORD_PZKMEntity.VOUCHERID))]
        public List<VOUCHER_RECORD_PZKMEntity> VOUCHER_RECORD_PZKM
        {
            get;
            set;
        }

        [ForeignKey(nameof(VOUCHERID), nameof(VOUCHER_RECORD_ZYEntity.VOUCHERID))]
        public List<VOUCHER_RECORD_ZYEntity> VOUCHER_RECORD_ZY
        {
            get;
            set;
        }


    }
    public partial class VOUCHER_RECORDEntity
    {
        [ForeignKey(nameof(VOUCHERID), nameof(VOUCHER_RECORD_PZKMEntity.VOUCHERID))]
        [ForeignKey(nameof(SQLINX), nameof(VOUCHER_RECORD_PZKMEntity.INX))]
        public List<VOUCHER_RECORD_PZKMEntity> VOUCHER_RECORD_PZKM
        {
            get;
            set;
        }
    }

    public partial class VOUCHER_RECORDEntity
    {
        [ForeignKey(nameof(VOUCHERID), nameof(VOUCHER_RECORD_ZYEntity.VOUCHERID))]
        [ForeignKey(nameof(SQLINX), nameof(VOUCHER_RECORD_ZYEntity.INX))]
        public List<VOUCHER_RECORD_ZYEntity> VOUCHER_RECORD_ZY
        {
            get;
            set;
        }
    }
}
