using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("MAPPUBLICDATA", "公共设施数据")]
    public partial class MAPPUBLICDATAEntity : TableEntityBase
    {
        public MAPPUBLICDATAEntity() { }
        public MAPPUBLICDATAEntity(string id)
        {
            ID = id;
        }
        /// <summary>
        /// 代码
        /// <summary>
        [PrimaryKey]
        [Field("代码")]
        public string ID
        {
            get; set;
        }
        [Field("楼层id")]
        public string FLOORID
        {
            get; set;
        }
        [Field("设施类型id")]
        public string PUBLICDATAID
        {
            get; set;
        }
        [Field("定位坐标")]
        public string POINTS
        {
            get; set;
        }
        [Field("IMAGE定位坐标")]
        public string IMAGEPOINTS
        {
            get; set;
        }
    }
}
