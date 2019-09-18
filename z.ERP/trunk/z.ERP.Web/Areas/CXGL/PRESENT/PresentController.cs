using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Entities.Enum;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.DefineDetail;
using z.Exceptions;
using z.Extensions;
using z.MVC5.Results;
using System;

namespace z.ERP.Web.Areas.CXGL.PRESENT
{
    public class PresentController : BaseController
    {
        public ActionResult Present()
        {
            ViewBag.Title = "赠品定义";
            return View();
        }
        public ActionResult PresentDetail(string Id)
        {
            ViewBag.Title = "赠品定义";
            return View("PresentDetail", model: (DefineDetailRender)Id);
        }


        public string Save(PresentEntity DefineSave)
        {
            var v = GetVerify(DefineSave);


            v.Require(a => a.ID);
            v.Require(a => a.BRANCHID); 
            v.Require(a => a.NAME);
            v.Require(a => a.PRICE);
            v.Require(a => a.STATUS);
            v.Verify();
            return CommonSave(DefineSave);
        }


        public void Delete(List<PresentEntity> DefineDelete)
        {
            foreach (var con in DefineDelete)
            {
                CommenDelete(con);
            }
        }
        /// <summary>
        /// 编辑列表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public UIResult GetPresent(PresentEntity data)
        {
            var dt = service.CxglService.GetPresent(data);
            return new UIResult(
                new
                    {
                       dt
                    }
                ); 
        }

     
    }
}