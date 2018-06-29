using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using  z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.Extensions;
using z.ERP.Web.Areas.Layout.Define;

namespace z.ERP.Web.Areas.XTGL.CONFIG
{
    public class ConfigController : BaseController
    {
        public ActionResult Config()
        {
            ViewBag.Title = "系统参数";
            return View(new DefineRender()
            {
              Permission_Mod = "10100101",
              Invisible_Add = true
            });
        }

        public string Save(CONFIGEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            if (DefineSave.ID.IsEmpty())
                DefineSave.ID = service.CommonService.NewINC("CONFIG");
            v.IsUnique(a => a.ID);
            v.Require(a => a.ID);
            v.Require(a => a.DEF_VAL);
            v.Require(a => a.CUR_VAL);
            v.Require(a => a.MAX_VAL);
            v.Require(a => a.MIN_VAL);
            v.Require(a => a.DESCRIPTION);
            v.Verify();
            return CommonSave(DefineSave);
        }
        public void Delete(CONFIGEntity DefineDelete)
        {
            throw new Exception("系统参数不能删除!");
            //这里应该改成系统参数界面删除按钮不显示
        }
    }
}