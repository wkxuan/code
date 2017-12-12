using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Optimization;
using z.MVC5.Models;
using z.MVC5.Views;

namespace z.ERP.Web.Areas.Share
{
    public class ControlViewBase<TModel> : ViewBase<TModel>
    {
        public ControlViewBase()
        {

        }

        public override string Layout
        {
            get
            {
                return null;
            }
        }
    }
}
