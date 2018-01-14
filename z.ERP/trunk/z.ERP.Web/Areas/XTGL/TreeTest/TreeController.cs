using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Model.Vue;
using z.ERP.Web.Areas.Base;

namespace z.ERP.Web.Areas.XTGL.TreeTest
{
    public class TreeTestController : BaseController
    {
        // GET: XTGL/Tree
        public ActionResult List()
        {
            return View();
        }

        public string Save(string Tar, string Key, MENUEntity DefineSave)
        {
            var allenum = SelectList(new MENUEntity());
            string newkey = TreeModel.GetNewKey(allenum, a => a.MENUCODE, Key, Tar);
            DefineSave.MENUCODE = newkey;

            DefineSave.ENABLE_FLAG = "1";
            CommonSave(DefineSave);
            return newkey;
        }
    }
}