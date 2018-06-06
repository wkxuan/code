using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DbHelper.DbDomain;

namespace z.ERP.Entities
{
    public partial class SYSUSEREntity
    {
        [ForeignKey(nameof(USERID), nameof(USER_ROLEEntity.USERID))]
        public List<USER_ROLEEntity> USER_ROLE
        {
            get;
            set;
        }

    }
}
