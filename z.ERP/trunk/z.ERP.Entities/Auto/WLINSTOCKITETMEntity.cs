using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("WLINSTOCKITETM", "物料购进单子表WLINSTOCKITETM")]
    public partial class WLINSTOCKITETMEntity : TableEntityBase
    {
        public WLINSTOCKITETMEntity()
        {
        }

        public WLINSTOCKITETMEntity(string billid, string goodsid)
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
    }
}
