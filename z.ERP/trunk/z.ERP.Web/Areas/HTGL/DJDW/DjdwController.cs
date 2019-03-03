using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using System.Collections.Generic;
using z.MVC5.Results;
using z.MVC5.Attributes;
using z.ERP.Web.Areas.Layout.Search;
using z.ERP.Web.Areas.Layout.EditDetail;

namespace z.ERP.Web.Areas.HTGL.DJDW
{
    public class DJDWController : BaseController
    {
        public ActionResult DjdwList()
        {
            ViewBag.Title = "多经点位租约列表信息";
            return View(new SearchRender()
            {
                Permission_Browse = "10600403",
                Permission_Add = "10600401",
                Permission_Edit = "10600401",
                Permission_Del = "10600401",
                Permission_Exec = "10600402"
            });
        }

        public ActionResult DjdwEdit(string Id)
        {
            ViewBag.Title = "多经点位租约信息编辑";
            return View("DjdwEdit", model: (EditRender)Id);
        }
        [Permission("10600401")]
        public string Save(CONTRACTEntity SaveData)
        {
            return service.HtglService.SaveContract(SaveData);
        }
        public ActionResult DjdwDetail(string Id)
        {
            ViewBag.Title = "多经点位租约浏览";
            var entity = service.HtglService.GetContractDjdwElement(new CONTRACTEntity(Id));
            ViewBag.contract = entity.Item1;
            ViewBag.contractShop = entity.Item2;
            ViewBag.contractCostDjdw = entity.Item3;
            return View();
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

        public void Delete(List<CONTRACTEntity> DeleteData)
        {
            service.HtglService.DeleteContract(DeleteData);
        }
        [Permission("10600402")]
        public void ExecData(CONTRACTEntity Data)
        {
            service.HtglService.ExecData(Data);
        }
    }
}