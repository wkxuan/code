using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("WLOUTSTOCKITETM", "物料购进单子表WLOUTSTOCKITETM")]
    public partial class WLOUTSTOCKITETMEntity : TableEntityBase
    {
        public WLOUTSTOCKITETMEntity()
        {
        }

        public WLOUTSTOCKITETMEntity(string billid, string goodsid)
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

        public string TAXINPRICE
        {
            get; set;
        }
        public string CANQTY
        {
            get; set;
        }
    }
}
