using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("REDEMPTIONRULES", "积分抵现规则")]
    public partial class REDEMPTIONRULESEntity : TableEntityBase
    {
        public REDEMPTIONRULESEntity()
        {
        }

        public REDEMPTIONRULESEntity(string id)
        {
            ID = id;
        }

        [PrimaryKey]
        [Field("编号")]
        public string ID
        {
            get; set;
        }

        [Field("门店")]
        public string BRANCHID
        {
            get; set;
        }

        [Field("开始日期")]
        [DbType(DbType.DateTime)]
        public string START_DATE
        {
            get; set;
        }

        [Field("结束日期")]
        [DbType(DbType.DateTime)]
        public string END_DATE
        {
            get; set;
        }

        [Field("积分")]
        public string CENT
        {
            get; set;
        }

        [Field("金额")]
        public string MONEY
        {
            get; set;
        }

        [Field("状态")]
        public string STATUS
        {
            get; set;
        }
    }

}
