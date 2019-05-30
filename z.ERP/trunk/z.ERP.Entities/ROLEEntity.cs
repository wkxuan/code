using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ERP.Entities
{
    public partial class ROLEEntity
    {
        [ForeignKey(nameof(ROLEID), nameof(ROLE_MENUEntity.ROLEID))]
        public List<ROLE_MENUEntity> ROLE_MENU
        {
            get;
            set;
        }
        [ForeignKey(nameof(ROLEID), nameof(ROLE_FEEEntity.ROLEID))]
        public List<ROLE_FEEEntity> ROLE_FEE
        {
            get;
            set;
        }

        [ForeignKey(nameof(ROLEID), nameof(ROLE_FEEEntity.ROLEID))]
        public List<ROLE_REGIONEntity> ROLE_REGION
        {
            get;
            set;
        }

        [ForeignKey(nameof(ROLEID), nameof(ROLE_FEEEntity.ROLEID))]
        public List<ROLE_YTEntity> ROLE_YT
        {
            get;
            set;
        }
    }
}
