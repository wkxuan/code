using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.Extensions;
using System;
using System.Collections.Generic;
using z.MVC5.Results;
using z.ERP.Model;
using z.ERP.Entities.Enum;
using System.Data;
using z.MathTools;
using z.ERP.Web.Areas.Layout.Search;
using z.MVC5.Attributes;
using z.ERP.Web.Areas.Layout.EditDetail;

namespace z.ERP.Web.Areas.WLGL.WLCheck
{
    public class WLCheckController : BaseController
    {
        public ActionResult WLCheckList()
        {
            ViewBag.Title = "物料损溢单";
            return View(new SearchRender()
            {
                Permission_Browse = "10900603",
                Permission_Add = "10900601",
                Permission_Del = "10900601",
                Permission_Edit = "10900601",
                Permission_Exec = "10900602"
            });
        }
        public ActionResult WLCheckMx(string Id)
        {
            ViewBag.Title = "物料损溢单信息浏览";
            var entity = service.WyglService.GetWlCheckElement(new WLCHECKEntity(Id));
            ViewBag.data = entity.Item1;
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