using System.Collections.Generic;

namespace z.ERP.Entities.Service.Pos
{
    public class SaleSummaryResult
    {
        public decimal saleamountsum
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
