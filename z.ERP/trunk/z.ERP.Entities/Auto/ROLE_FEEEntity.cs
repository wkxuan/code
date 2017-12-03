/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 16:59:01
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("ROLE_FEE", "")]
    public partial class ROLE_FEEEntity : EntityBase
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
        /// 
        /// <summary>
        [PrimaryKey]
        public string ROLEID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string TRIMID
        {
            get; set;
        }
    }
}
