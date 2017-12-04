namespace z.MVC5.Views
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
                return base.LayoutUrl;
            }
        }

        public override string LayoutUrl
        {
            get
            {
                return "~/Areas/Base/_DefineLayout.cshtml";
            }
        }
    }
}
