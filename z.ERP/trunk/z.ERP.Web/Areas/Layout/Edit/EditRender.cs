namespace z.ERP.Web.Areas.Layout.Edit
{
    public class EditRender
    {
        public string Id
        {
            get;
            set;
        }
        public static implicit operator EditRender(string id)
        {
            return new EditRender()
            {
                Id = id
            };
        }
    }
}