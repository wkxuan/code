/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:18
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("ROLE_FEE", "角色-收费项目权限")]
    public partial class ROLE_FEEEntity : TableEntityBase
    {
        public ROLE_FEEEntity()
        {
        }

        public ROLE_FEEEntity(string roleid, string trimid)
        {
            ROLEID = roleid;
            TRIMID = trimid;
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
        /// <summary>
        /// 费用项目ID
        /// <summary>
        [PrimaryKey]
        [Field("费用项目ID")]
        public string TRIMID
        {
            get; set;
        }
    }
}
