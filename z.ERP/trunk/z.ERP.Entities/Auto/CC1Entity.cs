/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/5 22:24:28
 * 生成人：书房
 * 代码生成器版本号：1.2.6562.36915
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("CC1", "")]
    public partial class CC1Entity : EntityBase
    {
        public CC1Entity()
        {
        }

        public CC1Entity(string ccf1, string ccf2, string ccf3)
        {
            CCF1 = ccf1;
            CCF2 = ccf2;
            CCF3 = ccf3;
        }

        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string CCF1
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string CCF2
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string CCF3
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string CCF4
        {
            get; set;
        }
    }
}
