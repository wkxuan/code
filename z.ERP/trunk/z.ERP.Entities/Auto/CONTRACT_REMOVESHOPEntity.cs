/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-19 0:12:01
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6560.42822
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

        /// <summary>
        /// 
        /// <summary>
        public string BILLID
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
    }
}
