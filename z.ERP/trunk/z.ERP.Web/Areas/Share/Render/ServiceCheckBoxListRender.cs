using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using z.ERP.Services;
using z.Results;

namespace z.ERP.Web.Areas.Share.Render
{
    public class ServiceCheckBoxListRender: CheckBoxListRender
    {
        public ServiceCheckBoxListRender()
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
                return "ServiceCheckBoxList";
            }
        }
    }
}