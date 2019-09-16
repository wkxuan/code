﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.DefineDetail;
using z.Extensions;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.XTGL.SHOP
{
    public class PromotionController : BaseController
    {
        public ActionResult Promotion()
        {
            ViewBag.Title = "促销活动主题";
            return View();
        }
        public ActionResult PromotionDetail(string Id)
        {
            ViewBag.Title = "促销活动主题信息";
            return View("PromotionDetail", model: (DefineDetailRender)Id);
        }
        public string Save(PROMOTIONEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            if (DefineSave.ID.IsEmpty())
            {
                DefineSave.ID = service.CommonService.NewINC("PROMOTION");
            }
            DefineSave.REPORTER = employee.Id;
            DefineSave.REPORTER_NAME = employee.Name;
            DefineSave.REPORTER_TIME = DateTime.Now.ToString();

            v.IsUnique(a => a.ID);
            v.Require(a => a.NAME);
            v.Require(a => a.YEAR);
            v.Require(a => a.START_DATE);
            v.Require(a => a.END_DATE);
            v.Require(a => a.STATUS);
            v.Verify();

            return CommonSave(DefineSave);
        }
        public void Delete(List<PROMOTIONEntity> DefineDelete)
        {
            foreach (var con in DefineDelete)
            {
                CommenDelete(con);
            }  
        }
        public string Check(PROMOTIONEntity DefineSave)
        {
            DefineSave.STATUS = "2";
            DefineSave.VERIFY = employee.Id;
            DefineSave.VERIFY_NAME = employee.Name;
            DefineSave.VERIFY_TIME = DateTime.Now.ToString();

            var v = GetVerify(DefineSave);
            v.IsUnique(a => a.ID);
            v.Require(a => a.NAME);
            v.Require(a => a.YEAR);
            v.Require(a => a.START_DATE);
            v.Require(a => a.END_DATE);
            v.Require(a => a.STATUS);
            v.Verify();

            return CommonSave(DefineSave);
        }
        public UIResult ShowOneData(PROMOTIONEntity Data)
        {
            var res = service.CxglService.ShowOneData(Data);
            return new UIResult(
                new
                {
                    res
                }
            );
        }
    }
}