using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("WLCHECKITEM", "损溢单子表WLCHECKITEM")]
    public partial class WLCHECKITEMEntity : TableEntityBase
    {
        public WLCHECKITEMEntity()
        {
        }

        public WLCHECKITEMEntity(string billid, string goodsid)
        {
            BILLID = billid;
            GOODSID = goodsid;
        }
        [PrimaryKey]
        public string BILLID
        {
            get; set;
        }
        /// <summary>
        /// 收款方式
        /// <summary>
        [PrimaryKey]
        public string GOODSID
        {
            get; set;
        }


        public string QUANTITY
        {
            get; set;
        }
        public string CANQTY
        {
            get; set;
        }
    }
}
