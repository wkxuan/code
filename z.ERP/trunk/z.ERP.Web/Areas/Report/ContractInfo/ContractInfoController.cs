using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.Report.ContractInfo
{
    public class ContractInfoController : BaseController
    {
        public ActionResult ContractInfo()
        {
            ViewBag.Title = "合同信息表";
            return View();
        }

        public UIResult SearchCate()
        {
            var res = service.DataService.GetTreeCategory();
            return new UIResult(
                new
                {
                    treeOrg = res.Item1
                }
            );
        }
        public string Output(string Name, Dictionary<string, string> Cols, SearchItem item)
        {
            var dtSource = service.ReportService.ContractInfoOutput(item);
            var dt = service.ReportService.SEARCHFEE();
            if (dt.Rows.Count>0) {
                foreach (DataRow dts in dt.Rows) {
                    Cols.Add(dts["PYM"].ToString(), dts["NAME"].ToString());
                }
            }
            return NPOIHelper.ExportExcel(dtSource, Name, Cols);
        }
        public UIResult SearchFEE()
        {
            return new UIResult(service.ReportService.SEARCHFEE());
        }
    }
}