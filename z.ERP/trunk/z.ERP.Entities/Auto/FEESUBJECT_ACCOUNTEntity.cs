using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities.Auto
{
    [DbTable("FEESUBJECT_ACCOUNT", "费用项目门店")]
    public class FEESUBJECT_ACCOUNTEntity:TableEntityBase
    {
        public FEESUBJECT_ACCOUNTEntity()
        {
        }

        public FEESUBJECT_ACCOUNTEntity(string branchid,string tremid)
        {
            TERMID = tremid;
            BRANCHID = branchid;
        }
        [PrimaryKey]
        [Field("费用项目")]
        public string TERMID
        {
            get; set;
        }
        [PrimaryKey]
        [Field("分店")]
        public string BRANCHID
        {
            get; set;
        }
        [Field("收费单位")]
        public string FEE_ACCOUNTID
        {
            get; set;
        }
        [Field("通知单生成方式")]
        public string NOTICE_CREATE_WAY
        {
            get; set;
        }
    }
}
