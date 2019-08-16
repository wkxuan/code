using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Auto
{
    [DbTable("MAPSHOPDATA", "mapSHOP数据")]
    public class MAPSHOPDATAEntity: TableEntityBase
    {
        public MAPSHOPDATAEntity()
        {
        }

        public MAPSHOPDATAEntity(string shopid)
        {
            SHOPID = shopid;
        }
        /// <summary>
        /// 单元
        /// <summary>
        [PrimaryKey]
        [Field("楼层编号")]
        public string SHOPID
        {
            get; set;
        }
        [Field("定位坐标")]
        public string POINTS
        {
            get; set;
        }
        [Field("title定位坐标")]
        public string TITLEPOINTS
        {
            get; set;
        }
    }
}
