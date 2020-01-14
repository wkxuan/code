using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("APPROVAL_BRANCH", "门店审批开关")]
    public partial class APPROVAL_BRANCHEntity : TableEntityBase
    {
        public APPROVAL_BRANCHEntity()
        {
        }

        public APPROVAL_BRANCHEntity(string apprid, string branchid)
        {
            APPRID = apprid;
            BRANCHID = branchid;
        }
        [PrimaryKey]
        [Field("模板ID")]
        public string APPRID
        {
            get; set;
        }
        /// <summary>
        /// 分店ID
        /// <summary>
        [PrimaryKey]
        [Field("分店ID")]
        public string BRANCHID
        {
            get; set;
        }
        [Field("状态1启用 2未启用")]
        public string STATUS
        {
            get; set;
        }
    }
}

