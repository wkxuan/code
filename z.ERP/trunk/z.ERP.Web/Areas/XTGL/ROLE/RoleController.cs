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
        public ActionResult Role()
        {
            ViewBag.Title = "用户信息";
            return View();
        }

        public string Save(ROLEEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            if (DefineSave.ROLEID.IsEmpty())
            {
                DefineSave.ROLEID = service.CommonService.NewINC("ROLE");
            }
            v.IsUnique(a => a.ROLEID);
            v.IsUnique(a => a.ROLECODE);
            v.Require(a => a.ROLENAME);
            v.Require(a => a.ORGID);
            v.Verify();
            return CommonSave(DefineSave);
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
                    sfxm = res.Item3
                }
                );
        }
    }
}