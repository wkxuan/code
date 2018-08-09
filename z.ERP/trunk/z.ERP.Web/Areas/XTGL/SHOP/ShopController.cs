using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.Extensions;
using System;
using z.MVC5.Results;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Define;

namespace z.ERP.Web.Areas.XTGL.SHOP
{
    public class ShopController: BaseController
    {
        public ActionResult Shop()
        {
            ViewBag.Title = "资产单元信息";
            return View(new DefineRender()
            {
                Permission_Chk = "104004"
            });
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
            v.Require(a => a.AREA_RENTABLE);
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
        public string Check(SHOPEntity DefineSave)
        {
            DefineSave.STATUS = "2";
            var v = GetVerify(DefineSave);
            v.IsUnique(a => a.SHOPID);
            v.IsUnique(a => a.CODE);
            v.Require(a => a.NAME);
            v.Require(a => a.BRANCHID);
            v.Require(a => a.FLOORID);
            v.Require(a => a.ORGID);
            v.Require(a => a.CATEGORYID);
            v.Require(a => a.TYPE);
            v.Require(a => a.AREA_RENTABLE);
            v.Require(a => a.STATUS);
            v.Require(a => a.RENT_STATUS);
            v.Verify();
            return CommonSave(DefineSave);
        }
        public UIResult GetBranch(BRANCHEntity Data)
        {
            return new UIResult(service.DataService.GetBranch(Data));
        }
        public UIResult GetFloor(FLOOREntity Data)
        {
            return new UIResult(service.DataService.GetFloor(Data));
        }
        public UIResult SearchInit()
        {
            var resOrg = service.DataService.GetTreeOrg();
            var resCategory = service.DataService.GetTreeCategory();
            return new UIResult(
                new
                {
                    treeOrg = resOrg.Item1,
                    treeCategory = resCategory.Item1
                }
            );
        }
        public UIResult GetShop(SHOPEntity Data)
        {
            var res = service.DpglService.GetShop(Data);
            return new UIResult(
                new
                {
                    shopelement = res.Item1
                }
                );
        }
    }
}