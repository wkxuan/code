using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Auto
{
    [DbTable("ROLE_BRANCH", "角色-门店")]
    public partial class ROLE_BRANCHEntity : TableEntityBase
    {
        public ROLE_BRANCHEntity(){}
        public ROLE_BRANCHEntity(string roleid, string branchid) {
            ROLEID = roleid;
            BRANCHID = branchid;
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
        /// 分店ID
        /// <summary>
        [PrimaryKey]
        [Field("分店ID")]
        public string BRANCHID
        {
            get; set;
        }
    }
}
