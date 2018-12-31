using System.Data;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    [DbTable("WLUSESITETM", "物料领用单子表WLUSESITETM")]
    public partial class WLUSESITETMEntity : TableEntityBase
    {
        public WLUSESITETMEntity()
        {
        }

        public WLUSESITETMEntity(string billid, string goodsid)
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
