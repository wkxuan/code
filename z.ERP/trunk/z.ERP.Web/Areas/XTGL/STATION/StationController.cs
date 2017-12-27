using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;

namespace z.ERP.Web.Areas.XTGL.STATION
{
    public class StationController: BaseController
    {
        public ActionResult Station()
        {
            return View();
        }
        public string Save(STATIONEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            if (DefineSave.STATIONBH.IsEmpty())
            DefineSave.STATIONBH = service.CommonService.NewINC("STATION").PadLeft(6, '0');
            v.Require(a => a.STATIONBH);
            v.Require(a => a.TYPE);
            v.Require(a => a.IP);            
            v.IsUnique(a => a.IP);
            v.Verify();
            return CommonSave(DefineSave);
        }
    }
}