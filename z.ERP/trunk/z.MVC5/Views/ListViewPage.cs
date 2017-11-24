using z.MVC5.Models;

namespace z.MVC5.Views
{
    public class ListViewPage : ListViewBase
    {
        public ListViewPage()
        {
           
        }

        public override string Layout
        {
            get
            {
                return base.LayoutUrl;
            }
        }
    }
}
