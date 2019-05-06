using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.ERP.Entities.Service.Pos
{
    public class LoginConfigInfo
    {
        public string branchid
        {
            get;
            set;
        }

        public string crmStoreCode
        {
            get;
            set;
        }

        public string shopid
        {
            get;
            set;
        }

        public string shopcode
        {
            get;
            set;
        }

        public string shopname
        {
            get;
            set;
        }

        public TicketInfo ticketInfo
        {
            get;
            set;
        }

        public PosWFTConfig posWFTConfig
        {
            get;
            set;
        }

        public PosUMSConfig posUMSConfig
        {
            get;
            set;
        }
    }
}