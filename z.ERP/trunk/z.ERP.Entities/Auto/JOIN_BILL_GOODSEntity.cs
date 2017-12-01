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
    [DbTable("JOIN_BILL_GOODS", "")]
    public partial class JOIN_BILL_GOODSEntity : EntityBase
    {
        public JOIN_BILL_GOODSEntity()
        {
        }

        public JOIN_BILL_GOODSEntity(string billid, string goodsid, string drate)
        {
            BILLID = billid;
            GOODSID = goodsid;
            DRATE = drate;
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
