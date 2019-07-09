using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Auto
{
    [DbTable("NOTICE_BRANCH", "通知门店表")]
    public partial class NOTICE_BRANCHEntity:TableEntityBase
    {
        public NOTICE_BRANCHEntity(){}
        public NOTICE_BRANCHEntity(string noticeid,string branchid){
            NOTICEID = noticeid;
            BRANCHID = branchid;
        }
        /// <summary>
        /// 通知ID
        /// <summary>
        [PrimaryKey]
        [Field("通知ID")]
        public string NOTICEID
        {
            get; set;
        }
        /// <summary>
        /// 门店id
        /// <summary>
        [PrimaryKey]
        [Field("门店id")]
        public string BRANCHID
        {
            get; set;
        }
    }
}
