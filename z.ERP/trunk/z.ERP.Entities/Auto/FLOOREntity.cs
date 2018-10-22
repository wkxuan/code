/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:13
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("FLOOR", "楼层")]
    public partial class FLOOREntity : TableEntityBase
    {
        public FLOOREntity()
        {
        }

        public FLOOREntity(string id)
        {
            ID = id;
        }

        /// <summary>
        /// 楼层编号
        /// <summary>
        [PrimaryKey]
        [Field("楼层编号")]
        public string ID
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

        /// <summary>
        /// 楼层代码
        /// <summary>
        [Field("楼层代码")]
        public string CODE
        {
            get; set;
        }
        /// <summary>
        /// 楼层名称
        /// <summary>
        [Field("楼层名称")]
        public string NAME
        {
            get; set;
        }
        /// <summary>
        /// 管理部门
        /// <summary>
        [Field("管理部门")]
        public string ORGID
        {
            get; set;
        }
        /// <summary>
        /// 建筑面积
        /// <summary>
        [Field("建筑面积")]
        public string AREA_BUILD
        {
            get; set;
        }
        /// <summary>
        /// 可使用面积
        /// <summary>
        [Field("可使用面积")]
        public string AREA_USABLE
        {
            get; set;
        }
        /// <summary>
        /// 可租赁面积
        /// <summary>
        [Field("可租赁面积")]
        public string AREA_RENTABLE
        {
            get; set;
        }
        /// <summary>
        /// 状态
        /// <summary>
        [Field("状态")]
        public string STATUS
        {
            get; set;
        }
    }
}
