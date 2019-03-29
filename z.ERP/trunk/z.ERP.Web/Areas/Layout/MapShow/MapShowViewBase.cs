using z.MVC5.Views;

namespace z.ERP.Web.Areas.Layout.MapShow
{
    public class MapShowViewBase : ViewBase<dynamic>
    {
        public MapShowViewBase()
        {

        }

        public override string Layout
        {
            get
            {
                return "~/Areas/Layout/MapShow/_MapShow.cshtml";
            }
        }
    }
}