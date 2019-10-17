﻿var colList = [
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

search.beforeVue = function () {
    search.service = "ReportService";
    search.method = "PayTypeSale";
    search.panelTwoShow = true;
    search.indexShow = true;
    search.selectionShow = false;

    search.popConfig = {
        title: "",
        src: "",
        width: 800,
        height: 550,
        open: false
    };
    search.screenParam.popParam = {};

    search.screenParam.colDef = colList;
    search.screenParam.echartType = echartTypeList1;
    search.screenParam.echartRadioVal = "NAME";
    search.screenParam.dataSumTypeList = [{ label: "销售金额", value: "AMOUNT" }];
    search.screenParam.echartData = [];
};

search.newCondition = function () {
    search.searchParam.SrchTYPE = 1;
    search.searchParam.BRANCHID= [];
    search.searchParam.MERCHANTNAME = "";   
    search.searchParam.BRANDNAME = "";
    search.searchParam.Pay = [];
    search.searchParam.RQ_START = "";
    search.searchParam.RQ_END = "";
    search.searchParam.YEARMONTH_START = "";
    search.searchParam.YEARMONTH_END = "";

    search.screenParam.colDef = colList;
    search.screenParam.echartType = echartTypeList1;
    search.screenParam.echartRadioVal = "NAME";
    search.screenParam.dataSumTypeList = [{ label: "销售金额", value: "AMOUNT" }];
    search.screenParam.echartData = [];
};

search.searchDataAfter = function (data) {
    search.screenParam.echartData = data;
};

search.mountedInit = function () {
    search.btnConfig = [{
        id: "search",
        authority: ""
    }, {
        id: "clear",
        authority: ""
    }, {
        id: "export",
        authority: ""
    }];
}

search.otherMethods = {
    SelMerchant: function () {
        search.screenParam.popParam = {};
        search.popConfig.title = "选择商户";
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopMerchantList/";
        search.popConfig.open = true;
    },
    SelBrand: function () {
        search.screenParam.popParam = {};
        search.popConfig.title = "选择品牌";
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopBrandList/";
        search.popConfig.open = true;
    },
    changeSrchType: function (value) {
        search.screenParam.echartData = [];
        debugger
        if (value == 1) {
            Vue.set(this, "data", []);   //清空table
            Vue.set(this, "pagedataCount", 0);    //清空分页数据
            Vue.set(search.screenParam, "colDef", colList);
            search.screenParam.echartType = echartTypeList1;
            search.screenParam.echartRadioVal = "NAME";
        } else {
            Vue.set(this, "data", []); //清空table
            Vue.set(this, "pagedataCount", 0); //清空分页数据
            Vue.set(search.screenParam, "colDef", colSum);
            search.screenParam.echartType = echartTypeList2;
            search.screenParam.echartRadioVal = "PAYNAME";
        }
    }
}

search.popCallBack = function (data) {
    if (search.popConfig.open) {
        search.popConfig.open = false;
        for (var i = 0; i < data.sj.length; i++) {
            switch (search.popConfig.title) {
                case "选择商户":
                    search.searchParam.MERCHANTNAME = data.sj[i].NAME;
                    break;
                case "选择品牌":
                    search.searchParam.BRANDNAME = data.sj[i].NAME;
                    break;
            }
        }
    }
};

search.IsValidSrch = function () {
    if (!search.searchParam.SrchTYPE) {
        iview.Message.info("请选择查询类型!");
        return false;
    }
    return true;
}