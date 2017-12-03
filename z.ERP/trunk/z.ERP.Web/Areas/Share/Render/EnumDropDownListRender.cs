using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.Extensions;
using z.MVC5.Models;
using z.Results;

namespace z.ERP.Web.Areas.Share.Render
{
    public class EnumDropDownListRender<T> : DropDownListRender where T : struct
    {
        public EnumDropDownListRender()
        {
            Data = EnumExtension.EnumToSelectItem<T>();
        }
    }
}