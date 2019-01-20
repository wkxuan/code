/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:17
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("PERIOD", "账期区间")]
    public partial class PERIODEntity : TableEntityBase
    {
        public PERIODEntity()
        {
        }

        public PERIODEntity(string yearmonth)
        {
            YEARMONTH = yearmonth;
        }

        /// <summary>
        /// 年月
        /// <summary>
        [PrimaryKey]
        [Field("年月")]
        public string YEARMONTH
        {
            get; set;
        }
        /// <summary>
        /// 开始日期
        /// <summary>
        [Field("开始日期")]
        [DbType(DbType.DateTime)]
        public string DATE_START
        {
            get; set;
        }
        /// <summary>
        /// 结束日期
        /// <summary>
        [Field("结束日期")]
        [DbType(DbType.DateTime)]
        public string DATE_END
        {
            get; set;
        }
    }
}
