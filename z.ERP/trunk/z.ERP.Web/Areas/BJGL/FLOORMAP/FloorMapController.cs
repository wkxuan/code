using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.Extensions;
using System;
using System.Collections.Generic;
using z.MVC5.Results;
using z.ERP.Model;
using z.ERP.Entities.Enum;
using System.Data;
using z.MathTools;
using z.ERP.Web.Areas.Layout.Search;
using z.MVC5.Attributes;
using System.IO;
using z.ERP.Web.Areas.Layout.EditDetail;

namespace z.ERP.Web.Areas.BJGL.FLOORMAP
{
    public class FloorMapController : BaseController
    {
        public ActionResult FloorMapList()
        {
            ViewBag.Title = "楼层图纸信息";
            return View(new SearchRender()
            {
                Permission_Browse = "10200100",
                Permission_Add = "10200101",
                Permission_Del = "10200101",
                Permission_Edit = "10200101",
                Permission_Exec = "10200102"
            });
        }


        public ActionResult Detail(string Id)
        {
            ViewBag.Title = "商户信息浏览";
            var entity = service.ShglService.GetMerchantElement(new MERCHANTEntity(Id));
            ViewBag.merchant = entity.Item1;
            ViewBag.merchantBrand = entity.Item2;
            return View();
        }

        public ActionResult FloorMapEdit(string Id)
        {
            ViewBag.Title = "楼层图纸信息编辑";

            return View("FloorMapEdit", model: (EditRender)Id);

        }

        public void Delete(List<FLOORMAPEntity> DeleteData)
        {
            service.DpglService.DeleteFloorMap(DeleteData);
        }

        [Permission("10200101")]
        public string Save(FLOORMAPEntity SaveData)
        {
            return service.DpglService.SaveFloorMap(SaveData);
        }
        public UIResult SearchFloorMap(FLOORMAPEntity Data)
        {
            var res = service.DpglService.GetFLOORMAPElement(Data);
            return new UIResult(
                new
                {
                    floormap = res.Item1,
                    floorshop = res.Item2
                }
            );
        }

        public UIResult UploadMap()
        {
            var dirPath = HttpContext.Request.MapPath(@"/BackMap") + "\\";

            var fileName = string.Empty;


            if (this.Request.Files.Count > 0)
            {
                var file = this.Request.Files[0];
                fileName = GenerateFileName(file.FileName);

                var filePath = dirPath + @"\" + fileName;
                file.SaveAs(filePath);
            }



            return null;
        }

        private string GenerateFileName(string filename)
        {
            const string nameFMT = "{0}_{1:X}{2}";

            var nameWithoutExt = Path.GetFileNameWithoutExtension(filename);
            var ext = Path.GetExtension(filename);

            return string.Format(nameFMT, nameWithoutExt, DateTime.Now.Ticks, ext);
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
        [Permission("10200102")]
        public void ExecData(MERCHANTEntity Data)
        {
            service.ShglService.ExecData(Data);
        }

        public string Output(string Id)
        {
            return service.ShglService.Output(Id);
        }
    }
}