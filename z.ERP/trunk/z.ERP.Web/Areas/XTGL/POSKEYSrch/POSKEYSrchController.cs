using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.XTGL.POSKEYSrch
{
    public class POSKEYSrchController : BaseController
    {
        public ActionResult POSKEYSrch()
        {
            ViewBag.Title = "终端密钥查询";
            return View();
        }
        

        public string Output(string Name, Dictionary<string, string> Cols, SearchItem item)
        {
            var dtSource = service.XtglService.POSKEYSrchOutput(item);
            return NPOIHelper.ExportExcel(dtSource, Name, Cols);
        }

    }
}