/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 0:49:23
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("CONTRACT_BRAND", "")]
    public partial class CONTRACT_BRANDEntity : EntityBase
    {
        public CONTRACT_BRANDEntity()
        {
        }

        public CONTRACT_BRANDEntity(string contractid, string brandid)
        {
            CONTRACTID = contractid;
            BRANDID = brandid;
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
        public string BRANDID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string FTBL
        {
            get; set;
        }
    }
}
