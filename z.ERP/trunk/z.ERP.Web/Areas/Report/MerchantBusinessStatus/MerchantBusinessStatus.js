var colD = [
    { title: '商户编码', key: 'MERCHANTID', width: 150,sortable:true },
    { title: '商户名称', key: 'MERCHANTNAME', width: 250, sortable: true },
    { title: '品牌名称', key: 'BRANDNAME', width: 150, sortable: true },
    { title: '年月', key: 'NIANYUE', width: 150, sortable: true },
    { title: '铺位面积', key: 'AREA_RENTABLE', width: 150, align: "right", sortable: true },
    { title: '基础租金', key: 'MUST_MONEY', width: 150, align: "right", sortable: true },
    { title: '抽成租金', key: 'TCZJ', width: 150, align: "right", sortable: true },
    { title: '实际租金', key: 'PAID_MONEY', width: 150, align: "right", sortable: true },
    { title: '销售额', key: 'AMOUNT', width: 150, align: "right", sortable: true },
    { title: '坪效', key: 'AMOUNT_AREA', width: 150, align: "right", sortable: true },
    { title: '租售比', key: 'AREA_MONEY', width: 150, align: "right", sortable: true },
];

var colGD = [
    { title: '商户编码', key: 'MERCHANTID', width: 150, sortable: true },
    { title: '商户名称', key: 'MERCHANTNAME', width: 250, sortable: true },
    { title: '品牌名称', key: 'BRANDNAME', width: 150, sortable: true },
    { title: '年月', key: 'NIANYUE', width: 150, sortable: true },
    { title: '铺位面积', key: 'AREA_RENTABLE', width: 150, align: "right", sortable: true },
    { title: '基础租金', key: 'MUST_MONEY', width: 150, align: "right", sortable: true },
    { title: '销售额', key: 'AMOUNT', width: 150, align: "right", sortable: true },
    { title: '坪效', key: 'AMOUNT_AREA', width: 150, align: "right", sortable: true },
    { title: '租售比', key: 'AREA_MONEY', width: 150, align: "right", sortable: true },

];

srch.beforeVue = function () {
    srch.screenParam.colDef = colD;
    srch.service = "ReportService";
    srch.method = "MerchantBusinessStatus";

    srch.screenParam.showPopMerchant = false;
    srch.screenParam.srcPopMerchant = __BaseUrl + "/" + "Pop/Pop/PopMerchantList/";
    srch.screenParam.showPopBrand = false;
    srch.screenParam.srcPopBrand = __BaseUrl + "/" + "Pop/Pop/PopBrandList/";

    srch.screenParam.popParam = {};
    srch.searchParam.SrchTYPE = 1;
};

srch.newCondition = function () {
    srch.searchParam.BRANCHID = "";
    srch.searchParam.MERCHANTNAME = "";
    srch.searchParam.BRANDNAME = "";
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
    changeSrchType: function (value) {
        if (value == 1) {
            Vue.set(this, "data", []);   //清空table
            Vue.set(this, "pagedataCount", 0);    //清空分页数据
            Vue.set(srch.screenParam, "colDef", colD);
            Vue.set(srch, "method", "MerchantBusinessStatus");
        } else {
            Vue.set(this, "data", []);   //清空table
            Vue.set(this, "pagedataCount", 0);    //清空分页数据
            Vue.set(srch.screenParam, "colDef", colGD);
            Vue.set(srch, "method", "MerchantBusinessStatusGD");
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