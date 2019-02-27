namespace z.POS.Entities.Pos
{
    public class FindGoodsResult
    {
        public int goodsid
        {
            get; set;
        }

        public string goodscode
        {
            get;
            set;
        }
        public string name
        {
            get; set;
        }
        /// <summary>
        /// 类型：1品类2大类
        /// </summary>
        public int type
        {
            get; set;
        }
        public decimal price
        {
            get; set;
        }
        public decimal member_Price
        {
            get; set;
        }

        public int shopid
        {
            get; set;
        }

    }
}
