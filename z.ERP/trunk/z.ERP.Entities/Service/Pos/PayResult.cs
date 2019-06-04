using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.ERP.Entities.Service.Pos
{
    public class PayResult
    {
        public int payid
        {
            get;
            set;
        }
        public decimal amount
        {
            get;
            set;
        }

        public string payname
        {
            get;
            set;
        }

        public int paytype
        {
            get;
            set;
        }

    }
}
