/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/5 22:24:37
 * 生成人：书房
 * 代码生成器版本号：1.2.6562.36915
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("OPERATIONRULE", "合作经营方式")]
    public partial class OPERATIONRULEEntity : EntityBase
    {
        public OPERATIONRULEEntity()
        {
        }

        public OPERATIONRULEEntity(string id)
        {
            ID = id;
        }

        /// <summary>
        /// ID
        /// <summary>
        [PrimaryKey]
        [Field("ID")]
        public string ID
        {
            get; set;
        }
        /// <summary>
        /// 名称
        /// <summary>
        [Field("名称")]
        public string NAME
        {
            get; set;
        }
        /// <summary>
        /// 物业标记
        /// <summary>
        [Field("物业标记")]
        public string WYSIGN
        {
            get; set;
        }
        /// <summary>
        /// 合作方式
        /// <summary>
        [Field("合作方式")]
        public string PROCESSTYPE
        {
            get; set;
        }
        /// <summary>
        /// 阶梯标记
        /// <summary>
        [Field("阶梯标记")]
        public string LADDERSIGN
        {
            get; set;
        }
    }
}
