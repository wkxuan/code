using System;

namespace z.POS.Entities.Pos
{
    public class SaleList
    {
        public string posno
        {
            get;
            set;
        }

        public int dealid
        {
            get;
            set;
        }

        public string sale_time
        {
            get;
            set;
        }

        public decimal amount
        {
            get;
            set;
        }
    }
}
