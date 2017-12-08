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
            DefineSave.CREATE_TIME = DateTime.Now.ToLongString();
            DefineSave.VOID_FLAG = "0";
            DefineSave.FK = "0";
            DefineSave.JF = "0";
            DefineSave.ZLFS = "0";
            DefineSave.FLAG = "0";
            CommonSave(DefineSave);
        }
    }
}