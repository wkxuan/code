using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("WLSETTLEITEM", "物料结算单子表WLSETTLEITEM")]
    public partial class WLSETTLEITEMEntity : TableEntityBase
    {
        public WLSETTLEITEMEntity()
        {
        }

        public WLSETTLEITEMEntity(string billid, string goodsid, string dh, string lx)
        {
            BILLID = billid;
            GOODSID = goodsid;
            DH = dh;
            LX = lx;
        }
        [PrimaryKey]
        public string BILLID
        {
            get; set;
        }

        [PrimaryKey]
        public string GOODSID
        {
            get; set;
        }


        [PrimaryKey]
        public string DH
        {
            get; set;
        }

        [PrimaryKey]
        public string LX
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
