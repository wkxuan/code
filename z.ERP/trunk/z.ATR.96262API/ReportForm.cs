using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using z.ATR._96262API.Model;
using z.Extensions;
using z.WSTools.Compress;
using z.WSTools.Txt;
using z.ATR.Entities;

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
                                { 1,"SHBH" },
                                { 2,"ZDBH" },
                                { 3,"YHKH" },
                                { 4,"JYJE" },
                                { 5,"SXF" },
                                { 6,"JYRQ" },
                                { 7,"JYSJ" },
                                { 8,"JYLBH" },
                                { 9,"JYLBM" },
                                { 10,"QFSJ" },
                                { 11,"JGM" },
                                { 12,"ZFFS" },
                                { 13,"LSBH" }
                            }
                        }));
                    });
                });
            }
            return ret;
        }


        public List<YHDZJL> GetYHDZJL(string merchant_no, string chkDate)
        {
            List<YHDZJL> ret = new List<YHDZJL>();
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
                IOExtension.GetTempPathAndDo(path =>
                {
                    CompressBase cm = new ZipCompress();
                    cm.DeCompression(st, path);
                    IOExtension.GetAllFiles(path).ForEach(file =>
                    {
                        ret = TxtReader.ReadToModel<YHDZJL>(new TableReaderSettings()
                        {
                            ColumnSplit = new string[] { "|" },
                            FilePath = file.FullName,
                            RowSplit = new string[] { "\n" },
                            RowSettings = new Dictionary<int, string>()   //这里处理格式
                            {
                                { 1,"SHBH" },
                                { 2,"ZDBH" },
                                { 3,"YHKH" },
                                { 4,"JYJE" },
                                { 5,"SXF" },
                                { 6,"JYRQ" },
                                { 7,"JYSJ" },
                                { 8,"JYLBH" },
                                { 9,"JYLBM" },
                                { 10,"QFSJ" },
                                { 11,"JGM" },
                                { 12,"ZFFS" },
                                { 13,"LSBH" }
                            }
                        });
                    });
                });
            }
            return ret;
        }
    }
}
