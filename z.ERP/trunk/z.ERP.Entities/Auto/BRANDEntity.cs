﻿/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:07
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("BRAND", "品牌")]
    public partial class BRANDEntity : TableEntityBase
    {
        public BRANDEntity()
        {
        }

        public BRANDEntity(string id)
        {
            ID = id;
        }

        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string ID
        {
            get; set;
        }
        /// <summary>
        /// 名称
        /// <summary>
        [Field("名称")]
        public string NAME
        {
            get; set;
        }
        /// <summary>
        /// 业态
        /// <summary>
        [Field("业态")]
        public string CATEGORYID
        {
            get; set;
        }
        /// <summary>
        /// 地址
        /// <summary>
        [Field("地址")]
        public string ADRESS
        {
            get; set;
        }
        /// <summary>
        /// 联系人
        /// <summary>
        [Field("联系人")]
        public string CONTACTPERSON
        {
            get; set;
        }
        /// <summary>
        /// 电话
        /// <summary>
        [Field("电话")]
        public string PHONENUM
        {
            get; set;
        }
        /// <summary>
        /// 邮编
        /// <summary>
        [Field("邮编")]
        public string PIZ
        {
            get; set;
        }
        /// <summary>
        /// 微信
        /// <summary>
        [Field("微信")]
        public string WEIXIN
        {
            get; set;
        }
        /// <summary>
        /// QQ
        /// <summary>
        [Field("QQ")]
        public string QQ
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
        /// 描述
        /// <summary>
        [Field("描述")]
        public string DESCRIPTION
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
