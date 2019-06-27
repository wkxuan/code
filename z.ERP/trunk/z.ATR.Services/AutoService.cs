using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.ATR.Entities;
using z.WSTools.Txt;
using z.Extensions;

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
            Log.Info("list", list);
            

            DataTable dt = TxtReader.ReadToDatatable(new TableReaderSettings()
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
            Log.Info("DataTable", dt);
            //using (var tran = DbHelper.BeginTransaction())
            //{
            //    list.ForEach(l => DbHelper.Insert(l));
            //    tran.Commit();
            //}
        }


        public void insertYHDZXX()
        {
            string filePath  = ConfigExtension.GetConfig("FilePath").ToString();

            List<YHDZXX> list = TxtReader.ReadToModel<YHDZXX>(new TableReaderSettings()
            {
                FilePath = filePath,
                ColumnSplit = new string[] { "|" },
                RowSplit = new string[] { "\r\n" },
                RowSettings = new Dictionary<int, string>()
                {
                    { 1,"SHBM" },
                    { 2,"ZDBM" },
                    { 3,"ZDLSH" },
                    { 4,"JYLX" },
                    { 5,"JYCD" },
                    { 6,"DDBH" },
                    { 7,"JYRQ"},
                    { 8,"JYSJ" },
                    { 9,"JYJE" },
                    { 10,"YHKH" },
                    { 11,"JSZH" },
                    { 12,"SXF" }
                }
            });

            Log.Info("DataTable", list);


            using (var tran = DbHelper.BeginTransaction())
            {
                list.ForEach(l => DbHelper.Insert(l));
                tran.Commit();
            }
        }
    }
}
