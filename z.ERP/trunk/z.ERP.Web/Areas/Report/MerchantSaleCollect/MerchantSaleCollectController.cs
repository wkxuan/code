using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.Report.MerchantSaleCollect
{
    public class MerchantSaleCollectController: BaseController
    {
        public ActionResult MerchantSaleCollect()
        {
            ViewBag.Title = "商户销售汇总";
            return View();
        }
        public UIResult GetPay(SearchItem item)
        {
            return new UIResult(service.ReportService.GetPayData(item));
        }
        public string Output(string Name, Dictionary<string, string> Cols, SearchItem item)
        {
            var dtSource = service.ReportService.MerchantSaleCollectOutput(item);
            var dt = service.ReportService.GetPayData(item);
            if (dt.Rows.Count > 0)
            {
                Cols.Remove("SUMPAY");
                foreach (DataRow dts in dt.Rows)
                {
                    Cols.Add("PAYID"+dts["PAYID"].ToString(), dts["NAME"].ToString());
                }
                Cols.Add("SUMPAY", "总计金额");
            }
            return NPOIHelper.ExportExcel(dtSource, Name, Cols);
        }
    }
}