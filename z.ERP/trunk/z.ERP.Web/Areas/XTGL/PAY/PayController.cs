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

        public void Save(PAYEntity pay)
        {
            pay = HttpExtension.GetRequestParam<PAYEntity>("saveParam");
            var v = GetVerify(pay);
            pay.PAYID = service.CommonService.NewINC("PAY");

            if (pay.CODE.IsEmpty()){
                pay.CODE = pay.PAYID;
            }
            pay.CREATE_TIME = DateTime.Now.ToLongString();
            pay.VOID_FLAG = "0";
            pay.FK = "0";
            pay.JF = "0";
            pay.ZLFS = "0";
            pay.FLAG = "0";
            CommonSave(pay);
        }
    }
}