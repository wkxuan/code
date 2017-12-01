/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 0:49:27
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("JOIN_BILL_TRINM", "")]
    public partial class JOIN_BILL_TRINMEntity : EntityBase
    {
        public JOIN_BILL_TRINMEntity()
        {
        }

        public JOIN_BILL_TRINMEntity(string billid, string trimid, string inx)
        {
            BILLID = billid;
            TRIMID = trimid;
            INX = inx;
        }

        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string BILLID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string TRIMID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string INX
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string JE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string FINAL_BILLID
        {
            get; set;
        }
    }
}
