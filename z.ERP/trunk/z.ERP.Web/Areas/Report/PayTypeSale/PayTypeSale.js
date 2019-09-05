var colList = [
    { title: '交易时间', key: 'SALE_TIME', width: 200, sortable: true, ellipsis: true, tooltip: true },
    { title: '交易号', key: 'DEALID', width: 150, sortable: true, ellipsis: true, tooltip: true },
    { title: '商户编码', key: 'MERCHANTID', width: 120, sortable: true, ellipsis: true, tooltip: true },
    { title: '商户名称', key: 'NAME', width: 250, sortable: true, ellipsis: true, tooltip: true },
    { title: '品牌名称', key: 'BRANDNAME', width: 150, sortable: true, ellipsis: true, tooltip: true },
    { title: '终端号', key: 'POSNO', width: 150, sortable: true, ellipsis: true, tooltip: true },
    { title: '收款方式', key: 'PAYNAME', width: 150, sortable: true, ellipsis: true, tooltip: true },
    { title: '销售金额', key: 'AMOUNT', width: 150, align: "right", sortable: true, ellipsis: true, tooltip: true },
];

var colSum = [
    { title: '商户编码', key: 'MERCHANTID', width: 120, sortable: true, ellipsis: true, tooltip: true },
    { title: '商户名称', key: 'NAME', width: 250, sortable: true, ellipsis: true, tooltip: true },
    { title: '终端号', key: 'POSNO', width: 150, sortable: true, ellipsis: true, tooltip: true },
    { title: '收款方式', key: 'PAYNAME', width: 150, sortable: true, ellipsis: true, tooltip: true },
    { title: '销售金额', key: 'AMOUNT', width: 150, align: "right", sortable: true, ellipsis: true, tooltip: true },
];
var echartTypeList1 = [{ label: "按商户", value: "NAME" },
                     { label: "按终端号", value: "POSNO" },
                     { label: "按交易时间", value: "SALE_TIME" },
                     { label: "按品牌", value: "BRANDNAME" },
                     { label: "按收款方式", value: "PAYNAME" }];

var echartTypeList2 = [{ label: "按商户", value: "NAME" },
                     { label: "按终端号", value: "POSNO" },
                     { label: "按收款方式", value: "PAYNAME" }];

srch.beforeVue = function () {
    srch.screenParam.colDef = colList;
    srch.service = "ReportService";
    srch.method = "PayTypeSale";
    srch.echartResult = true;
    srch.screenParam.showPopMerchant = false;
    srch.screenParam.srcPopMerchant = __BaseUrl + "/" + "Pop/Pop/PopMerchantList/";
    srch.screenParam.showPopBrand = false;
    srch.screenParam.srcPopBrand = __BaseUrl + "/" + "Pop/Pop/PopBrandList/";

    srch.screenParam.popParam = {};
    srch.searchParam.SrchTYPE = 1;

    srch.screenParam.echartType = echartTypeList1;
    srch.screenParam.echartRadioVal = "NAME";
    srch.screenParam.dataSumTypeList = [{ label: "销售金额", value: "AMOUNT" }];
    srch.screenParam.echartData = [];
};

srch.newCondition = function () {
    srch.searchParam.BRANCHID= "";
    srch.searchParam.MERCHANTNAME = "";   
    srch.searchParam.MERCHANTID = "";
    srch.searchParam.BRANDNAME = "";
    srch.searchParam.Pay = "";
    srch.searchParam.RQ_START = "";
    srch.searchParam.RQ_END = "";
    srch.searchParam.YEARMONTH_START = "";
    srch.searchParam.YEARMONTH_END = "";
};

srch.echartInit = function (data) {
    srch.screenParam.echartData = data;
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
        srch.screenParam.echartData = [];
        debugger
        if (value == 1) {
            Vue.set(this, "data", []);   //清空table
            Vue.set(this, "pagedataCount", 0);    //清空分页数据
            Vue.set(srch.screenParam, "colDef", colList);
            srch.screenParam.echartType = echartTypeList1;
            srch.screenParam.echartRadioVal = "NAME";
        } else {
            Vue.set(this, "data", []); //清空table
            Vue.set(this, "pagedataCount", 0); //清空分页数据
            Vue.set(srch.screenParam, "colDef", colSum);
            srch.screenParam.echartType = echartTypeList2;
            srch.screenParam.echartRadioVal = "PAYNAME";
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