using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.MVC5.Results;

namespace z.ERP.Web.Areas.Pop.Pop
{
    public class PopController : BaseController
    {
        public ActionResult PopBillList()
        {
            ViewBag.Title = "账单";
            return View();
        }
        public ActionResult PopFeeSubjectList()
        {
            ViewBag.Title = "收费项目";
            return View();
        }
        public ActionResult PopRoleList()
        {
            ViewBag.Title = "角色";
            return View();
        }
        public ActionResult PopShopList()
        {
            ViewBag.Title = "单元";
            return View();
        }
        public ActionResult PopContractList()
        {
            ViewBag.Title = "租约";
            return View();
        }
        public ActionResult PopMerchantList()
        {
            ViewBag.Title = "商户";
            return View();
        }
        public ActionResult PopSysuserList()
        {
            ViewBag.Title = "用户";
            return View();
        }
        public ActionResult PopJsklGroupList()
        {
            ViewBag.Title = "扣率组";
            return View();
        }
        public ActionResult PopBrandList()
        {
            ViewBag.Title = "品牌";
            return View();
        }
        public ActionResult PopGoodsList()
        {
            ViewBag.Title = "商品";
            return View();
        }
        public ActionResult PopGoodsShopList()
        {
            ViewBag.Title = "商品";
            return View();
        }

        public ActionResult PopWLMerchantList()
        {
            ViewBag.Title = "物料供应商";
            return View();
        }

        public ActionResult PopWLGoodsList()
        {
            ViewBag.Title = "物料信息";
            return View();
        }

        public ActionResult PopWLGoodsStockList()
        {
            ViewBag.Title = "物料库存";
            return View();
        }
        public ActionResult PopWLGoodsDjxxList()
        {
            ViewBag.Title = "物料业务数据";
            return View();
        }
        public ActionResult PopFloorMapShow()
        {
            ViewBag.Title = " ";
            return View();
        }
        public ActionResult PopStationList() {
            ViewBag.Title = "POS";
            return View();
        }
        public ActionResult PopInvoiceList() {
            ViewBag.Title = "发票";
            return View();
        }
        public ActionResult PopPayList()
        {
            ViewBag.Title = "收款方式";
            return View();
        }
        //public UIResult SearchFloorMapData(FLOORMAPEntity Data)
        //{
        //    var res = service.DpglService.GetFLOORMAPDATA(Data);
        //    return new UIResult(
        //        new
        //        {
        //            floormap = res.Item1,
        //            floorshopdata = res.Item2
        //        }
        //    );
        //}
    }

}