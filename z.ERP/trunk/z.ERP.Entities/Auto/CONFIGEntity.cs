/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-18 0:07:19
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6560.42822
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("CONFIG", "系统运行参数配置")]
    public partial class CONFIGEntity : EntityBase
    {
        public CONFIGEntity()
        {
        }

        /// <summary>
        /// 编号
        /// <summary>
        [Field("编号")]
        public string ID
        {
            get; set;
        }
        /// <summary>
        /// 默认值
        /// <summary>
        [Field("默认值")]
        public string DEF_VAL
        {
            get; set;
        }
        /// <summary>
        /// 当前值
        /// <summary>
        [Field("当前值")]
        public string CUR_VAL
        {
            get; set;
        }
        /// <summary>
        /// 最大值
        /// <summary>
        [Field("最大值")]
        public string MAX_VAL
        {
            get; set;
        }
        /// <summary>
        /// 最小值
        /// <summary>
        [Field("最小值")]
        public string MIN_VAL
        {
            get; set;
        }
        /// <summary>
        /// 描述
        /// <summary>
        [Field("描述")]
        public string DESCRIPTION
        {
            get; set;
        }
    }
}
