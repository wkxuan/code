﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.Edit;
using z.MVC5.Attributes;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.BJGL.FLOORMAP
{
    public class FloorMapController : BaseController
    {
        public ActionResult FloorMapList()
        {
            ViewBag.Title = "楼层图纸信息";
            return View();
        }

        public ActionResult FloorMapEdit(string Id)
        {
            ViewBag.Title = "楼层图纸信息编辑";

            return View("FloorMapEdit", model: (EditRender)Id);

        }
        [Permission("11010104")]
        public void Delete(List<FLOORMAPEntity> DeleteData)
        {
            service.DpglService.DeleteFloorMap(DeleteData);
        }

        [Permission("11010101")]
        public string Save(FLOORMAPEntity SaveData)
        {
            return service.DpglService.SaveFloorMap(SaveData);
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

        public UIResult UploadMap()
        {
            var dirPath = HttpContext.Request.MapPath(@"/BackMap") + "\\";

            var fileName = string.Empty;
            var filePath = string.Empty;

            if (this.Request.Files.Count > 0)
            {
                var file = this.Request.Files[0];
                fileName = GenerateFileName(file.FileName);

                filePath = dirPath + @"\" + fileName;
                file.SaveAs(filePath);
            }
            return new UIResult(
                new
                {
                    uploadFileName = fileName,
                    uploadPath = filePath
                });
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
        [Permission("11010103")]
        public void ExecData(FLOORMAPEntity Data)
        {
            service.DpglService.ExecData(Data);
        }
        [Permission("11010104")]
        public void EliminateData(FLOORMAPEntity Data)
        {
            service.DpglService.EliminateData(Data);
        }

    }
}