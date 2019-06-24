using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.WSTools.Txt
{
    public static class TxtWriter
    {
        public static void Write(string path, string Txt, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.Default;
            File.WriteAllText(path, Txt, encoding);
        }
    }
}
