/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:08
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("FLOORSHOP", "楼层铺位")]
    public partial class FLOORSHOPEntity : TableEntityBase
    {
        public FLOORSHOPEntity()
        {
        }

        public FLOORSHOPEntity(string mapid, string shopcode)
        {
            MAPID = mapid;
            SHOPCODE = shopcode;
        }

        /// <summary>
        /// 图纸编号
        /// <summary>
        [PrimaryKey]
        [Field("图纸编号")]
        public string MAPID
        {
            get; set;
        }
        /// <summary>
        /// 铺位代码
        /// <summary>
        [PrimaryKey]
        [Field("铺位编号")]
        public string SHOPID
        {
            get; set;
        }
        /// <summary>
        /// 铺位代码
        /// <summary>
        [PrimaryKey]
        [Field("铺位代码")]
        public string SHOPCODE
        {
            get; set;
        }
        /// <summary>
        /// X坐标
        /// <summary>
        [Field("X坐标")]
        public string P_X
        {
            get; set;
        }
        /// <summary>
        /// Y坐标
        /// <summary>
        [Field("Y坐标")]
        public string P_Y
        {
            get; set;
        }
    }
}
