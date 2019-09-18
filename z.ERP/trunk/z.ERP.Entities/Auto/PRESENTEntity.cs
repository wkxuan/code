/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2018/3/22 0:39:18
 * 生成人：书房
 * 代码生成器版本号：1.2.6655.1027
 *
 */ 

using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("Present", "赠品定义")]
    public partial class PresentEntity : TableEntityBase
    {
        public PresentEntity()
        {
        }

        public PresentEntity(string id)
        {
            ID = id;
        }

        /// <summary>
        /// 门店id
        /// <summary>    
        [Field("门店id")]
        public string BRANCHID
        {
            get; set;
        }
        
        /// <summary>
        /// 赠品id
        /// <summary>
        [PrimaryKey]
        [Field("赠品id")]
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
        /// 价值
        /// <summary>
        [Field("价值")]
        public string PRICE
        {
            get; set;
        }
        /// <summary>
        /// 状态
        /// <summary>
        [Field("状态")]
        public string STATUS
        {
            get; set;
        }
      
    }
}
