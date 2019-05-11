using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities.Auto;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Define;

namespace z.ERP.Web.Areas.XTGL.POSUMSCONFIG
{
    public class POSUMSCONFIGController:BaseController
    {
        public ActionResult POSUMSCONFIG()
        {
            ViewBag.Title = "POS银联支付配置";
            return View(new DefineRender()
            {
                Permission_Add = "10500601",
                Permission_Mod = "10500602"
            });
        }

        public string Save(POSUMSCONFIGEntity DefineSave)
        {
            var v = GetVerify(DefineSave);

            v.IsUnique(a => a.POSNO);
            v.Require(a => a.IP);
            v.Require(a => a.IP_BAK);
            v.Require(a => a.PORT);
            v.Require(a => a.CFX_MCHTID);
            v.Require(a => a.CFX_TERMID);
            v.Require(a => a.CFXMPAY_MCHTNAME);
            v.Require(a => a.CFXMPAY_MCHTID);
            v.Require(a => a.CFXMPAY_TERMID);
            v.Verify();
            return CommonSave(DefineSave);
        }

        public void Delete(POSUMSCONFIGEntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            CommenDelete(DefineDelete);
        }
    }
}