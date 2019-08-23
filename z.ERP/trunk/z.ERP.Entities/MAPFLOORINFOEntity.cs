using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.ERP.Entities
{
    public  class MAPFLOORINFOEntity
    {
        public string BRANCHID { set; get; }
        public string REGIONID { set; get; }
        public string FLOORID { set; get; }
        public List<MAPSHOP> MAPSHOPLIST { set; get; }
    }
    public class MAPSHOP {
        public string TYPE { set; get; }
        public string POINTS { set; get; }
        public string COLOR { set; get; }
        public MAPSHOPINFO SHOPINFO { set; get; }
    }
    public class MAPSHOPINFO
    {
        public string ID { set; get; }
        public string NAME { set; get; }
        public string TYPE { set; get; }
        public string STATUS { set; get; }
        public string RENT_STATUS { set; get; }
    }
}
