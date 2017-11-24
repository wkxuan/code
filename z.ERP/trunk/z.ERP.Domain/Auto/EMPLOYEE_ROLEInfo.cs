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
    [DbTable("EMPLOYEE_ROLE")]
    public class EMPLOYEE_ROLEInfo : DomainBase
    {
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string EMPLOYEEID
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
