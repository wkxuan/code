using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("ROLE_REGION", "角色-区域权限")]
    public partial class ROLE_REGIONEntity : TableEntityBase
    {
        public ROLE_REGIONEntity()
        {
        }

        public ROLE_REGIONEntity(string roleid, string regionid)
        {
            ROLEID = roleid;
            REGIONID = regionid;
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
        /// 区域ID
        /// <summary>
        [PrimaryKey]
        [Field("区域ID")]
        public string REGIONID
        {
            get; set;
        }
    }
}
