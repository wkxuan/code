using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.MVC5.Results;
using z.MVC5.Attributes;
using z.ERP.Web.Areas.Layout.Search;
using z.ERP.Web.Areas.Layout.EditDetail;

namespace z.ERP.Web.Areas.XTGL.ROLE
{
    public class RoleController: BaseController
    {
        public ActionResult RoleList()
        {
            ViewBag.Title = "角色信息";
            return View(new SearchRender()
            {
                Permission_Add = "10200701",
                Permission_Del = "10200701",
                Permission_Edit = "10100702",

            });
        }
        public ActionResult RoleEdit(string Id)
        {
            ViewBag.Title = "角色定义";
            return View("RoleEdit",model: (EditRender)Id);
        }


        public UIResult SearchRole(ROLEEntity Data)
        {
            var res = service.UserService.GetRoleElement(Data);
            return new UIResult(
                new
                {
                    role = res.Item1,
                    fee = res.Item2,
                    module = res.Item3,
                    ytTreeData = res.Item6,
                    region = res.Item5,
                    branch = res.Item7,
                    alert = res.Item4
                }
            );
        }


        [Permission("102007")]
        public string Save(ROLEEntity SaveData)
        {
            return service.UserService.SaveRole(SaveData);
        }

        public void Delete(ROLEEntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            CommenDelete(DefineDelete);
        }

        public UIResult SearchInit()
        {
            var res = service.UserService.GetRoleInit();
            return new UIResult(
                new
                {
                    treeOrg = res.Item1,
                    fee = res.Item2,
                    module = res.Item3,
                    ytTree = res.Item4,
                    region = res.Item5,
                    branch=res.Item6,
                    alert = res.Item7
                }
            );
        }

        public UIResult OrgInit()
        {
            var res = service.DataService.GetTreeOrg();
            return new UIResult(
                new
                {
                    treeOrg = res.Item1
                }
            );
        }
    }
}