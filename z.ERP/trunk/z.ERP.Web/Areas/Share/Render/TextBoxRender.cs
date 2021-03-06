﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.MVC5.Models;

namespace z.ERP.Web.Areas.Share.Render
{
    public class TextBoxRender : VueRender
    {
        public override string ControllerMothod
        {
            get
            {
                return "TextBox";
            }
        }
        /// <summary>
        /// 水印说明
        /// </summary>
        public string Placeholder
        {
            get;
            set;
        }
        public string Readonly
        {
            get;
            set;
        }

        public string type
        {
            get;
            set;
        }

        public string Change
        {
            get;
            set;
        }
        public string Enter
        {
            get;
            set;
        }
        public string rows
        {
            get;
            set;
        }
        public string blur
        {
            set;
            get;
        }
        public string keyup
        {
            set;
            get;
        }
    }
}