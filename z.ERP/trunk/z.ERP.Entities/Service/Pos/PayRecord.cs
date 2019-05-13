using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.ERP.Entities.Service.Pos
{
    public class PayRecord
    {
        public int inx
        {
            get;
            set;
        }
        public int payid
        {
            get;
            set;
        }

        public string cardno
        {
            get;
            set;
        }

        public string bank
        {
            get;
            set;
        }

        public int bankid
        {
            get;
            set;
        }

        public double amount
        {
            get;
            set;
        }

        public string serialno
        {
            get;
            set;
        }

        public string refno
        {
            get;
            set;
        }

        public string opertime
        {
            get;
            set;
        }


    }
}