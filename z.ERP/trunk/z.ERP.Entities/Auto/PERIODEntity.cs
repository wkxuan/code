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
    [DbTable("PERIOD", "")]
    public partial class PERIODEntity : EntityBase
    {
        public PERIODEntity()
        {
        }

        public PERIODEntity(string yearmonth)
        {
            YEARMONTH = yearmonth;
        }

        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string YEARMONTH
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string DATE_START
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string DATE_END
        {
            get; set;
        }
    }
}
