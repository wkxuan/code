using z.MVC5.Views;

namespace z.ERP.Web.Areas.Layout.Tree
{
    public class TreeViewBase : ViewBase<dynamic>
    {
        public TreeViewBase()
        {

        }

        public override string Layout
        {
            get
            {
                return "~/Areas/Layout/Tree/_TreeLayout.cshtml";
            }
        }
        
    }
}
