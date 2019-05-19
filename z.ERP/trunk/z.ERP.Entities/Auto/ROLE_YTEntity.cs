using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("ROLE_YT", "角色-业态权限")]
    public partial class ROLE_YTEntity : TableEntityBase
    {
        public ROLE_YTEntity()
        {
        }

        public ROLE_YTEntity(string roleid, string ytid)
        {
            ROLEID = roleid;
            YTID = ytid;
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
        /// 业态ID
        /// <summary>
        [PrimaryKey]
        [Field("业态ID")]
        public string YTID
        {
            get; set;
        }
    }
}
