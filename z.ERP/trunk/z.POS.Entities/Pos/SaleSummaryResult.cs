using System.Collections.Generic;

namespace z.POS.Entities.Pos
{
    public class SaleSummaryResult
    {
        public decimal saleamountsum
        {
            get;
            set;
        }

        public decimal saleamountreturn
        {
            get;
            set;
        }

        public List<PaySumResult> paysumlist
        {
            get;
            set;
        }

        public List<PayDetailResult> paydetaillist
        {
            get;
            set;
        }
    }
}
