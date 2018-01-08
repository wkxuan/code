using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.Extensions;
using System;
using System.Collections.Generic;
using z.ERP.Model.Vue;
using System.Linq;

namespace z.ERP.Web.Areas.XTGL.PAY
{
    public class PayController : BaseController
    {
        public ActionResult Pay()
        {
            List<MENUEntity> p = SelectList(new MENUEntity());
            TreeModel[] tt = TreeModel.Create(p, a => a.MENUCODE, a => new TreeModel()
            {
                code = a.MENUCODE,
                title = a.MENUNAME
            }).ToArray();
            ViewBag.Title = "支付方式信息";
            return View();
        }

        public string Save(PAYEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            if (DefineSave.PAYID.IsEmpty())
            {
                DefineSave.PAYID = service.CommonService.NewINC("PAY");
            }
            v.Require(a => a.NAME);
            v.IsUnique(a => a.NAME);
            v.Require(a => a.TYPE);
            v.Require(a => a.JF);
            v.Require(a => a.FK);
            v.Require(a => a.ZLFS);
            v.Require(a => a.VOID_FLAG);
            v.IsNumber(a => a.FLAG);
            v.Verify();
            return CommonSave(DefineSave);
        }

        public void Delete(PAYEntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            //外键验证应该是在删除的时候使用
            //v.IsForeignKey<P1Entity>(a => a.NAME, b => b.F1);
            CommenDelete(DefineDelete);
        }
    }
}