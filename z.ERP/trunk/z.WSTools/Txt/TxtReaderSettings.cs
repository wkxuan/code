using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.Extensions;

namespace z.WSTools.Txt
{
    public abstract class TxtReaderSettings
    {
        public TxtReaderSettings()
        {
            Encoding = Encoding.Default;
        }

        public string FilePath
        {
            get;
            set;
        }
        public Encoding Encoding
        {
            get;
            set;
        }

        public abstract DataTable ReadTable();

        protected string ReadText()
        {
            if (FilePath.IsEmpty())
                throw new Exception("地址无效");
            return File.ReadAllText(FilePath, Encoding);
        }

    }
}
