namespace z.MVC5.Views
{
    public class DefineViewPage : DefineViewBase
    {
        public DefineViewPage()
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
