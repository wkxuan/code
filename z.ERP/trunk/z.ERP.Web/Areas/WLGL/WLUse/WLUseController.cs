using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Edit;
using z.MVC5.Attributes;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.WLGL.WLUse
{
    public class WLUseController : BaseController
    {
        public ActionResult WLUseList()
        {
            ViewBag.Title = "物料领用单";
            return View();
        }
        public ActionResult WLUseEdit(string Id)
        {
            ViewBag.Title = "物料领用单信息编辑";

            return View("WLUseEdit", model: (EditRender)Id);
        }
        [Permission("10900501")]
        public void Delete(List<WLUSESEntity> DeleteData)
        {
            service.WyglService.WLDeleteWlUser(DeleteData);
        }
        [Permission("10900501")]
        public string Save(WLUSESEntity SaveData)
        {
            return service.WyglService.SaveWlUsers(SaveData);
        }

        [Permission("10900502")]
        public void ExecData(WLUSESEntity Data)
        {
            service.WyglService.ExecWlUses(Data);
        }


        public UIResult SearchWLUSES(WLUSESEntity Data)
        {
            var res = service.WyglService.GetWLUSESElement(Data);
            return new UIResult(
                new
                {
                    WLUSES = res.Item1,
                    WLUSESITEM = res.Item2
                }
            );
        }
    }
}