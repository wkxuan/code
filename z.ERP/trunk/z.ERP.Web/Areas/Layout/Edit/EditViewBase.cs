using z.ERP.Web.Areas.Layout.EditDetail;
using z.MVC5.Views;
namespace z.ERP.Web.Areas.Layout.Edit
{
    public class EditViewBase<T> : ViewBase<EditRender>
    {
        public EditViewBase()
        {

        }

        public override string Layout
        {
            get
            {
                return "~/Areas/Layout/Edit/_Edit.cshtml";
            }
        }
    }
}