﻿/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 16:58:50
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("BILL_OBTAIN_ITEM", "")]
    public partial class BILL_OBTAIN_ITEMEntity : EntityBase
    {
        public BILL_OBTAIN_ITEMEntity()
        {
        }

        public BILL_OBTAIN_ITEMEntity(string billid, string type, string final_billid)
        {
            BILLID = billid;
            TYPE = type;
            FINAL_BILLID = final_billid;
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
        public string TYPE
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        [PrimaryKey]
        public string FINAL_BILLID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string MUST_MONEY
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string RECEIVE_MONEY
        {
            get; set;
        }
    }
}