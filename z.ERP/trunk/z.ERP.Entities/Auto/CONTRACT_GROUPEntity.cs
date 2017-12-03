/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 16:58:53
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("CONTRACT_GROUP", "")]
    public partial class CONTRACT_GROUPEntity : EntityBase
    {
        public CONTRACT_GROUPEntity()
        {
        }

        public CONTRACT_GROUPEntity(string contractid, string groupno)
        {
            CONTRACTID = contractid;
            GROUPNO = groupno;
        }

        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string CONTRACTID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string GROUPNO
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string JSKL
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string DESCRIPTION
        {
            get; set;
        }
    }
}
