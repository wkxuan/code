using z.MVC5.Views;

namespace z.ERP.Web.Areas.Layout.DefineDetail
{
    public class DefineDetailViewBase: ViewBase<dynamic>
    {
        public DefineDetailViewBase()
        {

        }

        public override string Layout
        {
            get
            {
                return "~/Areas/Layout/DefineDetail/_DefineDetail.cshtml";
            }
        }
        
    }
}
