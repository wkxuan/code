using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Services;
using z.ERP.Web.Areas.Base;
using z.Extensions;
using z.MVC5;
using z.MVC5.Controllers;
using z.MVC5.Results;
using z.Results;
using z.Verify;

namespace z.ERP.Web.Areas.XTGL.test
{
    public class testController : BaseController
    {
        public ActionResult List()
        {
            service.TestService.a();


            //throw new Exception("123");
            return View();
        }

        //public void Fun2(BMEntity bm)
        //{

        //}
        //public void Save1(BMEntity bm)
        //{
        //    var v = GetVerify(bm);
        //    v.Require(a => a.DEPTID);
        //    v.IsNumber(a => a.DEPTID);
        //    v.Require(a => a.DEPT_NAME);
        //    //v.Require(a => a.BMJB);
        //    v.IsUnique(a => a.DEPTID);
        //    v.Verify();
        //    CommonSave(bm);
        //}
        public UIResult Func1(string s)
        {
            //return new JsonResult()
            //{
            //    Data = "1"
            //};
            //return new UIResult(new SelectItem("111", "222"));
            //return new
            //{
            //    a = "111",
            //    b = 123
            //};
            throw new Exception("123");
        }
    }
}