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

        public void Save(FKFSEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            if (string.IsNullOrEmpty(DefineSave.ID))
                DefineSave.ID = service.CommonService.NewINC("FKFS");
            v.Require(a => a.ID);
            v.Require(a => a.NAME);
            v.IsNumber(a => a.ID);
            v.IsUnique(a => a.ID);
            v.IsUnique(a => a.NAME);
            v.Verify();
            CommonSave(DefineSave);
        }
        public void Delete(FKFSEntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            v.Require(a => a.ID);
            v.Verify();
            CommenDelete(DefineDelete);
        }
        
    }
}