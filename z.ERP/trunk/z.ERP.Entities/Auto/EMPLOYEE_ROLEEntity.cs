/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/11/28 21:03:15
 * 生成人：书房
 * 代码生成器版本号：1.2.6537.1447
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("EMPLOYEE_ROLE", "")]
    public partial class EMPLOYEE_ROLEEntity : EntityBase
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
