using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.DGS.Entities
{
    public class SaleGatherReq
    {

        public string saleTime
        {
            get; set;
        }

        public double amount
        {
            get; set;
        }

        public List<SalePay> payList
        {
            get; set;
        }
    }
}
