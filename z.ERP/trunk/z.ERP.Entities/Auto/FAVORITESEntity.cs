/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 16:58:56
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("FAVORITES", "")]
    public partial class FAVORITESEntity : EntityBase
    {
        public FAVORITESEntity()
        {
        }

        public FAVORITESEntity(string userid, string menuid)
        {
            USERID = userid;
            MENUID = menuid;
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
        public string MENUID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string SERIAL_NUM
        {
            get; set;
        }
    }
}
