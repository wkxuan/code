using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;
using z.ERP.Web.Areas.Layout.Define;

namespace z.ERP.Web.Areas.XTGL.STATION
{
    public class StationController: BaseController
    {
        public ActionResult Station()
        {
            ViewBag.Title = "收银终端信息";
            return View(new DefineRender()
            {
                Permission_Add = "10500101",
                Permission_Mod = "10500102"
            });
        }
        public string Save(STATIONEntity DefineSave)
        {
           return service.XtglService.SaveSataion(DefineSave);             
        }

        public UIResult SearchStation(STATIONEntity Staion)
        {            
         //   return new UIResult(service.XtglService.GetStaionElement(Staion));
            var res = service.XtglService.GetStaionElement(Staion);
            return new UIResult(
                new
                {
                    Station = res.Item1,
                    Pay = res.Item2
                }
                );
        }

        public UIResult GetStaionPayList()
        {
            return new UIResult(service.XtglService.GetStaionPayList());
        }
        public void Delete(STATIONEntity DefineDelete)
        {
            service.XtglService.DeleteStation(DefineDelete);
        }
    }
}