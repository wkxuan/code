using z.MVC5.Views;

namespace z.ERP.Web.Areas.Layout.DefineNew
{
    public class DefineViewNewBase : ViewBase<DefineNewRender>
    {
        public DefineViewNewBase()
        {

        }

        public override string Layout
        {
            get
            {
                return "~/Areas/Layout/DefineNew/_DefineLayout.cshtml";
            }
        }
        
    }
}
