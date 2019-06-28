using System.Collections.Generic;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.ERP.Web.Areas.Layout.EditDetail;
using z.ERP.Web.Areas.Layout.Search;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.SPGL.RATE_ADJUST
{
    public class Rate_AdjustController : BaseController
    {
        public ActionResult Rate_AdjustList()
        {
            ViewBag.Title = "扣率调整单";
            return View(new SearchRender()
            {
                Permission_Browse = "10500700",
                Permission_Add = "10500701",
                Permission_Del = "10500701",
                Permission_Edit = "10500701",
                Permission_Exec = "10500702"
            });
        }
            public ActionResult Rate_AdjustEdit(string Id)
        {
            ViewBag.Title = "编辑扣率调整单";
            return View("Rate_AdjustEdit", model: (EditRender)Id);
        }
        public UIResult ShowOneRateAdjustEdit(RATE_ADJUSTEntity Data)
        {
            return new UIResult(service.SpglService.ShowOneRateAdjustEdit(Data));
        }
        public string Save(RATE_ADJUSTEntity SaveData)
        {
            return service.SpglService.SaveRateAdjust(SaveData);
        }
        public void Delete(List<RATE_ADJUSTEntity> DeleteData)
        {
            service.SpglService.DeleteRateAdjust(DeleteData);
        }

        public string ExecData(RATE_ADJUSTEntity SaveData)
        {
            return service.SpglService.ExecRateAdjust(SaveData);
        }

        public string StopData(RATE_ADJUSTEntity SaveData)
        {
            return service.SpglService.StopRateAdjust(SaveData);
        }

    }
    
}