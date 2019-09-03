var colD = [
    { title: '商户编码', key: 'MERCHANTID', width: 120,sortable:true },
    { title: '商户名称', key: 'MERCHANTNAME', width: 200, sortable: true },
    { title: '品牌名称', key: 'BRANDNAME', width: 150, sortable: true },
    { title: '年月', key: 'YEARMONTH', width: 120, sortable: true },
    { title: '铺位面积', key: 'AREA', width: 120, align: "right", sortable: true },
    { title: '基础租金', key: 'JCZJ', width: 120, align: "right", sortable: true },
    { title: '抽成租金', key: 'TCZJ', width: 120, align: "right", sortable: true },
    { title: '实际租金', key: 'SJZJ', width: 120, align: "right", sortable: true },
    { title: '销售额', key: 'AMOUNT', width: 120, align: "right", sortable: true },
    { title: '坪效', key: 'BX', width: 120, align: "right", sortable: true },
    { title: '租售比', key: 'ZSB', width: 120, align: "right", sortable: true },
];

var colGD = [
    { title: '商户编码', key: 'MERCHANTID', width: 120, sortable: true },
    { title: '商户名称', key: 'MERCHANTNAME', width: 200, sortable: true },
    { title: '品牌名称', key: 'BRANDNAME', width: 150, sortable: true },
    { title: '年月', key: 'YEARMONTH', width: 120, sortable: true },
    { title: '铺位面积', key: 'AREA', width: 120, align: "right", sortable: true },
    { title: '基础租金', key: 'JCZJ', width: 120, align: "right", sortable: true },
    { title: '销售额', key: 'AMOUNT', width: 120, align: "right", sortable: true },
    { title: '坪效', key: 'BX', width: 120, align: "right", sortable: true },
    { title: '租售比', key: 'ZSB', width: 120, align: "right", sortable: true },

];

srch.beforeVue = function () {
    srch.screenParam.colDef = colD;
    srch.service = "ReportService";
    srch.method = "MerchantBusinessStatus";

    srch.screenParam.showPop = false;
    srch.screenParam.srcPop = "";
    srch.screenParam.title = "";
    srch.screenParam.popParam = {};
    srch.searchParam.SrchTYPE = 1;
};

srch.newCondition = function () {
    srch.searchParam.SrchTYPE = 1;
    srch.searchParam.BRANCHID = "";
    srch.searchParam.MERCHANTID = "";
    srch.searchParam.MERCHANTNAME = "";
    srch.searchParam.BRANDID = "";
    srch.searchParam.BRANDNAME = "";
    srch.searchParam.NIANYUE_END = "";
    srch.searchParam.NIANYUE_START = "";
};

srch.otherMethods = {
    SelMerchant: function () {
        srch.screenParam.title = "选择商户";
        srch.screenParam.srcPop = __BaseUrl + "/Pop/Pop/PopMerchantList/";
        srch.screenParam.showPop = true;
    },
    SelBrand: function () {
        srch.screenParam.title = "选择品牌";
        srch.screenParam.srcPop = __BaseUrl + "/Pop/Pop/PopBrandList/";
        srch.screenParam.showPop = true;
    },
    changeSrchType: function (value) {
        if (value == 1) {
            Vue.set(this, "data", []);   //清空table
            Vue.set(this, "pagedataCount", 0);    //清空分页数据
            Vue.set(srch.screenParam, "colDef", colD);
        } else {
            Vue.set(this, "data", []);   //清空table
            Vue.set(this, "pagedataCount", 0);    //清空分页数据
            Vue.set(srch.screenParam, "colDef", colGD);
        }
    }
}

srch.popCallBack = function (data) {
    if (srch.screenParam.showPop) {
        srch.screenParam.showPop = false;
        if (srch.screenParam.title == "选择商户") {
            for (var i = 0; i < data.sj.length; i++) {
                srch.searchParam.MERCHANTID = data.sj[i].MERCHANTID;
                srch.searchParam.MERCHANTNAME = data.sj[i].NAME;
            }
        }
        if (srch.screenParam.title == "选择品牌") {
            for (var i = 0; i < data.sj.length; i++) {
                srch.searchParam.BRANDID = data.sj[i].BRANDID;
                srch.searchParam.BRANDNAME = data.sj[i].NAME;
            }
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