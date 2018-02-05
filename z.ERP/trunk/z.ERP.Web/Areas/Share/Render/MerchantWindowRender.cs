using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace z.ERP.Web.Areas.Share.Render
{
    public class MerchantWindowRender:WindowBaseRender
    {
        public override string ControllerMothod
        {
            get
            {
                return "MerchantWindow";
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
        public string ResVModel3
        {
            get;
            set;
        }
    }
}