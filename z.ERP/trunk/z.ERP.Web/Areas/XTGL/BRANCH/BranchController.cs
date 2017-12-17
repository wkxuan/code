using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.Extensions;
using System;

namespace z.ERP.Web.Areas.XTGL.BRANCH
{
    public class BranchController: BaseController
    {
        public ActionResult Branch()
        {
            return View();
        }


        public string Save(BRANCHEntity DefineSave)
        {
            var v = GetVerify(DefineSave);

            //当ID是界面输入的话怎么判断这个创建时间
            if (DefineSave.ID.IsEmpty())
            {
                DefineSave.ID = service.CommonService.NewINC("BRANCH");

            }


            v.IsUnique(a => a.ID);
            v.Require(a => a.NAME);
            v.IsUnique(a => a.NAME);
            v.Require(a => a.ORGID);
            v.Require(a => a.AREA_BUILD);
            v.Require(a => a.STATUS);
            v.Verify();
            return CommonSave(DefineSave);
            //这里返回主键
        }

        public void Delete(BRANCHEntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            CommenDelete(DefineDelete);
        }
    }
}