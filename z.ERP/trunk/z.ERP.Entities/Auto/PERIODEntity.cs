/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-18 0:07:22
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6560.42822
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("PERIOD", "账期区间")]
    public partial class PERIODEntity : EntityBase
    {
        public PERIODEntity()
        {
        }

        /// <summary>
        /// 年月
        /// <summary>
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
