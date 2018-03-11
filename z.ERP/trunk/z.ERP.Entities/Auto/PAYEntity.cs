/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/11 22:19:01
 * 生成人：书房
 * 代码生成器版本号：1.2.6562.36915
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("PAY", "收款方式")]
    public partial class PAYEntity : EntityBase
    {
        public PAYEntity()
        {
        }

        public PAYEntity(string payid)
        {
            PAYID = payid;
        }

        /// <summary>
        /// 收款方式编号
        /// <summary>
        [PrimaryKey]
        [Field("收款方式编号")]
        public string PAYID
        {
            get; set;
        }
        /// <summary>
        /// 收款方式名称
        /// <summary>
        [Field("收款方式名称")]
        public string NAME
        {
            get; set;
        }
        /// <summary>
        /// 收款方式类型
        /// <summary>
        [Field("收款方式类型")]
        public string TYPE
        {
            get; set;
        }
        /// <summary>
        /// 停用标记
        /// <summary>
        [Field("停用标记")]
        public string VOID_FLAG
        {
            get; set;
        }
        /// <summary>
        /// 返款
        /// <summary>
        [Field("返款")]
        public string FK
        {
            get; set;
        }
        /// <summary>
        /// 积分
        /// <summary>
        [Field("积分")]
        public string JF
        {
            get; set;
        }
        /// <summary>
        /// 找零方式
        /// <summary>
        [Field("找零方式")]
        public string ZLFS
        {
            get; set;
        }
        /// <summary>
        /// 显示顺序号
        /// <summary>
        [Field("显示顺序号")]
        public string FLAG
        {
            get; set;
        }
    }
}
