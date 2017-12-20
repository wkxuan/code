/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/19 20:30:48
 * 生成人：书房
 * 代码生成器版本号：1.2.6562.36915
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("INC_TAB", "")]
    public partial class INC_TABEntity : EntityBase
    {
        public INC_TABEntity()
        {
        }

        public INC_TABEntity(string tblname)
        {
            TBLNAME = tblname;
        }

        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string TBLNAME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string VALUE
        {
            get; set;
        }
    }
}
