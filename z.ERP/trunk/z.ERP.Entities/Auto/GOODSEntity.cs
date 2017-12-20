/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/19 20:30:48
 * 生成人：书房
 * 代码生成器版本号：1.2.6562.36915
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("GOODS", "商品")]
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
        /// 商品内码
        /// <summary>
        [PrimaryKey]
        [Field("商品内码")]
        public string GOODSID
        {
            get; set;
        }
        /// <summary>
        /// 商品代码
        /// <summary>
        [Field("商品代码")]
        public string GOODSDM
        {
            get; set;
        }
        /// <summary>
        /// 商品条码
        /// <summary>
        [Field("商品条码")]
        public string BARCODE
        {
            get; set;
        }
        /// <summary>
        /// 拼音码
        /// <summary>
        [Field("拼音码")]
        public string PYM
        {
            get; set;
        }
        /// <summary>
        /// 商品名称
        /// <summary>
        [Field("商品名称")]
        public string NAME
        {
            get; set;
        }
        /// <summary>
        /// 租约号
        /// <summary>
        [Field("租约号")]
        public string CONTRACTID
        {
            get; set;
        }
        /// <summary>
        /// 商户代码
        /// <summary>
        [Field("商户代码")]
        public string MERCHANTID
        {
            get; set;
        }
        /// <summary>
        /// 商品分类
        /// <summary>
        [Field("商品分类")]
        public string KINDID
        {
            get; set;
        }
        /// <summary>
        /// 类型
        /// <summary>
        [Field("类型")]
        public string TYPE
        {
            get; set;
        }
        /// <summary>
        /// 单价
        /// <summary>
        [Field("单价")]
        public string PRICE
        {
            get; set;
        }
        /// <summary>
        /// 会员单价
        /// <summary>
        [Field("会员单价")]
        public string MEMBER_PRICE
        {
            get; set;
        }
        /// <summary>
        /// 进项税率
        /// <summary>
        [Field("进项税率")]
        public string JXSL
        {
            get; set;
        }
        /// <summary>
        /// 销项税率
        /// <summary>
        [Field("销项税率")]
        public string XXSL
        {
            get; set;
        }
        /// <summary>
        /// 经营方式
        /// <summary>
        [Field("经营方式")]
        public string STYLE
        {
            get; set;
        }
        /// <summary>
        /// 地区
        /// <summary>
        [Field("地区")]
        public string REGION
        {
            get; set;
        }
        /// <summary>
        /// 状态
        /// <summary>
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
        /// 登记日期
        /// <summary>
        [Field("登记日期")]
        [DbType(DbType.DateTime)]
        public string REPORTER_TIME
        {
            get; set;
        }
        /// <summary>
        /// 审核人
        /// <summary>
        [Field("审核人")]
        public string VERIFY
        {
            get; set;
        }
        /// <summary>
        /// 审核人名称
        /// <summary>
        [Field("审核人名称")]
        public string VERIFY_NAME
        {
            get; set;
        }
        /// <summary>
        /// 审核日期
        /// <summary>
        [Field("审核日期")]
        [DbType(DbType.DateTime)]
        public string VERIFY_TIME
        {
            get; set;
        }
        /// <summary>
        /// 描述
        /// <summary>
        [Field("描述")]
        public string DESCRIPTION
        {
            get; set;
        }
    }
}
