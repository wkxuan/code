using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.Extensions;

namespace z.WSTools.Txt
{
    public class JsonReaderSettings : TxtReaderSettings
    {
        public override DataTable ReadTable()
        {
            string str = ReadText();
            return str.ToObj<DataTable>();
        }
    }
}
