using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.Extensions;
using System;


namespace z.ERP.Web.Areas.XTGL.PAY
{
    public class PayController:BaseController
    {
        public ActionResult Pay()
        {
            return View();
        }

        public string  Save(PAYEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            if (DefineSave.PAYID.IsEmpty())
            {
                DefineSave.PAYID = service.CommonService.NewINC("PAY");
            }
            v.Require(a => a.NAME);
            //v.IsUnique(a => a.NAME);
            v.Require(a => a.TYPE);
            v.Require(a => a.JF);
            v.Require(a => a.FK);
            v.Require(a => a.ZLFS);
            v.Require(a => a.VOID_FLAG);
            v.IsNumber(a => a.FLAG);
            v.Verify();
            return  CommonSave(DefineSave);
        }

        public void Delete(PAYEntity DefineDelete)
        { 
            var v = GetVerify(DefineDelete);
            CommenDelete(DefineDelete);
        }
    }
}