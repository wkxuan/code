using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using z.DbHelper.DbDomain;
using z.Extensions;

namespace z.MVC5.Results
{
    public class SearchItem
    {
        private SearchItem()
        {
        }
        private Dictionary<string, string> _values;

        public PageInfo PageInfo
        {
            get;
            set;
        }

        public Dictionary<string, string> Values
        {
            get
            {
                if (_values == null)
                    _values = new Dictionary<string, string>();
                return _values;
            }

            set
            {
                _values = value;
            }
        }

        public static SearchItem GetAllPram()
        {
            SearchItem item = new Results.SearchItem();
            item.Values = HttpExtension.GetRequestParam<Dictionary<string, string>>("Data");
            item.PageInfo = PageInfo.GetPageinfoFormUI();
            return item;
        }

        public bool HasKey(string key, Action<string> act = null)
        {
            if (Values.ContainsKey(key) && !string.IsNullOrWhiteSpace(Values[key]))
            {
                act?.Invoke(Values[key]);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

