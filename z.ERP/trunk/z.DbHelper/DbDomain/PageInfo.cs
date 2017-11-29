using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using z.Extensions;

namespace z.DbHelper.DbDomain
{
    public class PageInfo
    {
        private PageInfo()
        {
        }
        public static PageInfo GetPageinfoFormUI()
        {
            PageInfo p = new DbDomain.PageInfo();
            if (string.IsNullOrEmpty(HttpExtension.GetRequestParam("page")))
            {
                p.PageIndex = p.PageSize = 0;
            }
            else
            {
                p.PageIndex = HttpExtension.GetRequestParam("page").ToInt() - 1;
                p.PageSize = HttpExtension.GetRequestParam("rows").ToInt();
            }
            return p;
        }
        public int PageSize
        {
            get; set;
        }
        public int PageIndex
        {
            get; set;
        }
    }
}
