using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.MVC5.Results;
using z.ERP.Web.Areas.Layout.Define;

namespace z.ERP.Web.Areas.XTGL.SYSUSER
{
    public class SysuserController: BaseController
    {
        public ActionResult Sysuser()
        {
            ViewBag.Title = "用户信息";
            return View(new DefineRender()
            {
                Permission_Add = "10100601",
                Permission_Mod = "10100602" 
            });
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
    }
}