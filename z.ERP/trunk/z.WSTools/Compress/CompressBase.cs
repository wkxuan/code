using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.Extensions;

namespace z.WSTools.Compress
{
    public abstract class CompressBase
    {
        protected string _path;
        public static CompressBase Create(string path)
        {
            if (path.IsEmpty())
                throw new Exception($"没有文件路径");
            string filename = Path.GetFileName(path);
            if (filename.IsEmpty())
                throw new Exception($"找不到文件{path}");
            string e = Path.GetExtension(path);
            if (e.IsEmpty())
                throw new Exception($"找不到文件{path}的扩展名");
            switch (e.ToUpper())
            {
                case "ZIP":
                    return new ZipCompress();
                default:
                    throw new Exception($"不支持对{e}文件进行操作");
            }
        }
        public CompressBase()
        {

        }

        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="path"></param>
        public abstract void Compression(string path);
        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="to"></param>
        public virtual void DeCompression(string from, string to, string password = null)
        {
            if (!File.Exists(from))
                throw new Exception($"找不到文件{from}");
            Stream steam = File.OpenRead(from);
            DeCompression(steam, to, password);
        }
        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="to"></param>
        public abstract void DeCompression(Stream from, string to, string password = null);
    }
}
