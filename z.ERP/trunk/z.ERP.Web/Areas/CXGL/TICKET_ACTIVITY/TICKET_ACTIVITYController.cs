using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.CXGL.TICKET_ACTIVITY
{
    public class TICKET_ACTIVITYController: BaseController
    {
        public ActionResult TICKET_ACTIVITY()
        {
            ViewBag.Title = "小票活动兑奖";
            return View();
        }
        public UIResult GetSaleTicket(string PROMOTIONID,string POSNO, string DEALID)
        {
 
            return new UIResult(service.CxglService.GetSaleTicketInfo(PROMOTIONID, POSNO, DEALID));
        }
        public bool Saves(List<TICKET_ACTIVITY_HISTORYEntity> DefineSave)
        {
            return service.CxglService.SaveTICKET_ACTIVITY_HISTORY(DefineSave);
        }
    }
}