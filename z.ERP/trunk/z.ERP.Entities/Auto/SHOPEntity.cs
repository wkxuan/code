/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/19 20:30:52
 * 生成人：书房
 * 代码生成器版本号：1.2.6562.36915
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("SHOP", "店铺单元")]
    public partial class SHOPEntity : EntityBase
    {
        public SHOPEntity()
        {
        }

        public SHOPEntity(string shopid)
        {
            SHOPID = shopid;
        }

        /// <summary>
        /// 单元编号
        /// <summary>
        [PrimaryKey]
        [Field("单元编号")]
        public string SHOPID
        {
            get; set;
        }
        /// <summary>
        /// 单元代码
        /// <summary>
        [Field("单元代码")]
        public string CODE
        {
            get; set;
        }
        /// <summary>
        /// 单元名称
        /// <summary>
        [Field("单元名称")]
        public string NAME
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
        /// <summary>
        /// 楼层编号
        /// <summary>
        [Field("楼层编号")]
        public string FLOORID
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
        /// 业态
        /// <summary>
        [Field("业态")]
        public string CATEGORYID
        {
            get; set;
        }
        /// <summary>
        /// 单元类型
        /// <summary>
        [Field("单元类型")]
        public string TYPE
        {
            get; set;
        }
        /// <summary>
        /// 租用状态
        /// <summary>
        [Field("租用状态")]
        public string RENT_STATUS
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
    }
}
