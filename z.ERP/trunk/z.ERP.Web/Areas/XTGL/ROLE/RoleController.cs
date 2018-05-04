using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.XTGL.ROLE
{
    public class RoleController: BaseController
    {
        public ActionResult RoleList()
        {
            ViewBag.Title = "用户信息";
            return View();
        }
        public ActionResult RoleDetail(string Id)
        {
            ViewBag.Title = "角色定义";
            var entity = service.UserService.GetRoleElement(new ROLEEntity(Id));
            ViewBag.role = entity.Item1;
            ViewBag.menu = entity.Item2;
            ViewBag.fee = entity.Item3;
            return View(entity);
        }
        public ActionResult RoleEdit(string Id)
        {
            ViewBag.Title = "角色定义";
            return View("RoleEdit",model: Id);
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