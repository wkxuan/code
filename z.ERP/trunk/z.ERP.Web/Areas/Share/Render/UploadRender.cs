using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.MVC5.Models;
using z.Results;

namespace z.ERP.Web.Areas.Share.Render
{
    public class UploadRender : VueRender
    {
        public override string ControllerMothod
        {
            get
            {
                return "Upload";
            }
        }
        public string handleUpload
        {
            get;
            set;
        }
        public string Click
        {
            get;
            set;
        }       
        public string loadingStatus
        {
            get;
            set;
        }
        public string file
        {
            get;
            set;
        }
        public string filename
        {
            get;
            set;
        }
    }
}