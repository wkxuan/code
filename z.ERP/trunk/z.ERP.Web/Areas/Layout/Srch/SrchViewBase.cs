using z.MVC5.Views;

namespace z.ERP.Web.Areas.Layout.Srch
{
    public class SrchViewBase : ViewBase<SrchRender>
    {
        public SrchViewBase()
        {

        }

        public override string Layout
        {
            get
            {
                return "~/Areas/Layout/Srch/_SrchLayout.cshtml";
            }
        }
    }
}