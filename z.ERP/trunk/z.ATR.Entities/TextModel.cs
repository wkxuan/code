using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.DBHelper.DBDomain;

namespace z.ATR.Entities
{
    [DbTable("TextModel", "")]
    public class TextModel : TableEntityBase
    {
        public string a
        {
            get;
            set;
        }

        public int b
        {
            get;
            set;
        }
        public DateTime c
        {
            get;
            set;
        }

    }
}
