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

        public void WriteYHDZJL()
        {
            //先处理上次未成功的数据
            DataTable dt = DbHelper.ExecuteTable("select to_char(RQ,'yyyymmdd') RQ,YHSHBH from YHDZLOG where CLZT = 2");
            string sDate, sSHBH;
            foreach(DataRow dr in dt.Rows)
            {
                sSHBH = dr["YHSHBH"].ToString();
                sDate = dr["RQ"].ToString();

                try
                {
                    InsertYHDZJL(sSHBH, sDate);
                    DbHelper.ExecuteNonQuery($"update YHDZLOG set CLZT=1,CLSJ=sysdate where RQ=to_date('{sDate}','yyyymmdd') and YHSHBH ='{sSHBH}'");

                }
                catch (Exception ex)
                {
                    DbHelper.ExecuteNonQuery($"update YHDZLOG set CLZT=3,CLSJ=sysdate where RQ=to_date('{sDate}','yyyymmdd') and YHSHBH ='{sSHBH}'");
                    Log.Error("写YHDZJL异常", ex.Message);
                }
                }


            //处理本次数据
            dt = DbHelper.ExecuteTable("select YHSHBH,to_char(sysdate-10,'yyyymmdd') RQ from YHSHXX where STATUS=1");
            foreach (DataRow dr in dt.Rows)
            {
                sSHBH = dr["YHSHBH"].ToString();
                sDate = dr["RQ"].ToString();

                try
                {
                    InsertYHDZJL(sSHBH, sDate);
                    DbHelper.ExecuteNonQuery($"insert into YHDZLOG(RQ,YHSHBH,CLZT) values(to_date('{sDate}','yyyymmdd'),'{sSHBH}',1)");


                }
                catch (Exception ex)
                {
                    DbHelper.ExecuteNonQuery($"insert into YHDZLOG(RQ,YHSHBH,CLZT) values(to_date('{sDate}','yyyymmdd'),'{sSHBH}',2)");
                    Log.Error("写YHDZJL异常", ex.Message);
            }
            }


        }

        public void InsertYHDZJL(string shbh,string date)
        {
            ReportForm t = new ReportForm();

            List<YHDZJL> list = t.GetYHDZJL(shbh, date);

            Log.Info("list", list);

            using (var tran = DbHelper.BeginTransaction())
            {
                list.ForEach(l => DbHelper.Insert(l));
                tran.Commit();
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

