var colD = [
    { title: '交易日期', key: 'SALE_TIME', width: 200,sortable:true },
    { title: '终端号', key: 'POSNO', width: 150, sortable: true },
    { title: '交易号', key: 'DEALID', width: 150, sortable: true },
    { title: '商品名称', key: 'GOODSNAME', width: 200, sortable: true },
    { title: '品牌名称', key: 'BRANDNAME', width: 150, sortable: true },
    { title: '收款方式', key: 'NAME', width: 150, sortable: true },
    { title: '销售金额', key: 'AMOUNT', width: 150, align: "right", sortable: true },
    { title: '原终端号', key: 'POSNO_OLD', width: 150, sortable: true },
    {
        title: '原交易号', key: 'DEALID_OLD', width: 150,
        render: function (h, params) {
            return h('div',this.row.DEALID_OLD==0? "": this.row.DEALID_OLD)
        }
    },
];


srch.beforeVue = function () {
    srch.screenParam.colDef = colD;
    srch.service = "ReportService";
    srch.method = "GoodsSaleDetail";

    srch.screenParam.showPopMerchant = false;
    srch.screenParam.srcPopMerchant = __BaseUrl + "/" + "Pop/Pop/PopMerchantList/";
    srch.screenParam.showPopBrand = false;
    srch.screenParam.srcPopBrand = __BaseUrl + "/" + "Pop/Pop/PopBrandList/";
    srch.screenParam.popParam = {};
    srch.searchParam.SrchTYPE = 1;
};

srch.newCondition = function () {
    srch.searchParam.BRANCHID = "";
    srch.searchParam.MERCHANTID = "";
    srch.searchParam.MERCHANTNAME = "";
    srch.searchParam.GOODSDM = "";
    srch.searchParam.GOODSNAME = "";
    srch.searchParam.BRANDID = "";
    srch.searchParam.BRANDNAME = "";
    srch.searchParam.RQ_START = "";
    srch.searchParam.RQ_END = "";
    srch.searchParam.YEARMONTH_END = "";
    srch.searchParam.YEARMONTH_START = "";
};

srch.mountedInit = function () {
    _.Ajax('SearchKind', {
        Data: {}
    }, function (data) {
        Vue.set(srch.screenParam, "dataKind", data.treeorg.Obj);
    });
}

srch.otherMethods = {
    SelMerchant: function () {
        srch.screenParam.showPopMerchant = true;
    },
    SelBrand: function () {
        srch.screenParam.showPopBrand = true;
    },
}

srch.popCallBack = function (data) {

    if (srch.screenParam.showPopMerchant) {
        srch.screenParam.showPopMerchant = false;
        for (var i = 0; i < data.sj.length; i++) {
            srch.searchParam.MERCHANTID = data.sj[i].MERCHANTID;
            srch.searchParam.MERCHANTNAME = data.sj[i].NAME;
        }
    }
    if (srch.screenParam.showPopBrand) {
        srch.screenParam.showPopBrand = false;
        for (var i = 0; i < data.sj.length; i++) {
            srch.searchParam.BRANDID = data.sj[i].BRANDID;
            srch.searchParam.BRANDNAME = data.sj[i].NAME;
        }
    }
};

srch.IsValidSrch = function () {
    return true;
}