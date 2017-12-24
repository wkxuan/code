using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.Extensions;
using System;

namespace z.ERP.Web.Areas.XTGL.SHOP
{
    public class FloorController: BaseController
    {
        public ActionResult Shop()
        {
            return View();
        }

        public string Save(SHOPEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            if (DefineSave.SHOPID.IsEmpty())
            {
                DefineSave.SHOPID = service.CommonService.NewINC("SHOP");
            }
            v.IsUnique(a => a.SHOPID);
            v.IsUnique(a => a.CODE);
            v.Require(a => a.NAME);
            v.Require(a => a.BRANCHID);
            v.Require(a => a.FLOORID);
            v.Require(a => a.ORGID);
            v.Require(a => a.CATEGORYID);
            v.Require(a => a.TYPE);
            v.Require(a => a.AREA_BUILD);
            v.Require(a => a.STATUS);
            v.Require(a => a.RENT_STATUS);
            v.Verify();
            return CommonSave(DefineSave);
        }

        public void Delete(SHOPEntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            CommenDelete(DefineDelete);
        }
    }
}