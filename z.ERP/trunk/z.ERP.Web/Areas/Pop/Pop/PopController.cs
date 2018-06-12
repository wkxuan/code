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

namespace z.ERP.Web.Areas.Pop.Pop
{
    public class PopController : BaseController
    {
        public ActionResult PopBillList()
        {
            ViewBag.Title = "账单";
            return View();
        }
        public ActionResult PopFeeSubjectList()
        {
            ViewBag.Title = "收费项目";
            return View();
        }
        public ActionResult PopRoleList()
        {
            ViewBag.Title = "角色";
            return View();
        }
    }

}