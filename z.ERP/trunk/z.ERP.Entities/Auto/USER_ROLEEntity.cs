/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 16:59:03
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("USER_ROLE", "")]
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
        /// 
        /// <summary>
        [PrimaryKey]
        public string USERID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string ROLEID
        {
            get; set;
        }
    }
}
