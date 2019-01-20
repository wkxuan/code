/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:19
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("SYSUSER", "用户")]
    public partial class SYSUSEREntity : TableEntityBase
    {
        public SYSUSEREntity()
        {
        }

        public SYSUSEREntity(string userid)
        {
            USERID = userid;
        }

        /// <summary>
        /// 用户编码
        /// <summary>
        [PrimaryKey]
        [Field("用户编码")]
        public string USERID
        {
            get; set;
        }
        /// <summary>
        /// 用户代码
        /// <summary>
        [Field("用户代码")]
        public string USERCODE
        {
            get; set;
        }
        /// <summary>
        /// 用户名称
        /// <summary>
        [Field("用户名称")]
        public string USERNAME
        {
            get; set;
        }
        /// <summary>
        /// 用户类型
        /// <summary>
        [Field("用户类型")]
        public string USER_TYPE
        {
            get; set;
        }
        /// <summary>
        /// 用户机构
        /// <summary>
        [Field("用户机构")]
        public string ORGID
        {
            get; set;
        }
        /// <summary>
        /// 用户标记
        /// <summary>
        [Field("用户标记")]
        public string USER_FLAG
        {
            get; set;
        }
        /// <summary>
        /// 作废标记
        /// <summary>
        [Field("作废标记")]
        public string VOID_FLAG
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string PASSWORD
        {
            get; set;
        }

        public string SHOPID
        {
            get; set;
        }
    }
}
