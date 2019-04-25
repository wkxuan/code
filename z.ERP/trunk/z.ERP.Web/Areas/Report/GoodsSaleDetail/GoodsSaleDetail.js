var colD = [
    { title: '交易日期', key: 'SALE_TIME', width: 200 },
    { title: '收款机号', key: 'POSNO', width: 150 },
    { title: '交易号', key: 'DEALID', width: 150 },
    { title: '商品名称', key: 'GOODSNAME', width: 200 },
    { title: '收款方式', key: 'NAME', width: 150 },
    { title: '销售金额', key: 'AMOUNT', width: 150, align: "right" },
    { title: '原收款机号', key: 'POSNO_OLD', width: 150 },
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

    srch.screenParam.popParam = {};
    srch.screenParam.KINDID = [];
    srch.searchParam.SrchTYPE = 1;
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

}

srch.popCallBack = function (data) {

    if (srch.screenParam.showPopMerchant) {
        srch.screenParam.showPopMerchant = false;
        for (var i = 0; i < data.sj.length; i++) {
            srch.searchParam.MERCHANTID = data.sj[i].MERCHANTID;
            srch.searchParam.MERCHANTNAME = data.sj[i].NAME;
        }
    }
};

srch.IsValidSrch = function () {
    return true;
}