using System;
using System.Collections.Generic;
using System.Data;
using z.ATR.Entities;
using z.WSTools.Txt;
using z.ATR._96262API;

namespace z.ATR.Services
{
    public class AutoService : ServicesBase
    {

        public void InsertYHDZJL()
        {
            try
            {
                ReportForm t = new ReportForm();

                string sqlsh = "SELECT YHSHBH,to_char(sysdate-2,'yyyymmdd') RQ FROM YHSHXX where STATUS=1";

                DataTable dt = DbHelper.ExecuteTable(sqlsh);
                string sDate, sSHBM;
                foreach(DataRow dr in dt.Rows)
                {
                    sSHBM = dr["YHSHBH"].ToString();
                    sDate = dr["RQ"].ToString();

                    List<YHDZJL> list = t.GetYHDZJL(sSHBM, sDate);

                    Log.Info("list", list);

                    using (var tran = DbHelper.BeginTransaction())
                    {
                        list.ForEach(l => DbHelper.Insert(l));
                        tran.Commit();
                    }
                }

                //  List<DataTable> list = t.GetYHDZJL("010200000000129", "20171201");


                //List<YHDZJL> list = t.GetYHDZJL("010100000000122", "20171201");

                //    Log.Info("list", list);

                //    using (var tran = DbHelper.BeginTransaction())
                //    {
                //        list.ForEach(l => DbHelper.Insert(l));
                //        tran.Commit();
                //    }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

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
    }
}

