using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.MVC5.Views;

namespace z.ERP.Web.Areas.Layout.Detail
{
    public class DetailLayout<T> : ViewBase<T>
    {
        public DetailLayout()
        {

        }

        public override string Layout
        {
            get
            {
                return "~/Areas/Layout/Detail/_DetailLayout.cshtml";
            }
        }
    }
}