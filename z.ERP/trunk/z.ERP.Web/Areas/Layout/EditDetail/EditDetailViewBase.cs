using z.MVC5.Views;
namespace z.ERP.Web.Areas.Layout.EditDetail
{
    public class EditDetailViewBase : ViewBase<dynamic>
    {
        public EditDetailViewBase()
        {

        }

        public override string Layout
        {
            get
            {
                return "~/Areas/Layout/EditDetail/_EditDetail.cshtml";
            }
        }
    }
}