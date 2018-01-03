using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.Extensions;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.PPGL.BRAND
{
    public class BrandController : BaseController
    {

        public ActionResult BrandList()
        {
            ViewBag.Title = "品牌列表信息";
            return View();
        }

        public ActionResult BrandEdit()
        {
            ViewBag.Title = "编辑品牌列表信息";
            return View();
        }

        public string Save(BRANDEntity SaveData) {
            //测试临时加入，需要共用处理
            var v = GetVerify(SaveData);
            if (SaveData.ID.IsEmpty())
                SaveData.ID = service.CommonService.NewINC("BRAND");
            SaveData.STATUS = "0";
            SaveData.REPORTER = "1";
            SaveData.REPORTER_NAME = "测试人员";
            SaveData.REPORTER_TIME = DateTime.Now.ToString();
            v.Require(a => a.ID);
            v.Require(a => a.NAME);
            v.Require(a => a.CATEGORYID);
            v.IsNumber(a => a.ID);
            v.IsNumber(a => a.CATEGORYID);
            v.IsUnique(a => a.ID);
            v.IsUnique(a => a.NAME);            
            v.Verify();
            return CommonSave(SaveData);
        }

        public  void Delete(List<BRANDEntity> DeleteData)
        {
            foreach(var brand in DeleteData)
            {
                var v = GetVerify(brand);
                v.Require(a => a.ID);
                v.Verify();
                CommenDelete(brand);
            }

        }
    }
}