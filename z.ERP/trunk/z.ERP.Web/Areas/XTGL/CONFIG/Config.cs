using System;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.DefineDetail;
using z.Extensions;

namespace z.ERP.Web.Areas.XTGL.CONFIG
{
    public class ConfigController : BaseController
    {
        public ActionResult Config()
        {
            ViewBag.Title = "系统参数列表";
            return View();
        }
        public ActionResult ConfigDetail(string Id)
        {
            ViewBag.Title = "系统参数信息";
            return View("ConfigDetail", model: (DefineDetailRender)Id);
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