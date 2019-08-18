using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.WSTools.Compress
{
    public class ZipCompress : CompressBase
    {
        public ZipCompress()
        {
        }

        public override void Compression(string path)
        {
            throw new NotImplementedException();
        }

        public override void DeCompression(Stream from, string zipedFolder, string password = null)
        {
            FileStream fs = null;
            ZipInputStream zipStream = null;
            ZipEntry ent = null;
            string fileName;
            if (!Directory.Exists(zipedFolder))
                Directory.CreateDirectory(zipedFolder);
            try
            {
                zipStream = new ZipInputStream(from);
                if (!string.IsNullOrEmpty(password))
                    zipStream.Password = password;
                while ((ent = zipStream.GetNextEntry()) != null)
                {
                    if (!string.IsNullOrEmpty(ent.Name))
                    {
                        fileName = Path.Combine(zipedFolder, ent.Name);
                        fileName = fileName.Replace('/', '\\');
                        string dic = Path.GetDirectoryName(fileName);
                        if (!Directory.Exists(dic))
                        {
                            Directory.CreateDirectory(dic);
                        }
                        fs = File.Create(fileName);
                        int size = 2048;
                        byte[] data = new byte[size];
                        while (true)
                        {
                            size = zipStream.Read(data, 0, data.Length);
                            if (size > 0)
                                fs.Write(data, 0, data.Length);
                            else
                                break;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
                if (zipStream != null)
                {
                    zipStream.Close();
                    zipStream.Dispose();
                }
                if (ent != null)
                {
                    ent = null;
                }
                GC.Collect();
                GC.Collect(1);
            }
        }

    }
}
