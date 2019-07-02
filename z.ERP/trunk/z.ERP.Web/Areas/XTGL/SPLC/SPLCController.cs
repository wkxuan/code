using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.MVC5.Results;
using z.ERP.Web.Areas.Layout.Define;
using System.Collections.Generic;

namespace z.ERP.Web.Areas.XTGL.SPLC
{
    public class SPLCController : BaseController
    {
        public ActionResult SPLC(string ID)
        {

            ViewBag.Title = "审批流程定义";
            if (ID == null)
            {
                ViewBag.BILLID = "-1";
            }
            else
            {
                ViewBag.BILLID = ID;
            }

            return View();
        }
        public string Save(SPLCDEFDEntity SPLCDEFD, List<SPLCJDEntity> SPLCJD, List<SPLCJGEntity> SPLCJG)
        {
            return service.XtglService.SaveSplc(SPLCDEFD, SPLCJD, SPLCJG);
        }
        public void Delete(SPLCDEFDEntity Data)
        {
            service.XtglService.DeleteSplc(Data);
        }

        public UIResult Srch(SPLCDEFDEntity Data)
        {
            var res = service.XtglService.GetSplcdefdElement(Data);
            return new UIResult(
                new
                {
                    spd = res.Item1,
                    spjd = res.Item2,
                    spjg = res.Item3
                }
            );
        }

        public string exec(SPLCDEFDEntity Data)
        {
            return service.XtglService.ExecSplc(Data);
        }

        public string over(SPLCDEFDEntity Data)
        {
            return service.XtglService.OverSplc(Data);
        }

    }

}