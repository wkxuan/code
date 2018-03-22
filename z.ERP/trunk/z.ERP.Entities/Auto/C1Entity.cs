/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:07
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("C1", "")]
    public partial class C1Entity : TableEntityBase
    {
        public C1Entity()
        {
        }

        public C1Entity(string cf1, string cf2)
        {
            CF1 = cf1;
            CF2 = cf2;
        }

        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string CF1
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string CF2
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string CF3
        {
            get; set;
        }
    }
}
