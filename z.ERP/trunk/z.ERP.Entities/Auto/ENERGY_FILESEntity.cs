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
    [DbTable("ENERGY_FILES", "能源档案")]
    public partial class ENERGY_FILESEntity : EntityBase
    {
        public ENERGY_FILESEntity()
        {
        }

        public ENERGY_FILESEntity(string fileid)
        {
            FILEID = fileid;
        }

        /// <summary>
        /// 能源档案ID
        /// <summary>
        [PrimaryKey]
        [Field("能源档案ID")]
        public string FILEID
        {
            get; set;
        }
        /// <summary>
        /// 能源档案代码
        /// <summary>
        [Field("能源档案代码")]
        public string FILECODE
        {
            get; set;
        }
        /// <summary>
        /// 能源档案名称
        /// <summary>
        [Field("能源档案名称")]
        public string FILENAME
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
        /// 区域id
        /// <summary>
        [Field("区域id")]
        public string AREAID
        {
            get; set;
        }
        /// <summary>
        /// 出厂日期
        /// <summary>
        [Field("出厂日期")]
        [DbType(DbType.DateTime)]
        public string FACTORY_DATE
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
        /// 最后读数
        /// <summary>
        [Field("最后读数")]
        public string VALUE_LAST
        {
            get; set;
        }
        /// <summary>
        /// 最后读数日期
        /// <summary>
        [Field("最后读数日期")]
        [DbType(DbType.DateTime)]
        public string DATE_LAST
        {
            get; set;
        }
        /// <summary>
        /// 倍率
        /// <summary>
        [Field("倍率")]
        public string MULTIPLE
        {
            get; set;
        }
        /// <summary>
        /// 录入人
        /// <summary>
        [Field("录入人")]
        public string REPORTER
        {
            get; set;
        }
        /// <summary>
        /// 录入人名称
        /// <summary>
        [Field("录入人名称")]
        public string REPORTER_NAME
        {
            get; set;
        }
        /// <summary>
        /// 录入时间
        /// <summary>
        [Field("录入时间")]
        [DbType(DbType.DateTime)]
        public string REPORTER_TIME
        {
            get; set;
        }
        /// <summary>
        /// 描述
        /// <summary>
        [Field("描述")]
        public string DESCRIPTION
        {
            get; set;
        }
    }
}
