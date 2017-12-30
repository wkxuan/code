using z.MVC5.Views;

namespace z.ERP.Web.Areas.Layout.Search
{
    public class SearchViewBase : ViewBase<dynamic>
    {
        public SearchViewBase()
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