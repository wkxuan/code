using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Edit;
using z.MVC5.Attributes;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.WLGL.WLCheck
{
    public class WLCheckController : BaseController
    {
        public ActionResult WLCheckList()
        {
            ViewBag.Title = "物料损溢单";
            return View();
        }
        public ActionResult WLCheckEdit(string Id)
        {
            ViewBag.Title = "物料损溢单信息编辑";

            return View("WLCheckEdit", model: (EditRender)Id);

        }


        [Permission("10900601")]
        public void Delete(List<WLCHECKEntity> DeleteData)
        {
            service.WyglService.WLDeleteWLCheck(DeleteData);
        }

        [Permission("10900601")]
        public string Save(WLCHECKEntity SaveData)
        {
            return service.WyglService.SaveWLCheck(SaveData);
        }

        [Permission("10900602")]
        public void ExecData(WLCHECKEntity Data)
        {
            service.WyglService.ExecWLCheck(Data);
        }


        public UIResult SearchWLCHECKE(WLCHECKEntity Data)
        {
            var res = service.WyglService.GetWlCheckElement(Data);
            return new UIResult(
                new
                {
                    WLCHECK = res.Item1,
                    WLCHECKITEM = res.Item2
                }
            );
        }
    }
}