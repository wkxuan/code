using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("ROLE_FLOOR", "角色-楼层权限")]
    public partial class ROLE_FLOOREntity : TableEntityBase
    {
        public ROLE_FLOOREntity()
        {
        }

        public ROLE_FLOOREntity(string roleid, string floorid)
        {
            ROLEID = roleid;
            FLOORID = floorid;
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
        [Field("楼层ID")]
        public string FLOORID
        {
            get; set;
        }
    }
}
