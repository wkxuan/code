/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:08
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */

using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("FLOORMAP", "楼层图纸")]
    public partial class FLOORMAPEntity : TableEntityBase
    {
        public FLOORMAPEntity()
        {
        }

        public FLOORMAPEntity(string mapid)
        {
            MAPID = mapid;
        }

        /// <summary>
        /// 图纸编号
        /// <summary>
        [PrimaryKey]
        [Field("图纸编号")]
        public string MAPID
        {
            get; set;
        }
        /// <summary>
        /// 楼层编号
        /// <summary>
        [Field("楼层编号")]
        public string FLOORID
        {
            get; set;
        }
        /// <summary>
        /// 底图
        /// <summary>
        [Field("底图")]
        public string BACKMAP
        {
            get; set;
        }
        /// <summary>
        ///图纸宽度
        /// <summary>
        [Field("图纸宽度")]
        public string WIDTHS
        {
            get; set;
        }
        /// <summary>
        /// 图纸长度
        /// <summary>
        [Field("图纸长度")]
        public string LENGTHS
        {
            get; set;
        }
        /// <summary>
        /// 启动时间
        /// <summary>
        [Field("计划启动时间")]
        [DbType(DbType.DateTime)]
        public string INITINATE_TIME_P
        {
            get; set;
        }
        /// <summary>
        /// 录入员
        /// <summary>
        [Field("录入员")]
        public string REPORTER
        {
            get; set;
        }
        /// <summary>
        /// 录入员名称
        /// <summary>
        [Field("录入员名称")]
        public string REPORTER_NAME
        {
            get; set;
        }
        /// <summary>
        /// 录入时间
        /// <summary>
        [Field("录入时间")]
        [DbType(DbType.DateTime)]
        public string REPORTER_TIME
        {
            get; set;
        }
        /// <summary>
        /// 确认人
        /// <summary>
        [Field("确认人")]
        public string VERIFY
        {
            get; set;
        }
        /// <summary>
        /// 确认人名称
        /// <summary>
        [Field("确认人名称")]
        public string VERIFY_NAME
        {
            get; set;
        }
        /// <summary>
        /// 确认时间
        /// <summary>
        [Field("确认时间")]
        [DbType(DbType.DateTime)]
        public string VERIFY_TIME
        {
            get; set;
        }
                /// <summary>
        /// 启动人
        /// <summary>
        [Field("启动人")]
        public string INITINATE
        {
            get; set;
        }
        /// <summary>
        /// 启动人名称
        /// <summary>
        [Field("启动人名称")]
        public string INITINATE_NAME
        {
            get; set;
        }
        /// <summary>
        /// 启动时间
        /// <summary>
        [Field("启动时间")]
        [DbType(DbType.DateTime)]
        public string INITINATE_TIME
        {
            get; set;
        }
        /// <summary>
        /// 终止人
        /// <summary>
        [Field("终止人")]
        public string TERMINATE
        {
            get; set;
        }
        /// <summary>
        /// 终止人名称
        /// <summary>
        [Field("终止人名称")]
        public string TERMINATE_NAME
        {
            get; set;
        }
        /// <summary>
        /// 终止时间
        /// <summary>
        [Field("终止时间")]
        [DbType(DbType.DateTime)]
        public string TERMINATE_TIME
        {
            get; set;
        }
        /// <summary>
        /// 状态
        /// <summary>
        [Field("状态")]
        public string STATUS
        {
            get; set;
        }

    }
}
