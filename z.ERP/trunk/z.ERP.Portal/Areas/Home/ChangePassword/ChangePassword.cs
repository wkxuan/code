using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.SSO;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;

namespace z.ERP.Portal.Areas.Home.ChangePassword
{
    //注意继承了对应的内容才有对应的方法或者数据信息
    public class ChangePasswordController : BaseController
    {
        public ActionResult ChangePassword()
        {
            ViewBag.Title = "修改密码";
            return View();
        }

        public void ChangePsw(SYSUSEREntity data) {
            service.HomeService.ChangePs(data);
        }
    }
}