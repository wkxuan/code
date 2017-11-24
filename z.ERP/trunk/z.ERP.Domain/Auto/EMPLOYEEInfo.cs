/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/11/20 0:06:12
 * 生成人：书房
 * 代码生成器版本号：1.1.6533.181
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Domain.Auto
{
    [DbTable("EMPLOYEE")]
    public class EMPLOYEEInfo : DomainBase
    {
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string ID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string NAME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string LOGINNAME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string PASSWORD
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string STATUS
        {
            get; set;
        }
    }
}
