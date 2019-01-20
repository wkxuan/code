using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Auto
{
    [DbTable("OPENBUSINESSITEM", "店铺开业单明细")]
    public partial class OPENBUSINESSITEMEntity : TableEntityBase
    {
        public OPENBUSINESSITEMEntity()
        {
        }

        public OPENBUSINESSITEMEntity(string billid, string shopid)
        {
            BILLID = billid;
            SHOPID = shopid;
        }
        /// <summary>
        /// 单号
        /// <summary>
        [PrimaryKey]
        [Field("单号")]
        public string BILLID
        {
            get; set;
        }
        /// <summary>
        /// 商铺ID
        /// <summary>
        [PrimaryKey]
        [Field("商铺ID")]
        public string SHOPID
        {
            get; set;
        }
    }
}


