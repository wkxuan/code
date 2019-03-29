using z.MVC5.Views;

namespace z.ERP.Web.Areas.Layout.PopShow
{
    public class PopShowViewBase : ViewBase<dynamic>
    {
        public PopShowViewBase()
        {

        }

        public override string Layout
        {
            get
            {
                return "~/Areas/Layout/PopShow/_PopShowLayout.cshtml";
            }
        }
    }
}