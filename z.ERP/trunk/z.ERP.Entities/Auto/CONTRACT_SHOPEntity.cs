/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 0:49:25
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("CONTRACT_SHOP", "")]
    public partial class CONTRACT_SHOPEntity : EntityBase
    {
        public CONTRACT_SHOPEntity()
        {
        }

        public CONTRACT_SHOPEntity(string contractid, string shopid)
        {
            CONTRACTID = contractid;
            SHOPID = shopid;
        }

        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string CONTRACTID
        {
            get; set;
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
        public string CATEGORYID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string AREA
        {
            get; set;
        }
    }
}
