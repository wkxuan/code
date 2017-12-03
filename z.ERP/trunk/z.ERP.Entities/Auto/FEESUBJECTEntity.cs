/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 16:58:57
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("FEESUBJECT", "")]
    public partial class FEESUBJECTEntity : EntityBase
    {
        public FEESUBJECTEntity()
        {
        }

        public FEESUBJECTEntity(string trimid)
        {
            TRIMID = trimid;
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
        public string CODE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string NAME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string PYM
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string TYPE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string ACCOUNT
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string DEDUCTION
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string CREATE_TIME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [DbType(DbType.DateTime)]
        public string UPDATE_TIME
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string VOID_FLAG
        {
            get; set;
        }
    }
}
