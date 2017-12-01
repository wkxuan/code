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
    [DbTable("CONTRACT_REMOVESHOP", "")]
    public partial class CONTRACT_REMOVESHOPEntity : EntityBase
    {
        public CONTRACT_REMOVESHOPEntity()
        {
        }

        public CONTRACT_REMOVESHOPEntity(string billid, string shopid)
        {
            BILLID = billid;
            SHOPID = shopid;
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
        public string SHOPID
        {
            get; set;
        }
    }
}
