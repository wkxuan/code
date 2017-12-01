using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Optimization;
using z.Context;
using z.Extensions;
using z.Extensiont;
using z.MVC5.Models;

namespace z.MVC5.Views
{
    public abstract class ViewBase<TModel> : WebViewPage<TModel>
    {
        public ViewBase()
        {

        }
        public virtual string LayoutUrl
        {
            get
            {
                return "~/Areas/Base/_LayoutBase.cshtml";
            }
        }
        public virtual string[] WebFiles
        {
            get; set;
        }
        public override void Execute()
        {
            throw new NotImplementedException();
        }
        public Employee employee
        {
            get
            {
                return LoginHelper.GetLogin();
            }
        }

        public IHtmlString InitThisJs()
        {
            return InitFiles(IOExtension.MakeUri("Areas",
                ViewContext.RouteData.Values["area"].ToString(),
                ViewContext.RouteData.Values["controller"].ToString(),
                ViewContext.RouteData.Values["action"].ToString() + ".js"));
        }

        public IHtmlString InitFiles(params string[] files)
        {
            StringBuilder sb = new StringBuilder();
            files.ForEach(path =>
            {
                if (!path.StartsWith("HTTP", true, null))
                {
                    path = IOExtension.MakeUri(Request.Url.Scheme + "://" + Request.Url.Authority, Request.ApplicationPath, path);
                }
                Uri uri = new Uri(path);
                string query = uri.Query;
                if (string.IsNullOrEmpty(query))
                {
                    path += "?" + GetTm(path);
                }
                else
                {
                    path += "&" + GetTm(path);
                }
                if (uri.LocalPath.ToLower().EndsWith(".css"))
                {
                    sb.Append(Styles.Render(path));
                }
                else if (uri.LocalPath.ToLower().EndsWith(".js"))
                {
                    sb.Append(Scripts.Render(path));
                }
                else
                {
                    throw new Exception($"引用的文件{path}后缀不是js或css");
                }
            });
            return Html.Raw(sb.ToString());
        }

        string GetTm(string str)
        {
            return ((int)(DateTime.Now - new DateTime(1988, 12, 6)).TotalSeconds).ToString();
        }

        /// <summary>
        /// 渲染一个控件
        /// </summary>
        /// <param name="render"></param>
        public void RenderControl(ControlRenderBase render)
        {
            Html.RenderAction(render.View, render.ControllerName, render);
        }
    }
}
