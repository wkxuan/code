using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Edit;
using z.MVC5.Attributes;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.HTGL.DJDW
{
    public class DJDWController : BaseController
    {
        public ActionResult DjdwList()
        {
            ViewBag.Title = "多经点位租约列表信息";
            return View();
        }

        public ActionResult DjdwEdit(string Id)
        {
            ViewBag.Title = "多经点位租约信息编辑";
            return View("DjdwEdit", model: (EditRender)Id);
        }
        [Permission("10600501")]
        public string Save(CONTRACTEntity SaveData)
        {
            return service.HtglService.SaveContract(SaveData);
        }
        public void Delete(List<CONTRACTEntity> DeleteData)
        {
            service.HtglService.DeleteContract(DeleteData);
        }
        [Permission("10600502")]
        public void ExecData(CONTRACTEntity Data)
        {
            service.HtglService.ExecData(Data);
        }
        public UIResult SearchContract(CONTRACTEntity Data)
        {
            var res = service.HtglService.GetContractDjdwElement(Data);
            return new UIResult(
                new
                {
                    contract = res.Item1,
                    contractShop = res.Item2,
                    contractCostDjdw = res.Item3
                }
            );
        }
        public UIResult GetShop(SHOPEntity Data)
        {
            return new UIResult(service.DataService.GetShop(Data));
        }
        public UIResult GetFeeSubject(FEESUBJECTEntity Data)
        {
            return new UIResult(service.DataService.GetFeeSubject(Data));
        }
    }
}