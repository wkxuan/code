using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities.Enum;
using z.ERP.Web.Areas.Base;
using z.Extensions;

namespace z.ERP.Web.Areas.HTGL.ZLHT
{
    public class ZLHTController : BaseController
    {
        // GET: HTGL/ZLHT
        public ActionResult List()
        {
            合同类型.租赁合同.ToLocalString();
            return View();
        }


    }
}