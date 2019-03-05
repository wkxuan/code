using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.POS.Entities.Pos
{
    public class FindGoodsFilter
    {
        public string goodscode
        {
            get;
            set;
        }
        public int? shopid
        {
            get;
            set;
        }

    }
}
