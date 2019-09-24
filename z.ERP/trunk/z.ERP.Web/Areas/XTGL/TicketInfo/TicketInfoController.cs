using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Entities.Enum;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.DefineDetail;
using z.Exceptions;
using z.Extensions;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.XTGL.TicketInfo
{
    public class TicketInfoController : BaseController
    {
        public ActionResult TicketInfo()
        {
            ViewBag.Title = "交易小票信息设置";
            return View();
        }
        public ActionResult TicketInfoDetail(string Id)
        {
            ViewBag.Title = "交易小票信息设置";
            return View("TicketInfoDetail", model: (DefineDetailRender)Id);
        }


        public string Save(TicketInfoEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            if (DefineSave.PRINTCOUNT.IsEmpty())
            {
                DefineSave.PRINTCOUNT = "1";
            }
            v.Require(a => a.PRINTCOUNT);
            v.Require(a => a.BRANCHID);  
            v.Require(a => a.HEAD);
            v.Require(a => a.TAIL);
            v.Require(a => a.ADQRCODE);
            v.Require(a => a.ADCONTENT);
            v.Verify();
            return CommonSave(DefineSave);
        }

        public void Delete(List<TicketInfoEntity> DefineDelete)
        {
            foreach (var con in DefineDelete)
            {
                CommenDelete(con);
            }
        }
        public UIResult GetTicketInfo(TicketInfoEntity data)
        {
            var dt = service.XtglService.GetTicketInfo(data);
            return new UIResult(
                new
                    {
                       dt
                    }
                ); 
        }

    }
}