/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/5 22:24:40
 * 生成人：书房
 * 代码生成器版本号：1.2.6562.36915
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

        public USER_ROLEEntity(string userid, string roleid)
        {
            USERID = userid;
            ROLEID = roleid;
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
        /// 角色编码
        /// <summary>
        [PrimaryKey]
        [Field("角色编码")]
        public string ROLEID
        {
            get; set;
        }
    }
}
