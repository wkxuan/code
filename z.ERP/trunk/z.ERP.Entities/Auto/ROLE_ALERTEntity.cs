using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Auto
{
    [DbTable("ROLE_ALERT", "角色-预警")]
    public partial class ROLE_ALERTEntity: TableEntityBase
    {

        public ROLE_ALERTEntity() { }
        public ROLE_ALERTEntity(string roleid, string alertid)
        {
            ROLEID = roleid;
            ALERTID = alertid;
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
        /// 预警ID
        /// <summary>
        [PrimaryKey]
        [Field("预警ID")]
        public string ALERTID
        {
            get; set;
        }
    }
}
