using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("FEE_ACCOUNT", "收款单位定义")]
    public partial class FEE_ACCOUNTEntity : TableEntityBase
    {
        public FEE_ACCOUNTEntity()
        {
        }

        public FEE_ACCOUNTEntity(string id)
        {
            ID = id;
        }

        [PrimaryKey]
        [Field("单位编号")]
        public string ID
        {
            get; set;
        }

        [Field("单位名称")]
        public string NAME
        {
            get; set;
        }

        [Field("单位地址")]
        public string ADDRESS
        {
            get; set;
        }

        [Field("联系人")]
        public string CONTACT
        {
            get; set;
        }

        [Field("联系人电话")]
        public string CONTACT_NUM
        {
            get; set;
        }

        [Field("银行")]
        public string BANK
        {
            get; set;
        }

        [Field("账号")]
        public string ACCOUNT
        {
            get; set;
        }

    }

}
