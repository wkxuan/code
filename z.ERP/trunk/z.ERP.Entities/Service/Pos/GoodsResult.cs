using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.ERP.Entities.Service.Pos
{
    public class GoodsResult
    {
        public string sheetid
        {
            get; set;
        }
        public string inx
        {
            get; set;
        }
        public string shopid
        {
            get; set;
        }
        public string goodsid
        {
            get; set;
        }
        public string goodscode
        {
            get; set;
        }
        public string price
        {
            get; set;
        }
        public string quantity
        {
            get; set;
        }
        public string sale_amount
        {
            get; set;
        }
        public string discount_amount
        {
            get; set;
        }
        public string coupon_amount
        {
            get; set;
        }

    }
}
