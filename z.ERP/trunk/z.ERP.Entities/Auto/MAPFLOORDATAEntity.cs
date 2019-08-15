using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Auto
{
    [DbTable("MAPFLOORDATA", "map楼层数据")]
    public class MAPFLOORDATAEntity: TableEntityBase
    {
        public MAPFLOORDATAEntity()
        {
        }

        public MAPFLOORDATAEntity(string floorid, string branchid,string regionid)
        {
            FLOORID = floorid;
            BRANCHID = branchid;
            REGIONID = regionid;
        }
        /// <summary>
        /// 楼层编号
        /// <summary>
        [PrimaryKey]
        [Field("楼层编号")]
        public string FLOORID
        {
            get; set;
        }
        /// <summary>
        /// 门店编号
        /// <summary>
        [Field("门店编号")]
        public string BRANCHID
        {
            get; set;
        }

        [Field("区域编号")]
        public string REGIONID
        {
            get; set;
        }
        [Field("定位坐标")]
        public string POINTS
        {
            get; set;
        }
    }
}
