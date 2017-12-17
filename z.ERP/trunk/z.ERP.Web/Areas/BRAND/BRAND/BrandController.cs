using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.Extensions;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.XTGL.BRAND
{
    public class BrandController : BaseController
    {
        public ActionResult Brand()
        {
            //service.TestService.a();

            return View();
        }

        public void Save(BRANDEntity brand) {
            //测试临时加入，需要共用处理
            brand = HttpExtension.GetRequestParam<BRANDEntity>("BRAND");
            var v = GetVerify(brand);
            brand.ID = service.CommonService.NewINC("BRAND");
            brand.CATEGORYID = "1";
            brand.STATUS = "0";
            brand.REPORTER = "1";
            brand.REPORTER_NAME = "测试人员";
            brand.REPORTER_TIME = DateTime.Now.ToString();
            v.Require(a => a.ID);
            v.Require(a => a.NAME);
            v.Require(a => a.CATEGORYID);
            v.IsNumber(a => a.ID);
            v.IsNumber(a => a.CATEGORYID);
            v.IsUnique(a => a.ID);
            v.IsUnique(a => a.NAME);            
            v.Verify();
            CommonSave(brand);
        }
    }
}