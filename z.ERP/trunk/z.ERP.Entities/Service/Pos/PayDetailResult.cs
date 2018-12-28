using System;

namespace z.ERP.Entities.Service.Pos
{
    public class PayDetailResult
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

        public int returnflag
        {
            get;
            set;
        }

        public string sale_time
        {
            get;
            set;
        }

        public int payid
        {
            get;
            set;
        }

        public string payname
        {
            get;
            set;
        }

        public string paytype
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
