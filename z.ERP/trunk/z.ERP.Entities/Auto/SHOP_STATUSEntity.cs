/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 16:59:02
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("SHOP_STATUS", "")]
    public partial class SHOP_STATUSEntity : EntityBase
    {
        public SHOP_STATUSEntity()
        {
        }

        public SHOP_STATUSEntity(string shopid, string start_date)
        {
            SHOPID = shopid;
            START_DATE = start_date;
        }

        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string SHOPID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        [DbType(DbType.DateTime)]
        public string START_DATE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string END_DATE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string STATUS
        {
            get; set;
        }
    }
}
