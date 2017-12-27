using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.Extensions;
using System;
using System.Collections.Generic;

namespace z.ERP.Web.Areas.SHGL.MERCHANT
{
    public class MerchantController : BaseController
    {
        public ActionResult MerchantList()
        {
            return View();
        }

        public void Delete(List<MERCHANTEntity> DeleteData)
        {
            service.ShglService.DeleteMerchant(DeleteData);
        }
    }
}