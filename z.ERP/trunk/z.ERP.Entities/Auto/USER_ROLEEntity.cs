/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-19 0:12:04
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6560.42822
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("USER_ROLE", "用户_角色")]
    public partial class USER_ROLEEntity : EntityBase
    {
        public USER_ROLEEntity()
        {
        }

        /// <summary>
        /// 用户编码
        /// <summary>
        [Field("用户编码")]
        public string USERID
        {
            get; set;
        }
        /// <summary>
        /// 角色编码
        /// <summary>
        [Field("角色编码")]
        public string ROLEID
        {
            get; set;
        }
    }
}
