using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace z.ERP.Web.Areas.Layout.EditDetail
{
    public class EditRender
    {
        public string Id
        {
            get;
            set;
        }

        public string Permission_Add
        {
            get;
            set;
        }
        public string Permission_Save
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