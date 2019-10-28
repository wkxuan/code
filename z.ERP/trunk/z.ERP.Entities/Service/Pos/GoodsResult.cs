namespace z.ERP.Entities.Service.Pos
{
    public class GoodsResult
    {
        public int sheetid
        {
            get; set;
        }
        public int inx
        {
            get; set;
        }
        public int shopid
        {
            get; set;
        }
        public int goodsid
        {
            get; set;
        }
        public string goodscode
        {
            get; set;
        }

        public string goodsname
        {
            get; set;
        }

        public decimal price
        {
            get; set;
        }
        public float quantity
        {
            get; set;
        }

        public float returns  //退货数量 
        {
            get; set;
        }

        public decimal sale_amount
        {
            get; set;
        }
        public decimal discount_amount
        {
            get; set;
        }
        public decimal coupon_amount
        {
            get; set;
        }

    }
}
