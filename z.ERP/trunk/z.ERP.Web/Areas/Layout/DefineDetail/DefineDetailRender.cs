namespace z.ERP.Web.Areas.Layout.DefineDetail
{
    public class DefineDetailRender
    {
        public string Id
        {
            get;
            set;
        }

        public static implicit operator DefineDetailRender(string id)
        {
            return new DefineDetailRender()
            {
                Id = id
            };
        }
    }
}