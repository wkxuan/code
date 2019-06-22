using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("BILLSTATUS", "待处理任务")]
    public partial class BILLSTATUSEntity: TableEntityBase
    {
        public BILLSTATUSEntity()
        {
        }

        public BILLSTATUSEntity(string billid, string menuid)
        {
            BILLID = billid;
            MENUID = menuid;
        }
        /// <summary>
        ///单据号 
        /// </summary>
        [PrimaryKey]
        [Field("单据号")]
        public string BILLID
        {
            get; set;
        }
        /// <summary>
        ///菜单号 
        /// </summary>
        [PrimaryKey]
        [Field("菜单号")]
        public string MENUID
        {
            get; set;
        }
        /// <summary>
        /// 分店编号
        /// </summary>
        [Field("分店编号")]
        public string BRABCHID
        {
            get; set;
        }
        /// <summary>
        /// 路径
        /// </summary>
        [Field("路径")]
        public string URL
        {
            get; set;
        }

    }
}
