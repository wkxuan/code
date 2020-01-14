using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("APPROVAL_NODE", "审批流节点")]
    public partial class APPROVAL_NODEEntity : TableEntityBase
    {
        public APPROVAL_NODEEntity()
        {
        }

        public APPROVAL_NODEEntity(string appr_node_id)
        {
            APPR_NODE_ID = appr_node_id;
        }
        [PrimaryKey]
        [Field("节点ID")]
        public string APPR_NODE_ID
        {
            get; set;
        }
        /// <summary>
        /// 分店ID
        /// <summary>
        [Field("分店ID")]
        public string BRANCHID
        {
            get; set;
        }
        [Field("模板id")]
        public string APPRID
        {
            get; set;
        }
        [Field("节点序号")]
        public string NODE_INX
        {
            get; set;
        }
        [Field("节点标题")]
        public string NODE_TITLE
        {
            get; set;
        }
        [Field("下个节点ID")]
        public string NEXT_APPR_NODE_ID
        {
            get; set;
        }
    }
}

