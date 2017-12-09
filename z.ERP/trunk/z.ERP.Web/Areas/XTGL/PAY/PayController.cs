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

        public void Save(PAYEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            DefineSave.PAYID = service.CommonService.NewINC("PAY");

            if (DefineSave.CODE.IsEmpty()){
                DefineSave.CODE = DefineSave.PAYID;
            }
            if (DefineSave.VOID_FLAG.IsEmpty()) {
                DefineSave.VOID_FLAG = "0";
            }
            DefineSave.CREATE_TIME = DateTime.Now.ToLongString();

            DefineSave.UPDATE_TIME = DateTime.Now.ToLongString();
            v.Require(a => a.NAME);
            v.Require(a => a.TYPE);
            v.Require(a => a.JF);
            v.Require(a => a.FK);
            v.Require(a => a.ZLFS);
            v.IsNumber(a => a.FLAG);
            v.IsUnique(a => a.PAYID);
            v.IsUnique(a => a.CODE);
            v.Verify();
            CommonSave(DefineSave);
        }

        public void Delete(PAYEntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            CommenDelete(DefineDelete);
        }
    }
}