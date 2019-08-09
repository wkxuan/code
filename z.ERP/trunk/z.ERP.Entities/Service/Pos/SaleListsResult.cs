using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.ERP.Entities.Service.Pos
{
    public class SaleListsResult
    {
        public int recordCount
        {
            get;
            set;
        }

        public List<SaleLists> saleLists
        {
            get;
            set;
        }

    }
}
