using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.Extensions;

namespace z.ERP.Web.Areas.XTGL.FKFS
{
    public class FkfsController:BaseController
    {
        public ActionResult Fkfs() {

            return View();
        }

        public void Save(FKFSEntity fkfs)
        {
            var v = GetVerify(fkfs);
            if (string.IsNullOrEmpty(fkfs.ID))                
                fkfs.ID = service.CommonService.NewINC("FKFS");
            v.Require(a => a.ID);
            v.Require(a => a.NAME);
            v.IsNumber(a => a.ID);
            v.IsUnique(a => a.ID);
            v.IsUnique(a => a.NAME);
            v.Verify();
            CommonSave(fkfs);
        }
        
    }
}