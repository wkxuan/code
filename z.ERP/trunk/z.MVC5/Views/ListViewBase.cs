using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Optimization;
using z.MVC5.Models;

namespace z.MVC5.Views
{
    public class ListViewBase : ViewBase<dynamic>
    {
        public ListViewBase()
        {

        }

        public override string Layout
        {
            get
            {
                return base.LayoutUrl;
            }
        }

        public override string LayoutUrl
        {
            get
            {
                return "~/Areas/Base/_ListLayout.cshtml";
            }
        }
    }
}
