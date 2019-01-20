using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    public partial class P1Entity
    {
        [ForeignKey(nameof(F1), nameof(C1Entity.CF1))]
        public List<C1Entity> c1s
        {
            get;
            set;
        }
    }

    public partial class C1Entity
    {
        [ForeignKey(nameof(CF1), nameof(CC1Entity.CCF1))]
        [ForeignKey(nameof(CF2), nameof(CC1Entity.CCF2))]
        public CC1Entity[] cc1s
        {
            get;
            set;
        }
    }
}
