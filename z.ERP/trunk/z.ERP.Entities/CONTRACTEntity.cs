using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    public partial class CONTRACTEntity
    {
        [ForeignKey(nameof(CONTRACTID), nameof(CONTRACT_BRANDEntity.CONTRACTID))]
        public List<CONTRACT_BRANDEntity> CONTRACT_BRAND
        {
            get;
            set;
        }

        [ForeignKey(nameof(CONTRACTID), nameof(CONTRACT_SHOPEntity.CONTRACTID))]
        public List<CONTRACT_SHOPEntity> CONTRACT_SHOP
        {
            get;
            set;
        }


        [ForeignKey(nameof(CONTRACTID), nameof(CONTRACT_GROUPEntity.CONTRACTID))]
        public List<CONTRACT_GROUPEntity> CONTRACT_GROUP
        {
            get;
            set;
        }


        [ForeignKey(nameof(CONTRACTID), nameof(CONTRACT_RENTEntity.CONTRACTID))]
        public List<CONTRACT_RENTEntity> CONTRACT_RENT
        {
            get;
            set;
        }

    }

    public partial class CONTRACT_RENTEntity
    {
        [ForeignKey(nameof(CONTRACTID), nameof(CONTRACT_RENTITEMEntity.CONTRACTID))]
        [ForeignKey(nameof(INX), nameof(CONTRACT_RENTITEMEntity.INX))]
        public List<CONTRACT_RENTITEMEntity> CONTRACT_RENTITEM
        {
            get;
            set;
        }
        //[ForeignKey(nameof(CONTRACTID), nameof(CONTJSKLEntity.CONTRACTID))]
        //[ForeignKey(nameof(INX), nameof(CONTJSKLEntity.INX))]
        //public List<CONTJSKLEntity> CONTJSKL
        //{
        //    get;
        //    set;
        //}

    }

    public partial class CONTRACT_GROUPEntity
    {
        [ForeignKey(nameof(CONTRACTID), nameof(CONTJSKLEntity.CONTRACTID))]
        [ForeignKey(nameof(GROUPNO), nameof(CONTJSKLEntity.GROUPNO))]
        public List<CONTJSKLEntity> CONTJSKL
        {
            get;
            set;
        }

    }
}
