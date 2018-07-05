using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.MVC5.Views;

namespace z.ERP.Web.Areas.Layout.Print
{
    public class PrintLayout<T> : ViewBase<T>
    {
        public PrintLayout()
        {

        }

        public override string Layout
        {
            get
            {
                return "~/Areas/Layout/Print/_PrintLayout.cshtml";
            }
        }
    }
}