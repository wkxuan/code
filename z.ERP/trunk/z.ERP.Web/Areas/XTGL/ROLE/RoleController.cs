using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
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

namespace z.ERP.Web.Areas.XTGL.ROLE
{
    public class RoleController: BaseController
    {
        public ActionResult RoleList()
        {
            ViewBag.Title = "用户信息";
            return View();
        }
        public ActionResult Detail(string Id)
        {
            ViewBag.Title = "店铺拆分浏览";
            var entity = service.DpglService.GetAssetChangeElement(new ASSETCHANGEEntity(Id));
            ViewBag.assetchange = entity.Item1;
            ViewBag.assetchangeitem = entity.Item2;
            ViewBag.assetchangeitem2 = entity.Item3;
            return View(entity);
        }
        public ActionResult RoleEdit(string Id)
        {
            ViewBag.Title = "角色定义";
            return View(model: Id);
        }
        public string Save(ROLEEntity SaveData)
        {
            return service.UserService.SaveRole(SaveData,"MODULECODE");
        }

        public void Delete(ROLEEntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            CommenDelete(DefineDelete);
        }

        public UIResult SearchRole(ROLEEntity Data)
        {
            var res = service.UserService.GetRoleElement(Data);
            return new UIResult(
                new
                {
                    role = res.Item1,
                    menu = res.Item2,
                    fee = res.Item3
                }
                );
        }
    }
}