using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.ERP.Entities.Service.Pos
{
    public class SaleSummaryFilter
    {

        public string posno
        {
            get;
            set;
        }
        public DateTime? saledate_begin
        {
            get;
            set;
        }
        public DateTime? saledate_end
        {
            get;
            set;
        }
    }
}
