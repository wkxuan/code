using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    public partial class STATIONEntity
    {
        [ForeignKey(nameof(STATIONBH), nameof(STATIONEntity.STATIONBH))]
        public List<STATION_PAYEntity> STATION_PAY
        {
            get;
            set;
        }
    }
    
}
