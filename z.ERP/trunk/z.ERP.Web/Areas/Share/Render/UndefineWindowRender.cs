using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.MVC5.Models;

namespace z.ERP.Web.Areas.Share.Render
{
    public class UndefineWindowRender  : WindowBaseRender
    {


        public override string ControllerMothod
        {
            get
            {
                return "UndefineWindow";
            }
        }

        public string ResVModel1
        {
            get;
            set;
        }
        public string ResVModel2
        {
            get;
            set;
        }
    }
}