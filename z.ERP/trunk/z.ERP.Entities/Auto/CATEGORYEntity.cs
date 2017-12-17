/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017-12-18 0:07:18
 * 生成人：LinAJ
 * 代码生成器版本号：1.2.6560.42822
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("CATEGORY", "业态")]
    public partial class CATEGORYEntity : EntityBase
    {
        public CATEGORYEntity()
        {
        }

        /// <summary>
        /// 业态编码
        /// <summary>
        [Field("业态编码")]
        public string CATEGORYID
        {
            get; set;
        }
        /// <summary>
        /// 业态代码
        /// <summary>
        [Field("业态代码")]
        public string CATEGORYCODE
        {
            get; set;
        }
        /// <summary>
        /// 业态名称
        /// <summary>
        [Field("业态名称")]
        public string CATEGORYNAME
        {
            get; set;
        }
        /// <summary>
        /// 末级标志
        /// <summary>
        [Field("末级标志")]
        public string LEVEL_LAST
        {
            get; set;
        }
    }
}
