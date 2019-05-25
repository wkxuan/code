
using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("SPLCJG", "审批流程节点结构")]
    public partial class SPLCJGEntity : TableEntityBase
    {
        public SPLCJGEntity()
        {
        }

        public SPLCJGEntity(string billid, string jdid, string jgid)
        {
            BILLID = billid;
            JDID = jdid;
            JGID = jgid;
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
        [PrimaryKey]
        public string JGID
        {
            get; set;
        }

        public string TJMC
        {
            get; set;
        }

        public string JGTYPE
        {
            get; set;
        }


        public string JGMC
        {
            get; set;
        }
    }
}
