using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Share.Render;
using z.Extensions;
using z.MVC5.Models;
using z.Results;

namespace z.ERP.Web.Areas.Share
{
    public class ShareController : BaseController
    {
        public ActionResult Undefine(UndefineRender render)
        {
            return View(render);
        }
        public ActionResult TextBox(TextBoxRender render)
        {
            return View(render);
        }
        public ActionResult Button(ButtonRender render)
        {
            if (render.Invisible)    //不可见
                render.HasPermission = false;
            else
            { 
                if (!render.PermissionKey.IsEmpty())
                    render.HasPermission = employee.HasPermission(render.PermissionKey);
                else
                    render.HasPermission = true;
            }
            return View(render);
        }
        public ActionResult UndefineWindow(UndefineWindowRender render)
        {
            return View(render);
        }
        public ActionResult WindowButton(WindowButtonRender render)
        {
            return View(render);
        }

        public ActionResult Pop(PopRender render)
        {
            return View(render);
        }

        public ActionResult Cascader(CascaderRender render)
        {
            return View(render);
        }

        public ActionResult CommonWindow(CommonWindowRender render)
        {
            return View(render);
        }
        public ActionResult CheckBoxList(CheckBoxListRender render)
        {
            return View(render);
        }
        public ActionResult DateBox(DateBoxRender render)
        {
            return View(render);
        }
        public ActionResult YearMonthBox(YearMonthBoxRender render)
        {
            return View(render);
        }
        public ActionResult BaseDropDownList(DropDownListRender render)
        {
            return View(render);
        }
        public ActionResult ServiceDropDownList(ServiceDropDownListRender render)
        {
            var fun = render.ServiceMothod.Compile();
            render.Data = fun(service.DataService);
            return View("BaseDropDownList", render);
        }
        /// <summary>
        /// 加载html页面弹窗控件
        /// </summary>
        /// <param name="render"></param>
        /// <returns></returns>
        public ActionResult Pops(PopsRender render)
        {
            return View(render);
        }

        /// <summary>
        /// JS初始化下拉控件
        /// </summary>
        /// <param name="render"></param>
        /// <returns></returns>
        public ActionResult BaseDropDownLists(DropDownListsRender render)
        {
            return View(render);
        }
        /// <summary>
        /// 支持仅可显示功能
        /// </summary>
        /// <param name="render"></param>
        /// <returns></returns>
        public ActionResult ViewDropDownList(ViewDropDownListRender render)
        {
            return View(render);
        }
        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="render"></param>
        /// <returns></returns>
        public ActionResult Upload(UploadRender render)
        {
            return View(render);
        }
    }
}