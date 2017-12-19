/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/19 20:30:47
 * 生成人：书房
 * 代码生成器版本号：1.2.6562.36915
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

        public FAVORITESEntity(string userid, string menuid)
        {
            USERID = userid;
            MENUID = menuid;
        }

        /// <summary>
        /// 用户编号
        /// <summary>
        [PrimaryKey]
        [Field("用户编号")]
        public string USERID
        {
            get; set;
        }
        /// <summary>
        /// 菜单编号
        /// <summary>
        [PrimaryKey]
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
