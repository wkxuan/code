using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.ERP.Entities
{
    public class STATIONMONITOR
    {
        public string BRANCHID { set; get; }
        public string BRANCHNAME { set; get; }
        public List<STATIONINFO> STATIONINFOList { set; get; }
    }
    public class STATIONINFO {
        public string STATIONBH { set; get; }
        public string CASHIERNAME { set; get; }
        public int STATIONSTATUS { set; get; }
        public int GAPTIME { set; get; }
        public string REFRESHTIME { set; get; }
    }
}
