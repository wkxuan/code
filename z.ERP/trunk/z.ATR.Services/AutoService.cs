using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.ATR.Entities;
using z.WSTools.Txt;

namespace z.ATR.Services
{
    public class AutoService : ServicesBase
    {
        public void Test()
        {
            List<TextModel> list = TxtReader.ReadToModel<TextModel>(new TableReaderSettings()
            {
                FilePath = $@"C:\Users\zgy\Desktop\新建文件夹\ttt.txt",
                ColumnSplit = new string[] { "|" },
                RowSplit = new string[] { "\r\n" },
                RowSettings = new Dictionary<int, string>()
                {
                    { 1,"a" },
                    { 2,"b" },
                    { 3,"c" }
                }
            });
            //using (var tran = DbHelper.BeginTransaction())
            //{
            //    list.ForEach(l => DbHelper.Insert(l));
            //    tran.Commit();
            //}
        }
    }
}
