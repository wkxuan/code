using Microsoft.Office.Interop.Excel;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;
using System.Collections.Generic;
namespace z.ERP.Web.Areas.Report.MerchantPayable
{
    public class MerchantPayableController : BaseController
    {
        public ActionResult MerchantPayable()
        {
            ViewBag.Title = "商户应交已付报表";
            return View();
        }
        //获取收费项目list
        public UIResult GetSfxmList()
        {
            var res = service.ReportService.GetSfxmList();
            return new UIResult(
                new
                {
                  res
                }
            );
        }
        public void outputExcel(string filename, List<tableCols> cols, SearchItem item)
        {
            Application excel = new Application();
            //设置工作表的个数
            excel.SheetsInNewWorkbook = 1;
            //创建Wprkbook
            excel.Workbooks.Add();
            //取出第一个工作表
            Worksheet sheet1 = (Worksheet)excel.ActiveWorkbook.Worksheets[1];
            sheet1.Name = "data";
            var row = 1;
            var data = service.ReportService.getData(item);
            for (var i = 0; i < cols.Count; i++)
            {
                var col = i + 1;
                sheet1.Cells[row, col] = cols[i].name;
                while(row< data.Rows.Count)
                {
                    sheet1.Cells[row, col] = data.Rows[row - 1][cols[i].key].ToString();
                    row++;
                }
            }
            //显示excel
            excel.Visible = true;
        }
        public string Output(SearchItem item)
        {
            return service.ReportService.MerchantPayableOutput(item);
        }
    }
    public class tableCols
    {
        public string key
        {
            set;
            get;
        }
        public string name
        {
            set;
            get;
        }
    }
}