using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using z.ERP.Entities;
using z.ERP.Model.Vue;
using z.ERP.Web.Areas.Base;

namespace z.ERP.Web.Areas.XTGL.CATEGORY
{
    public class CategoryController: BaseController
    {
        public ActionResult Category()
        {
            ViewBag.Tiele = "业态信息维护";
            return View();
        }
        public string Save(string Tar, string Key, CATEGORYEntity DefineSave)
        {
            var allenum = SelectList(new CATEGORYEntity());
            string newkey = TreeModel.GetNewKey(allenum, a => a.CATEGORYCODE, Key, Tar);
            if (DefineSave.CATEGORYID.IsEmpty())
            {
                DefineSave.CATEGORYID = service.CommonService.NewINC("CATEGORY");
            }
            DefineSave.CATEGORYCODE = newkey;
           // DefineSave.LEVEL_LAST = "1";
            CommonSave(DefineSave);
            return newkey;
        }
        public void Delete(CATEGORYEntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            v.Require(a => a.CATEGORYID);
            v.Verify();
            CommenDelete(DefineDelete);
        }
    }
}