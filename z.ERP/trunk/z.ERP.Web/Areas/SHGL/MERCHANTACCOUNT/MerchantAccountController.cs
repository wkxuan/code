using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;

namespace z.ERP.Web.Areas.SHGL.MERCHANTACCOUNT
{
    public class MerchantAccountController:BaseController
    {
        public ActionResult MerchantAccount() {
            return View();
        }
        public ActionResult MerchantAccountDetail()
        {
            return View();
        }
    }
}