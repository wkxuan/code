using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;
using z.ERP.Entities.Auto;

namespace z.ERP.Entities
{
    public partial class NOTICEEntity
    {
        [ForeignKey(nameof(ID), nameof(NOTICE_BRANCHEntity.NOTICEID))]
        public List<NOTICE_BRANCHEntity> NOTICE_BRANCH
        {
            get;
            set;
        }
    }
}
