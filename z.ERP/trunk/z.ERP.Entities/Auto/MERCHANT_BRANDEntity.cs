/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 0:49:27
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("MERCHANT_BRAND", "")]
    public partial class MERCHANT_BRANDEntity : EntityBase
    {
        public MERCHANT_BRANDEntity()
        {
        }

        public MERCHANT_BRANDEntity(string merchantid, string brandid)
        {
            MERCHANTID = merchantid;
            BRANDID = brandid;
        }

        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string MERCHANTID
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
    }
}
