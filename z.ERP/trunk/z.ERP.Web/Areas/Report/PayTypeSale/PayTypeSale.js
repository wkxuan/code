var colList = [
    { title: '交易时间', key: 'SALE_TIME', width: 200 },
    { title: '交易号', key: 'DEALID', width: 80 },
    { title: '商户编码', key: 'MERCHANTID', width: 120},
    { title: '商户名称', key: 'NAME', width: 250 },
    { title: '品牌名称', key: 'BRANDNAME', width: 150 },
    { title: '终端号', key: 'POSNO', width: 80 },
    { title: '收款方式', key: 'PAYNAME', width: 100 },
    { title: '销售金额', key: 'AMOUNT', width: 150, align: "right" },
];

var colSum = [
    { title: '商户编码', key: 'MERCHANTID', width: 120 },
    { title: '商户名称', key: 'NAME', width: 250 },
    { title: '终端号', key: 'POSNO', width: 80 },
    { title: '收款方式', key: 'PAYNAME', width: 100 },
    { title: '销售金额', key: 'AMOUNT', width: 150, align: "right" },
];

srch.beforeVue = function () {
    srch.screenParam.colDef = colList;
    srch.service = "ReportService";
    srch.method = "PayTypeSale";

    srch.screenParam.showPopMerchant = false;
    srch.screenParam.srcPopMerchant = __BaseUrl + "/" + "Pop/Pop/PopMerchantList/";
    srch.screenParam.showPopBrand = false;
    srch.screenParam.srcPopBrand = __BaseUrl + "/" + "Pop/Pop/PopBrandList/";

    srch.screenParam.popParam = {};
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
    SelBrand: function () {
        srch.screenParam.showPopBrand = true;
    },
    changeSrchType: function (value) {
        if (value == 1) {
            Vue.set(this.screenParamData, "dataDef", []);   //清空table
            Vue.set(this, "pagedataCount", 0);    //清空分页数据
            Vue.set(srch.screenParam, "colDef", colList);
            Vue.set(srch, "method", "PayTypeSale");
        } else {
            Vue.set(this.screenParamData, "dataDef", []); //清空table
            Vue.set(this, "pagedataCount", 0); //清空分页数据
            Vue.set(srch.screenParam, "colDef", colSum);
            Vue.set(srch, "method", "PayTypeSaleS");
        }
    }
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
    if (!srch.searchParam.SrchTYPE) {
        iview.Message.info("请选择查询类型!");
        return false;
    }
    return true;
}