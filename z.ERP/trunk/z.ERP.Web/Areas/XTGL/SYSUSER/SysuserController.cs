using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.Extensions;
using System;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.XTGL.SYSUSER
{
    public class SysuserController: BaseController
    {
        public ActionResult Sysuser()
        {
            ViewBag.Title = "用户信息";
            return View();
        }

        public string Save(SYSUSEREntity DefineSave)
        {
            return service.HomeService.SaveSysUser(DefineSave);
        }



        public void Delete(SYSUSEREntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            CommenDelete(DefineDelete);
        }
        public UIResult SearchUser(SYSUSEREntity Data)
        {
            var res = service.UserService.GetUserElement(Data);
            return new UIResult(
                new
                {
                    user = res.Item1,
                    userrole = res.Item2
                }
                );
        }
        public UIResult SearchInit()
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