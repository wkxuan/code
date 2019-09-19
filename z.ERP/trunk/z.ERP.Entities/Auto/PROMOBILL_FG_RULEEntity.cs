using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("PROMOBILL_FG_RULE", "促销单赠品子表")]
    public partial class PROMOBILL_FG_RULEEntity
    {
        public PROMOBILL_FG_RULEEntity() { }
        public PROMOBILL_FG_RULEEntity(string id,string presentid) {
            BILLID = id;
            PRESENTID = presentid;
        }
        /// <summary>
        /// 促销单id
        /// <summary>
        [PrimaryKey]
        [Field("促销单id")]
        public string BILLID
        {
            get; set;
        }
        /// <summary>
        /// 序号
        /// <summary>
        [Field("序号")]
        public string INX
        {
            get; set;
        }
        [Field("满额")]
        public string FULL
        {
            get; set;
        }
        /// <summary>
        /// 值1 折扣率
        /// <summary>
        [PrimaryKey]
        [Field("赠品id")]
        public string PRESENTID
        {
            get; set;
        }
    }
}
