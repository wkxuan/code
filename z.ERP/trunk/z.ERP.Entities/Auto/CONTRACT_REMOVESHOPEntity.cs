/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:09
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("CONTRACT_REMOVESHOP", "")]
    public partial class CONTRACT_REMOVESHOPEntity : TableEntityBase
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
