/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:15
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */

using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("WL_GOODS", "物料")]
    public partial class WL_GOODSEntity : TableEntityBase
    {
        public WL_GOODSEntity()
        {
        }

        public WL_GOODSEntity(string goodsid)
        {
            GOODSID = goodsid;
        }


        [PrimaryKey]
        [Field("内码")]
        public string GOODSID
        {
            get; set;
        }

        [Field("代码")]
        public string GOODSDM
        {
            get; set;
        }

        [Field("名称")]
        public string NAME
        {
            get; set;
        }

        [Field("拼音码")]
        public string PYM
        {
            get; set;
        }

        [Field("物料供货商代码")]
        public string MERCHANTID
        {
            get; set;
        }

        [Field("含税进价")]
        public string TAXINPRICE
        {
            get; set;
        }

        [Field("不含税进价")]
        public string NOTAXINPRICE
        {
            get; set;
        }

        [Field("使用价")]
        public string USEPRICE
        {
            get; set;
        }

        [Field("进价税率")]
        public string JXSL
        {
            get; set;
        }

        [Field("状态")]
        public string STATUS
        {
            get; set;
        }
        /// <summary>
        /// 登记人
        /// <summary>
        [Field("登记人")]
        public string REPORTER
        {
            get; set;
        }
        /// <summary>
        /// 登记人名称
        /// <summary>
        [Field("登记人名称")]
        public string REPORTER_NAME
        {
            get; set;
        }
        /// <summary>
        /// 登记时间
        /// <summary>
        [Field("登记时间")]
        [DbType(DbType.DateTime)]
        public string REPORTER_TIME
        {
            get; set;
        }
        /// <summary>
        /// 确认人
        /// <summary>
        [Field("确认人")]
        public string VERIFY
        {
            get; set;
        }
        /// <summary>
        /// 确认人名称
        /// <summary>
        [Field("确认人名称")]
        public string VERIFY_NAME
        {
            get; set;
        }
        /// <summary>
        /// 确认时间
        /// <summary>
        [Field("确认时间")]
        [DbType(DbType.DateTime)]
        public string VERIFY_TIME
        {
            get; set;
        }
    }
}
