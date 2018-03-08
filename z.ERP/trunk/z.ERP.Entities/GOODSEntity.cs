using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    public partial class GOODSEntity
    {
        [ForeignKey(nameof(GOODSID), nameof(GOODSEntity.GOODSID))]
        public List<GOODS_SHOPEntity> GOODS_SHOP
        {
            get;
            set;
        }
    }
}
