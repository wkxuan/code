
using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("SPLCJG_MENU", "审批流程数据")]
    public partial class SPLCJG_MENUEntity : TableEntityBase
    {
        public SPLCJG_MENUEntity()
        {
        }

        public SPLCJG_MENUEntity(string billid, string menuid, string xh)
        {
            BILLID = billid;
            MENUID = menuid;
            XH = xh;
        }

        /// <summary>
        /// 代码
        /// <summary>
        [PrimaryKey]
        [Field("菜单号")]
        public string MENUID
        {
            get; set;
        }
        [PrimaryKey]
        [Field("单据号")]
        public string BILLID
        {
            get; set;
        }
        [PrimaryKey]
        [Field("序号")]
        public string XH
        {
            get; set;
        }

        [Field("最后节点")]
        public string JDID
        {
            get; set;
        }

        [Field("处理时间")]
        [DbType(DbType.DateTime)]
        public string CLSJ
        {
            get; set;
        }

        [Field("描述")]
        public string BZ
        {
            get; set;
        }
        public string JGTYPE
        {
            get; set;
        }
    }
}
