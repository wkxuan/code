/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/11/24 0:48:20
 * 生成人：书房
 * 代码生成器版本号：1.2.6537.1447
 *
 */

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("BM", "部门")]
    public partial class BMEntity : EntityBase
    {
        /// <summary>
        /// 部门ID
        /// <summary>
        [Field("部门ID")]
        public string DEPTID
        {
            get; set;
        }
        /// <summary>
        /// 部门名称
        /// <summary>
        [Field("部门名称")]
        public string DEPT_NAME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string DEPT_TYPE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string BMJB
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string MJBZ
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string ISSHOW
        {
            get; set;
        }
    }
}
