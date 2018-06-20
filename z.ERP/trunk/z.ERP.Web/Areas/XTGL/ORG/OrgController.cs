using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using z.ERP.Entities;
using z.ERP.Entities.Enum;
using z.ERP.Model.Vue;
using z.ERP.Web.Areas.Base;
using z.Exceptions;

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

            //当前数据有下级,并且当前数据要修改为末级,那么提示不让修改保存成功
            if (((Tar == null)||(Tar==" "))&&(DefineSave.LEVEL_LAST == ((int)末级标记.末级).ToString()))
            {
                foreach (var data in allenum) {
                    if (data.ORGCODE.Length > DefineSave.ORGCODE.Length) {
                        var dataOrgcode = data.ORGCODE.Substring(0, DefineSave.ORGCODE.Length);
                        if ((data.ORGCODE != DefineSave.ORGCODE)
                            && (dataOrgcode == DefineSave.ORGCODE))
                        {
                            throw new LogicException("当前部门已经有下级数据不能修改为末级!");
                        }
                    }
                }
            }

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