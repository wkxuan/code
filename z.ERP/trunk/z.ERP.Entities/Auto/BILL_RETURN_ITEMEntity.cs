﻿/*
 * 这是自动生成的代码文件，请勿做任何修改。
 * 生成时间：2017/12/2 16:58:51
 * 生成人：书房
 * 代码生成器版本号：1.2.6545.1474
 *
 */ 

using System.Data;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    [DbTable("BILL_RETURN_ITEM", "")]
    public partial class BILL_RETURN_ITEMEntity : EntityBase
    {
        public BILL_RETURN_ITEMEntity()
        {
        }

        public BILL_RETURN_ITEMEntity(string billid, string final_billid)
        {
            BILLID = billid;
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
        public string FINAL_BILLID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// <summary>
        public string RETURN_MONEY
        {
            get; set;
        }
    }
}