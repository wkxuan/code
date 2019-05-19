using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.MVC5.Models;

namespace z.ERP.Web.Areas.Share.Render
{
    public class DrawerRender : VueRender
    {
        public override string ControllerMothod
        {
            get
            {
                return "Drawer";
            }
        }
        public string Title { get; set; }
        public string Width { get; set; }
        public string Styles { get; set; }
        public string Closable { get; set; }
        public string Draggable { get; set; }
        public string Html { get; set; }
    }
}