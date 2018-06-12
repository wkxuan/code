using z.MVC5.Views;

namespace z.ERP.Web.Areas.Layout.PopSearch
{
    public class PopSearchViewBase : ViewBase<dynamic>
    {
        public PopSearchViewBase()
        {

        }

        public override string Layout
        {
            get
            {
                return "~/Areas/Layout/PopSearch/_PopSearchLayout.cshtml";
            }
        }
    }
}