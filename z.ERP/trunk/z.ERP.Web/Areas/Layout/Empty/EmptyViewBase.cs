using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.MVC5.Views;

namespace z.ERP.Web.Areas.Layout.Empty
{
    public class EmptyViewBase : ViewBase<dynamic>
    {
        public EmptyViewBase()
        {

        }

        public override string Layout
        {
            get
            {
                return "~/Areas/Layout/Empty/_EmptyLayout.cshtml";
            }
        }
    }
}