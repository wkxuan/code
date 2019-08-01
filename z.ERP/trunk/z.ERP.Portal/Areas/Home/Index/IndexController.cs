using System.Web.Mvc;
using z.ERP.Entities;
using z.ERP.Web.Areas.Base;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.Home.Index
{
    public class IndexController : BaseController
    {
        // GET: Home/Index
        public ActionResult Index()
        {
            return View();
        }

        public UIResult GetMenu(MENUEntity data)
        {
            string host = Request.Url.Host;
            return service.HomeService.GetMenuNew(data, host);
        }
        public UIResult AllTopData()
        {   
            var Noticedata= service.HomeService.NoticeData(1);   //通知公告
            var Dclrwdata = service.HomeService.DclrwData();  //待处理任务
            var Alertdata= service.HomeService.AlertData();    //预警数据
            var ErpDomain = service.HomeService.GetErpDomain();
            return new UIResult(
                new
                {
                    dclrwdata = Dclrwdata,
                    dclrwcount = Dclrwdata.Rows.Count > 0 ? Dclrwdata.Rows.Count : 0,
                    noticedata = Noticedata,
                    noticecount = Noticedata.Rows.Count > 0 ? Noticedata.Rows.Count : 0,
                    alertdata = Alertdata.Item1,
                    alertcount = Alertdata.Item2,
                    erpdomain = ErpDomain
                }
                );
        }
        public UIResult GetNoticeInfo(string id)
        {
            return new UIResult(service.XtglService.GetNOTICEInfo(id));
        }
        //消息已读
        public void NoticeRead(string id) {
            service.XtglService.NoticeRead(id);
        }
        //消息，已读，未读
        public UIResult GetNoticeData(int type)
        {
            var Noticedata = service.HomeService.NoticeData(type);   //通知公告
            return new UIResult(
                new {
                    noticedata = Noticedata
                }
                );
        }
    }
}