/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/5 22:24:30
 * 生成人：书房
 * 代码生成器版本号：1.2.6562.36915
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
