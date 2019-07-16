using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Entities.Auto;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.XTGL.NOTICE
{
    public class NOTICEController:BaseController
    {
        public ActionResult NOTICE()
        {
            return View();
        }
        [ValidateInput(false)]
        public string Save(NOTICEEntity DefineSave)
        {
            return service.XtglService.SaveNotice(DefineSave);
        }
        public void Delete(NOTICEEntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            v.Require(a => a.ID);
            v.Verify();
            CommenDelete(DefineDelete);
        }
        public UIResult SearchNOTICE(NOTICEEntity Data)
        {
            var res = service.XtglService.GetNOTICEElement(Data);
            return new UIResult(
                new
                {
                    notice = res.Item1,
                    branch = res.Item2
                }
            );
        }
        public UIResult GetBRANCH() {
            return new UIResult(service.DataService.branch());
        }
    }
}