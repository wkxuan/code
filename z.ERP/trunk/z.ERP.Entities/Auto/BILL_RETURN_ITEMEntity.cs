/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-18 23:51:14
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6537.1447
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("BILL_RETURN_ITEM", "")]
    public partial class BILL_RETURN_ITEMEntity : EntityBase
    {
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
        public string FINAL_BILLID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string RETURN_MONEY
        {
            get; set;
        }
    }
}
