using System.Web.Mvc;
using z.ERP.Web.Areas.Base;

namespace z.ERP.Web.Areas.Report.ContractSale
{
    public class ContractSaleController : BaseController
    {
        public ActionResult ContractSale()
        {
            ViewBag.Title = "租约销售查询";
            return View();
        }
    }
}