/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 16:58:56
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("ENERGY_REGISTER_ITEM", "")]
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
        /// 
        /// <summary>
        [PrimaryKey]
        public string BILLID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string FILEID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string INX
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string SHOPID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string CONTRACTID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
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
        public string VALUE_LAST
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string VALUE_CURRENT
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string VALUE_USE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string PRICE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string AMOUNT
        {
            get; set;
        }
    }
}
