/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:12
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */

using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("DEF_ALERT", "预警信息定义")]
    public partial class DEF_ALERTEntity : TableEntityBase
    {
        public DEF_ALERTEntity()
        {
        }

        public DEF_ALERTEntity(string id)
        {
            ID = id;
        }

        /// <summary>
        /// 代码
        /// <summary>
        [PrimaryKey]
        [Field("代码")]
        public string ID
        {
            get; set;
        }
        /// <summary>
        /// 名称
        /// <summary>
        [Field("名称")]
        public string MC
        {
            get; set;
        }

        /// <summary>
        /// 显示顺序
        /// <summary>
        [Field("显示顺序")]
        public string XSSX
        {
            get; set;
        }

        /// <summary>
        /// 预警SQL
        /// <summary>
        [Field("预警SQL")]
        public string SQLSTR
        {
            get; set;
        }
    }
}
