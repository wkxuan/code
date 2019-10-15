using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Search;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.BJGL.FLOORMAPSHOW
{
    public class FloorMapShowController : BaseController
    {
        public ActionResult FloorMapShowList()
        {
            ViewBag.Title = "楼层图纸信息";
            return View();
        }
        public ActionResult FloorMapShow(string Id)
        {
            ViewBag.Title = "楼层图纸信息编辑";

            return View(new SearchRender()
            {
                Permission_Browse = "10200100",
                Permission_Add = "10200101",
                Permission_Del = "10200101",
                Permission_Edit = "10200101",
                Permission_Exec = "10200102"
            });

        }
        public UIResult SearchFloorMap(FLOORMAPEntity Data)
        {
            var res = service.DpglService.GetFloorMapElement(Data);
            return new UIResult(
                new
                {
                    floormap = res.Item1,
                    floorshop = res.Item2
                }
            );
        }

        public UIResult SearchFloorShowMap(FLOORMAPSHOWEntity Data)
        {
            var res = service.DpglService.GetFloorShowMap(Data);
            return new UIResult(
                new
                {
                    floormap = res.Item1,
                    floorshop = res.Item2
                }
            );
        }
        public UIResult SearchFloorShowMapData(FLOORMAPSHOWEntity Data)
        {
            var res = service.DpglService.GetGetFloorShowMapData(Data);
            return new UIResult(
                new
                {
                    floormap = res.Item1,
                    floorcategory = res.Item2,
                    floorshopdata = res.Item3,
                    floorshoprent_status = res.Item4
                }
            );
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


    }
}