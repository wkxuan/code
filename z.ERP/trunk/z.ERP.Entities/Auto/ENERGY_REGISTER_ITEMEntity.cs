/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/5 22:24:32
 * 生成人：书房
 * 代码生成器版本号：1.2.6562.36915
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("ENERGY_REGISTER_ITEM", "能源设备登记子表(抄表)")]
    public partial class ENERGY_REGISTER_ITEMEntity : EntityBase
    {
        public ENERGY_REGISTER_ITEMEntity()
        {
        }

        public ENERGY_REGISTER_ITEMEntity(string billid, string fileid)
        {
            BILLID = billid;
            FILEID = fileid;
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
        /// 能源设备ID
        /// <summary>
        [PrimaryKey]
        [Field("能源设备ID")]
        public string FILEID
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
        /// <summary>
        /// 店铺ID
        /// <summary>
        [Field("店铺ID")]
        public string SHOPID
        {
            get; set;
        }
        /// <summary>
        /// 租约号
        /// <summary>
        [Field("租约号")]
        public string CONTRACTID
        {
            get; set;
        }
        /// <summary>
        /// 开始日期
        /// <summary>
        [Field("开始日期")]
        [DbType(DbType.DateTime)]
        public string START_DATE
        {
            get; set;
        }
        /// <summary>
        /// 结束日期
        /// <summary>
        [Field("结束日期")]
        [DbType(DbType.DateTime)]
        public string END_DATE
        {
            get; set;
        }
        /// <summary>
        /// 上次读数
        /// <summary>
        [Field("上次读数")]
        public string VALUE_LAST
        {
            get; set;
        }
        /// <summary>
        /// 当前读数
        /// <summary>
        [Field("当前读数")]
        public string VALUE_CURRENT
        {
            get; set;
        }
        /// <summary>
        /// 使用量
        /// <summary>
        [Field("使用量")]
        public string VALUE_USE
        {
            get; set;
        }
        /// <summary>
        /// 单价
        /// <summary>
        [Field("单价")]
        public string PRICE
        {
            get; set;
        }
        /// <summary>
        /// 金额
        /// <summary>
        [Field("金额")]
        public string AMOUNT
        {
            get; set;
        }
    }
}
