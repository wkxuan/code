using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("APPROVAL_NODE_OPER", "节点权限")]
    public partial class APPROVAL_NODE_OPEREntity : TableEntityBase
    {
        public APPROVAL_NODE_OPEREntity()
        {
        }

        public APPROVAL_NODE_OPEREntity(string appr_node_id, string oper_type,string oper_data)
        {
            APPR_NODE_ID = appr_node_id;
            OPER_TYPE = oper_type;
            OPER_DATA = oper_data;
        }
        [PrimaryKey]
        [Field("模板ID")]
        public string APPR_NODE_ID
        {
            get; set;
        }
        [PrimaryKey]
        [Field("分店ID")]
        public string OPER_TYPE
        {
            get; set;
        }
        [PrimaryKey]
        [Field("状态1启用 2未启用")]
        public string OPER_DATA
        {
            get; set;
        }
    }
}

