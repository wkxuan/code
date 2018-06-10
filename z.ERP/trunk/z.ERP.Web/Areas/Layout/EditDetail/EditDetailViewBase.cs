using z.MVC5.Views;
namespace z.ERP.Web.Areas.Layout.EditDetail
{
    public class EditDetailViewBase<T> : ViewBase<EditRender>
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