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

namespace z.ERP.Web.Areas.WLGL.WLUse
{
    public class WLUseController : BaseController
    {
        public ActionResult WLUseList()
        {
            ViewBag.Title = "物料领用单";
            return View(new SearchRender()
            {
                Permission_Browse = "10900503",
                Permission_Add = "10900501",
                Permission_Del = "10900501",
                Permission_Edit = "10900501",
                Permission_Exec = "10900502"
            });
        }
        public ActionResult WLUseMx(string Id)
        {
            ViewBag.Title = "物料领用单信息浏览";
            var entity = service.WyglService.GetWlUsersElement(new WLUSESEntity(Id));
            ViewBag.data = entity.Item1;
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