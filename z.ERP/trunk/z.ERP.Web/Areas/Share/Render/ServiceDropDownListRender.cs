using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using z.ERP.Services;
using z.MVC5.Models;
using z.Results;

namespace z.ERP.Web.Areas.Share.Render
{
    public class ServiceDropDownListRender : DropDownListRender
    {
        public ServiceDropDownListRender()
        {

        }

        public Expression<Func<DataService, List<SelectItem>>> ServiceMothod
        {
            get;
            set;
        }

        public override string ControllerMothod
        {
            get
            {
                return "ServiceDropDownList";
            }
        }
    }
}