using z.MVC5.Views;
namespace z.ERP.Web.Areas.Layout.EditDetail
{
    public class EditViewBase<T> : ViewBase<dynamic>
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