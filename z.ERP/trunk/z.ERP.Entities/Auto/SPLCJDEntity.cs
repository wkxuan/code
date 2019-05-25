
using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("SPLCJD", "审批流程节点")]
    public partial class SPLCJDEntity : TableEntityBase
    {
        public SPLCJDEntity()
        {
        }

        public SPLCJDEntity(string billid, string jdid)
        {
            BILLID = billid;
            JDID = jdid;
        }

        /// <summary>
        /// 代码
        /// <summary>
        [PrimaryKey]
        [Field("单号")]
        public string BILLID
        {
            get; set;
        }
        [PrimaryKey]
        public string JDID
        {
            get; set;
        }

        public string JDNAME
        {
            get; set;
        }

        public string JDTYPE
        {
            get; set;
        }


        public string ROLEID
        {
            get; set;
        }
        public string JDINX
        {
            get; set;
        }
    }
}
