using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Entities.Enum;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.DefineDetail;
using z.Exceptions;
using z.Extensions;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.XTGL.SHOP
{
    public class ShopController : BaseController
    {
        public ActionResult Shop()
        {
            ViewBag.Title = "资产单元信息";
            return View();
        }
        public ActionResult ShopDetail(string Id)
        {
            ViewBag.Title = "资产单元信息";
            return View("ShopDetail", model: (DefineDetailRender)Id);
        }
        public string Save(SHOPEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            if (DefineSave.SHOPID.IsEmpty())
            {
                DefineSave.SHOPID = service.CommonService.NewINC("SHOP");

                DefineSave.STATUS = ((int)单元状态.正常).ToString();

                DefineSave.RENT_STATUS = ((int)租用状态.空置).ToString();
            }
            else
            {
                var shop = new SHOPEntity();
                shop = service.XtglService.SelectShop(DefineSave.SHOPID);
                if (shop.RENT_STATUS == ((int)租用状态.出租).ToString())
                {
                    if (shop.AREA_RENTABLE != DefineSave.AREA_RENTABLE)
                    {
                        throw new LogicException($"出租状态不能修改租赁面积!");
                    }

                    if (shop.AREA_USABLE != DefineSave.AREA_USABLE)
                    {
                        throw new LogicException($"出租状态不能修改使用面积!");
                    }

                    if (shop.AREA_BUILD != DefineSave.AREA_BUILD)
                    {
                        throw new LogicException($"出租状态不能修改建筑面积!");
                    }
                }
            }
            v.IsUnique(a => a.SHOPID);
            v.IsUnique(a => a.CODE);
            v.Require(a => a.NAME);
            v.Require(a => a.BRANCHID);
            v.Require(a => a.REGIONID);
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
        public void Delete(List<SHOPEntity> DefineDelete)
        {
            foreach (var con in DefineDelete)
            {
                CommenDelete(con);
            }  
        }
        public string Check(SHOPEntity DefineSave)
        {
            DefineSave.STATUS = "2";
            var v = GetVerify(DefineSave);
            v.IsUnique(a => a.SHOPID);
            v.IsUnique(a => a.CODE);
            v.Require(a => a.NAME);
            v.Require(a => a.BRANCHID);
            v.Require(a => a.REGIONID);
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
        public UIResult GetRegion(REGIONEntity Data)
        {
            return new UIResult(service.DataService.GetRegion(Data));
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
                });
        }
    }
}