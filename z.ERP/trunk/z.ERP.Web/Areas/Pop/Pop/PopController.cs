﻿using z.ERP.Web.Areas.Base;
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
        public ActionResult PopShopList()
        {
            ViewBag.Title = "单元";
            return View();
        }
        public ActionResult PopContractList()
        {
            ViewBag.Title = "租约";
            return View();
        }
        public ActionResult PopMerchantList()
        {
            ViewBag.Title = "商户";
            return View();
        }
        public ActionResult PopSysuserList()
        {
            ViewBag.Title = "用户";
            return View();
        }
        public ActionResult PopJsklGroupList()
        {
            ViewBag.Title = "扣率组";
            return View();
        }
        public ActionResult PopBrandList()
        {
            ViewBag.Title = "品牌";
            return View();
        }
    }

}