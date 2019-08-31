using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using z.ATR._96262API.Model;
using z.Extensions;
using z.WSTools.Compress;
using z.WSTools.Txt;

namespace z.ATR._96262API
{

    public class ReportForm : Base
    {
        readonly string Url = "statement/ReportForm/";
        public List<DataTable> Get(string merchant_no, string chkDate)
        {
            List<DataTable> ret = new List<DataTable>();
            ReportFormResponse t = new ReportFormResponse()
            {
                merchant_no = merchant_no,
                chkDate = chkDate
            };
            ReportFormRequest res = Post<ReportFormResponse, ReportFormRequest>(Url, t);

            if (res.return_code != "00")
                throw new Exception(res.return_msg);
            using (Stream st = new MemoryStream(Convert.FromBase64String(res.fileData)))
            {
                File.WriteAllBytes(@"C:\Users\zgy\Desktop\新建文件夹\" + res.fileName, Convert.FromBase64String(res.fileData));
                IOExtension.GetTempPathAndDo(path =>
                {
                    CompressBase cm = new ZipCompress();
                    cm.DeCompression(st, path);
                    IOExtension.GetAllFiles(path).ForEach(file =>
                    {
                        ret.Add(TxtReader.ReadToDatatable(new TableReaderSettings()
                        {
                            ColumnSplit = new string[] { "|" },
                            FilePath = file.FullName,
                            RowSplit = new string[] { "\n" },
                            RowSettings = new Dictionary<int, string>()   //这里处理格式
                            {
                                { 1,"a" },
                                { 2,"b" },
                                { 3,"c" },
                                { 4,"d" },
                                { 5,"e" },
                                { 6,"f" },
                                { 7,"g" },
                                { 8,"h" },
                                { 9,"i" },
                                { 10,"j" }
                            }
                        }));
                    });
                });
            }
            return ret;
        }
    }
}
