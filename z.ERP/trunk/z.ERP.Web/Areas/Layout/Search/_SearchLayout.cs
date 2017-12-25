using z.MVC5.Views;

namespace z.ERP.Web.Areas.Layout.Search
{
    public class _SearchLayout : ViewBase<dynamic>
    {
        public _SearchLayout()
        {

        }

        public override string Layout
        {
            get
            {
                return "~/Areas/Layout/Search/_SearchLayout.cshtml";
            }
        }
    }
}