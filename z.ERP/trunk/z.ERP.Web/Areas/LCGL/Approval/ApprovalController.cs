using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.LCGL.Approval
{
    public class ApprovalController: BaseController
    {
        public ActionResult ApprovalList()
        {
            ViewBag.Title = "审批流管理";
            return View();
        }
        public ActionResult ApprovalEdit()
        {
            ViewBag.Title = "审批流详情";
            return View();
        }
        public UIResult GetBranch(BRANCHEntity Data)
        {
            return new UIResult(service.DataService.GetBranch(Data));
        }
        public UIResult GetApprovalData(string branchid)
        {                       
            return new UIResult(service.LcglService.GetApprovalData(branchid));
        }
        public string Switchchange(string branchid,string apprid) {
            return service.LcglService.Switchchange(branchid, apprid);
        }
        public string Save(APPROVAL_BRANCHEntity SaveData,List<APPROVAL_NODEEntity> SaveDataDetail, List<APPROVAL_NODE_OPEREntity> SaveDataOper)
        {            
            return service.LcglService.SaveApprovalData(SaveData, SaveDataDetail, SaveDataOper);
        }
        public UIResult ShowDetail(string branchid, string apprid)
        {
            var data = service.LcglService.ShowDetail(branchid, apprid);
            return new UIResult(new {
                an=data.Item1,
                ano=data.Item2
            });
        }
    }
}