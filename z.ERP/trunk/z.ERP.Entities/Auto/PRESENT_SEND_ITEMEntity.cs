using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("PRESENT_SEND_ITEM", "赠品发放明细")]
    public partial class PRESENT_SEND_ITEMEntity : TableEntityBase
    {
        public PRESENT_SEND_ITEMEntity(){}
        public PRESENT_SEND_ITEMEntity(string id,string presentid) {
            BILLID = id;
            PRESENTID = presentid;
        }
        /// <summary>
        /// id
        /// <summary>
        [PrimaryKey]
        [Field("id")]
        public string BILLID
        {
            get; set;
        }
        [PrimaryKey]
        [Field("赠品id")]
        public string PRESENTID
        {
            get; set;
        }
        [Field("发放数量")]
        public string COUNT
        {
            get; set;
        }
    }
}
