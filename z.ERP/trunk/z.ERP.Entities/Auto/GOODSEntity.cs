/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 16:58:58
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("GOODS", "")]
    public partial class GOODSEntity : EntityBase
    {
        public GOODSEntity()
        {
        }

        public GOODSEntity(string goodsid)
        {
            GOODSID = goodsid;
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
        public string GOODSDM
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string BARCODE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string PYM
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string NAME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string CONTRACTID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string MERCHANTID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string KINDID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string TYPE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string PRICE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string MEMBER_PRICE
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
        public string XXSL
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string STYLE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string REGION
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string STATUS
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string REPORTER
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string REPORTER_NAME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string REPORTER_TIME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string VERIFY
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string VERIFY_NAME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string VERIFY_TIME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string DESCRIPTION
        {
            get; set;
        }
    }
}
