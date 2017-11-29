using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace z.Results
{

    public class SelectItem
    {
        public SelectItem()
        {
        }

        public SelectItem(string key, string value)
        {
            Key = key;
            Value = value;
        }
        public string Key
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        public bool IsSelected
        {
            get;
            set;
        }
    }
}
