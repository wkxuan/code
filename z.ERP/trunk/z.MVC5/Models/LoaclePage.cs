using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using z.Extensions;

namespace z.MVC5.Models
{
    public class LoaclePage
    {
        public LoaclePage(string _area, string _controller, string _action, dynamic _id = null)
        {
            area = _area;
            controller = _controller;
            action = _action;
            id = _id == null ? UrlParameter.Optional : _id;
        }
        public const string UrlModel = "{area}/{controller}/{action}/{id}";

        public string area
        {
            get; set;
        }
        public string controller
        {
            get; set;
        }
        public string action
        {
            get; set;
        }
        public dynamic id
        {
            get; set;
        }
    }
}
