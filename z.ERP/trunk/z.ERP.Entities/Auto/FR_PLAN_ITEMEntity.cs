using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("FR_PLAN_ITEM", "满减方案子表")]
    public partial class FR_PLAN_ITEMEntity: TableEntityBase
    {
        public FR_PLAN_ITEMEntity(){}
        public FR_PLAN_ITEMEntity(string id,string inx) {
            ID = id;
            INX = inx;
        }
        /// <summary>
        /// 规则id
        /// <summary>
        [PrimaryKey]
        [Field("规则id")]
        public string ID
        {
            get; set;
        }
        /// <summary>
        /// 序号
        /// <summary>
        [PrimaryKey]
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
        [Field("减额")]
        public string CUT
        {
            get; set;
        }
    }
}
