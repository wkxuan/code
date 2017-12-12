using z.MVC5.Views;

namespace z.ERP.Web.Areas.Layout.Define
{
    public class DefineViewBase : ViewBase<dynamic>
    {
        public DefineViewBase()
        {

        }

        public override string Layout
        {
            get
            {
                return "~/Areas/Layout/Define/_DefineLayout.cshtml";
            }
        }
        
    }
}
