﻿using System;
using System.Collections.Generic;
using z.DBHelper.DBDomain;
using z.Extensions;

namespace z.MVC5.Results
{
    public class SearchItem
    {
        public SearchItem()
        {
        }
        private Dictionary<string, string> _values;

        public PageInfo _pageInfo;

        public PageInfo PageInfo
        {
            get
            {
                if (_pageInfo == null)
                {
                    _pageInfo = new PageInfo()
                    {
                        PageIndex = 0,
                        PageSize = -1
                    };
                }
                return _pageInfo;
            }
            set
            {
                _pageInfo = value;
            }
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

        public bool HasKey(string key, Action<string> act = null)
        {
            return CommonHas(key, act);
        }

        public bool HasArrayKey(string key, Action<string[]> act = null)
        {
            return CommonHas(key, a =>
            {
                act?.Invoke(a.ToObj<string[]>());
            });
        }

        public bool HasDateKey(string key, Action<string> act = null)
        {
            return CommonHas(key, a =>
            {
                act?.Invoke("to_date('"+a.ToDateTime().ToString("yyyy-MM-dd") +"','YYYY-MM-DD')");
            });
        }

        public bool HasTimeKey(string key, Action<string> act = null)
        {
            return CommonHas(key, a =>
            {
                act?.Invoke(a.ToDateTime().ToLongString());
            });
        }

        bool CommonHas(string key, Action<string> act = null)
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
    public class MenuAuthority
    {
        public string id { set; get; }
        public string authority { set; get; }
        public bool enable { set; get; }
    }
    public class ImportMsg
    {
        private bool _succeFlag = true;
        public bool SuccFlag
        {
            get { return _succeFlag; }
            set { _succeFlag = value; }
        }
        public string Message { get; set; }
    }
}

