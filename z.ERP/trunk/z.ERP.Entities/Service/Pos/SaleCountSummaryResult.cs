using System.Collections.Generic;

namespace z.ERP.Entities.Service.Pos
{
    public class SaleCountSummaryResult
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

        public int salecountsum
        {
            get;
            set;
        }

        public int returncountsum
        {
            get;
            set;
        }


        public List<PaySumCountResult> paysumlist
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
