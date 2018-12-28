using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    public partial class CONTRACTEntity { 
        [ForeignKey(nameof(CONTRACTID), nameof(CONTRACT_UPDATEEntity.CONTRACTID))]
        public List<CONTRACT_UPDATEEntity> CONTRACT_UPDATE
        {
            get;
            set;
        }

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


        [ForeignKey(nameof(CONTRACTID), nameof(CONTJSKLEntity.CONTRACTID))]
        public List<CONTJSKLEntity> CONTJSKL
        {
            get;
            set;
        }

        [ForeignKey(nameof(CONTRACTID), nameof(CONTRACT_PAYEntity.CONTRACTID))]
        public List<CONTRACT_PAYEntity> CONTRACT_PAY
        {
            get;
            set;
        }

        [ForeignKey(nameof(CONTRACTID), nameof(CONTRACT_COSTEntity.CONTRACTID))]
        public List<CONTRACT_COSTEntity> CONTRACT_COST
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
    }
}
