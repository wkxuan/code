/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:11
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("DAILYSETTLEMENT", "日结状态")]
    public partial class DAILYSETTLEMENTEntity : TableEntityBase
    {
        public DAILYSETTLEMENTEntity()
        {
        }

        public DAILYSETTLEMENTEntity(string day, string moduleid)
        {
            DAY = day;
            MODULEID = moduleid;
        }

        /// <summary>
        /// 日期
        /// <summary>
        [PrimaryKey]
        [Field("日期")]
        [DbType(DbType.DateTime)]
        public string DAY
        {
            get; set;
        }
        /// <summary>
        /// 模块编号
        /// <summary>
        [PrimaryKey]
        [Field("模块编号")]
        public string MODULEID
        {
            get; set;
        }
        /// <summary>
        /// 用户编号
        /// <summary>
        [Field("用户编号")]
        public string USERID
        {
            get; set;
        }
        /// <summary>
        /// 机器编号
        /// <summary>
        [Field("机器编号")]
        public string MACHINE
        {
            get; set;
        }
        /// <summary>
        /// 开始时间
        /// <summary>
        [Field("开始时间")]
        [DbType(DbType.DateTime)]
        public string DATE_START
        {
            get; set;
        }
        /// <summary>
        /// 结束时间
        /// <summary>
        [Field("结束时间")]
        [DbType(DbType.DateTime)]
        public string DATE_END
        {
            get; set;
        }
        /// <summary>
        /// 处理状态
        /// <summary>
        [Field("处理状态")]
        public string STATUS
        {
            get; set;
        }
    }
}
