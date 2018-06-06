using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using z.ERP.Entities;
using z.ERP.Model.Vue;
using z.ERP.Web.Areas.Base;

namespace z.ERP.Web.Areas.XTGL.GOODS_KIND
{
    public class Goods_KindController: BaseController
    {
        public ActionResult Goods_Kind()
        {
            ViewBag.Tiele = "商品分类维护";
            return View();
        }
        public string Save(string Tar, string Key, GOODS_KINDEntity DefineSave)
        {
            var allenum = SelectList(new GOODS_KINDEntity());
            string newkey = TreeModel.GetNewKey(allenum, a => a.CODE, Key, Tar);
            if (DefineSave.ID.IsEmpty())
            {
                DefineSave.ID = service.CommonService.NewINC("GOODS_KIND");
            }
            DefineSave.CODE = newkey;
            CommonSave(DefineSave);
            return newkey;
        }
        public void Delete(GOODS_KINDEntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            v.Require(a => a.ID);
            v.Verify();
            CommenDelete(DefineDelete);
        }
    }
}