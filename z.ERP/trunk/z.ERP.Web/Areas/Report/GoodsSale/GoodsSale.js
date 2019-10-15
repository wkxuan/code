﻿var cols = [
    { title: '商品编码', key: 'GOODSDM', width: 130, sortable: true },
    { title: '商品条码', key: 'BARCODE', width: 130, sortable: true },
    { title: '商品名称', key: 'GOODSNAME', width: 130, sortable: true },
    { title: '租约号', key: 'CONTRACTID', width: 130, sortable: true },
    { title: '商户编码', key: 'MERCHANTID', width: 130, sortable: true },
    { title: '商户名称', key: 'MERCHANTNAME', width: 200, sortable: true },
    { title: '分类编码', key: 'KINDCODE', width: 130, sortable: true },
    { title: '分类名称', key: 'KINDNAME', width: 130, sortable: true },
    { title: '品牌', key: 'BRANDNAME', width: 130, sortable: true },
    { title: '销售金额', key: 'AMOUNT', width: 130, align: "right" },
    { title: '销售成本', key: 'COST', width: 130, align: "right" },
    { title: '折扣金额', key: 'DIS_AMOUNT', width: 100, align: "right" },
    { title: '优惠金额', key: 'PER_AMOUNT', width: 100, align: "right" },
];

var echartTypeList = [{ label: "按租约", value: "CONTRACTID" },
                     { label: "按商户", value: "MERCHANTNAME" },
                     { label: "按商品", value: "GOODSNAME" },
                     { label: "按分类", value: "KINDNAME" },
                     { label: "按品牌", value: "BRANDNAME" }];

search.beforeVue = function () {
    search.screenParam.colDef = [{ title: '日期', key: 'RQ', width: 130, cellType: "date", sortable: true }].concat(cols);
    search.service = "ReportService";
    search.method = "GoodsSale";
    search.panelTwoShow = true;
    search.indexShow = true;
    search.selectionShow = false;
    search.screenParam.showPopMerchant = false;
    search.screenParam.srcPopMerchant = __BaseUrl + "/Pop/Pop/PopMerchantList/";
    search.screenParam.showPopContract = false;
    search.screenParam.srcPopContract = __BaseUrl + "/Pop/Pop/PopContractList/";
    search.screenParam.showPopBrand = false;
    search.screenParam.srcPopBrand = __BaseUrl + "/Pop/Pop/PopBrandList/";

    search.screenParam.popParam = {};
    search.searchParam.SrchTYPE = 1;
    search.searchParam.CATEGORYCODE = "";
    search.screenParam.CATEGORY = [];

    search.screenParam.echartType = echartTypeList.concat({ label: "按日期", value: "RQ" });
    search.screenParam.echartRadioVal = "CONTRACTID";
    search.screenParam.dataSumTypeList = [{ label: "销售金额", value: "AMOUNT" }, { label: "销售成本", value: "COST" },
                                        { label: "折扣金额", value: "DIS_AMOUNT" }, { label: "优惠金额", value: "PER_AMOUNT" }];
    search.screenParam.echartData = [];
};

search.newCondition = function () {
    search.searchParam.SrchTYPE = 1;
    search.searchParam.BRANCHID = "";
    search.searchParam.CATEGORYCODE = "";
    search.searchParam.MERCHANTID = "";
    search.searchParam.MERCHANTNAME = "";
    search.searchParam.GOODSDM = "";
    search.searchParam.GOODSNAME = "";
    search.searchParam.CONTRACTID = "";
    search.searchParam.BRANDID = "";
    search.searchParam.BRANDNAME = "";
    search.searchParam.RQ_START = "";
    search.searchParam.RQ_END = "";
    search.searchParam.YEARMONTH_END = "";
    search.searchParam.YEARMONTH_START = "";
};

search.searchDataAfter = function (data) {
    search.screenParam.echartData = data;
};

search.mountedInit = function () {
    _.Ajax('SearchCate', {
        Data: {}
    }, function (data) {
        Vue.set(search.screenParam, "CATEData", data.treeOrg.Obj);
    });

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
        search.screenParam.showPopMerchant = true;
    },
    SelContract: function () {
        search.screenParam.showPopContract = true;
    },
    SelBrand: function () {
        search.screenParam.showPopBrand = true;
    },
    changeCate: function (value, selectedData) {
        search.searchParam.CATEGORYCODE = selectedData[selectedData.length - 1].code;
    },
    changeSrchType: function (value) {
        search.screenParam.echartData = [];
        if (value == 1) {
            Vue.set(this, "data", []);
            Vue.set(this, "pagedataCount", 0);    //清空分页数据
            search.screenParam.colDef = [{ title: '日期', key: 'RQ', width: 130, cellType: "date", sortable: true }].concat(cols);
            search.screenParam.echartType = echartTypeList.concat({ label: "按日期", value: "RQ" });
            search.screenParam.echartRadioVal = "RQ";
        } else {
            Vue.set(this, "data", []);
            Vue.set(this, "pagedataCount", 0);    //清空分页数据
            search.screenParam.colDef = [{ title: '年月', key: 'YEARMONTH', width: 130, sortable: true }].concat(cols);
            search.screenParam.echartType = echartTypeList.concat({ label: "按年月", value: "YEARMONTH" });
            search.screenParam.echartRadioVal = "YEARMONTH";
        }
    },
};

search.popCallBack = function (data) {

    if (search.screenParam.showPopMerchant) {
        search.screenParam.showPopMerchant = false;
        for (var i = 0; i < data.sj.length; i++) {
            search.searchParam.MERCHANTID = data.sj[i].MERCHANTID;
            search.searchParam.MERCHANTNAME = data.sj[i].NAME;
        }
    }

    if (search.screenParam.showPopContract) {
        search.screenParam.showPopContract = false;
        for (var i = 0; i < data.sj.length; i++) {
            search.searchParam.CONTRACTID = data.sj[i].CONTRACTID;
        }
    }

    if (search.screenParam.showPopBrand) {
        search.screenParam.showPopBrand = false;
        for (var i = 0; i < data.sj.length; i++) {
            search.searchParam.BRANDID = data.sj[i].BRANDID;
            search.searchParam.BRANDNAME = data.sj[i].NAME;
        }
    }
};

search.IsValidSrch = function () {
    if (!search.searchParam.SrchTYPE) {
        iview.Message.info("请选择查询类型!");
        return false;
    }

    return true;
};