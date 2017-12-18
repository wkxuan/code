/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-18 23:51:17
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6537.1447
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("JOIN_BILL_GOODS", "")]
    public partial class JOIN_BILL_GOODSEntity : EntityBase
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
        public string GOODSID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string DRATE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string JXSL
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string SELL_SL
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string SELL_JE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string YHJE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string SELL_COST
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string ZZSJE
        {
            get; set;
        }
    }
}
