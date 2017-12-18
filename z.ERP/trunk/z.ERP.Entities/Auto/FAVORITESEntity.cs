/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-19 0:12:02
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6560.42822
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("FAVORITES", "收藏夹")]
    public partial class FAVORITESEntity : EntityBase
    {
        public FAVORITESEntity()
        {
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
        /// 菜单编号
        /// <summary>
        [Field("菜单编号")]
        public string MENUID
        {
            get; set;
        }
        /// <summary>
        /// 显示顺序
        /// <summary>
        [Field("显示顺序")]
        public string SERIAL_NUM
        {
            get; set;
        }
    }
}
