using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using z.ERP.Entities;
using z.ERP.Model.Vue;
using z.ERP.Web.Areas.Base;

namespace z.ERP.Web.Areas.XTGL.ORG
{
    public class OrgController: BaseController
    {
        public ActionResult Org()
        {
            ViewBag.Tiele = "组织架构维护";
            return View();
        }

        public string Save(string Tar, string Key, ORGEntity DefineSave)
        {
            var allenum = SelectList(new ORGEntity());
            string newkey = TreeModel.GetNewKey(allenum, a => a.ORGCODE, Key, Tar);

            if (DefineSave.ORGID.IsEmpty())
            {
                DefineSave.ORGID = service.CommonService.NewINC("ORG");
            }
            var v = GetVerify(DefineSave);

            v.Require(a => a.ORGNAME);
            v.IsUnique(a => a.ORGNAME);
            v.Require(a => a.ORG_TYPE);
            v.Require(a => a.LEVEL_LAST);
            v.Require(a => a.VOID_FLAG);
            v.Verify();
            if (!((Key.Length == 2) && (Tar == "tj")))  {
                DefineSave.BRANCHID = service.XtglService.Org_BRANCHID(Key);
            }
            DefineSave.ORGCODE = newkey;
            CommonSave(DefineSave);
            return newkey;
        }
        public void Delete(ORGEntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            v.Require(a => a.ORGID);
            v.Verify();
            CommenDelete(DefineDelete);
        }
    }
}