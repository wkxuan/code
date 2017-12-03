/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 16:59:00
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("P1", "")]
    public partial class P1Entity : EntityBase
    {
        public P1Entity()
        {
        }

        public P1Entity(string f1)
        {
            F1 = f1;
        }

        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string F1
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string F2
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string F3
        {
            get; set;
        }
    }
}
