var colD = [
    { title: '日期', key: 'RQ', width: 100, cellType: "date" },
    { title: '商品编码', key: 'GOODSDM', width: 95 },
    { title: '商品条码', key: 'BARCODE', width: 130 },
    { title: '商品名称', key: 'GOODSNAME', width: 120 },
    { title: '租约号', key: 'CONTRACTID', width: 95 },
    { title: '商户编码', key: 'MERCHANTID', width: 90 },
    { title: '商户名称', key: 'MERCHANTNAME', width: 200 },
    { title: '分类编码', key: 'KINDCODE', width: 100 },
    { title: '分类名称', key: 'KINDNAME', width: 100 },
    { title: '品牌', key: 'BRANDNAME', width: 100 },
    { title: '销售金额', key: 'AMOUNT', width: 120, align: "right" },
    { title: '销售成本', key: 'COST', width: 120, align: "right" },
    { title: '折扣金额', key: 'DIS_AMOUNT', width: 100, align: "right" },
    { title: '优惠金额', key: 'PER_AMOUNT', width: 100, align: "right" },

];

var colM = [
    { title: '年月', key: 'YEARMONTH', width: 100 },
    { title: '商品编码', key: 'GOODSDM', width: 95 },
    { title: '商品条码', key: 'BARCODE', width: 130 },
    { title: '商品名称', key: 'GOODSNAME', width: 120 },
    { title: '租约号', key: 'CONTRACTID', width: 95 },
    { title: '商户编码', key: 'MERCHANTID', width: 90 },
    { title: '商户名称', key: 'MERCHANTNAME', width: 200 },
    { title: '分类编码', key: 'KINDCODE', width: 100 },
    { title: '分类名称', key: 'KINDNAME', width: 100 },
    { title: '品牌', key: 'BRANDNAME', width: 100 },
    { title: '销售金额', key: 'AMOUNT', width: 120, align: "right" },
    { title: '销售成本', key: 'COST', width: 120, align: "right" },
    { title: '折扣金额', key: 'DIS_AMOUNT', width: 100, align: "right" },
    { title: '优惠金额', key: 'PER_AMOUNT', width: 100, align: "right" },

];

var echartTypeList = [{ label: "按租约", value: "CONTRACTID" },
                     { label: "按商户", value: "MERCHANTNAME" },
                     { label: "按商品", value: "GOODSNAME" },
                     { label: "按分类", value: "KINDNAME" },
                     { label: "按品牌", value: "BRANDNAME" }];

srch.beforeVue = function () {
    srch.screenParam.colDef = colD;
    srch.service = "ReportService";
    srch.method = "GoodsSale";
    srch.echartResult = true;

    srch.screenParam.showPopMerchant = false;
    srch.screenParam.srcPopMerchant = __BaseUrl + "/Pop/Pop/PopMerchantList/";
    srch.screenParam.showPopContract = false;
    srch.screenParam.srcPopContract = __BaseUrl + "/Pop/Pop/PopContractList/";
    srch.screenParam.showPopBrand = false;
    srch.screenParam.srcPopBrand = __BaseUrl + "/Pop/Pop/PopBrandList/";

    srch.screenParam.popParam = {};
    srch.screenParam.KINDID = [];
    srch.searchParam.SrchTYPE = 1;

    srch.screenParam.echartType = echartTypeList.concat({ label: "按日期", value: "RQ" });
    srch.screenParam.echartRadioVal = "CONTRACTID";
    srch.screenParam.dataSumTypeList = [{ label: "销售金额", value: "AMOUNT" }, { label: "销售成本", value: "COST" },
                                        { label: "折扣金额", value: "DIS_AMOUNT" }, { label: "优惠金额", value: "PER_AMOUNT" }];
    srch.screenParam.echartData = [];
};
srch.newCondition = function () {
    srch.searchParam.SrchTYPE = 1;
    srch.screenParam.echartData = [];
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
};

srch.otherMethods = {
    SelMerchant: function () {
        srch.screenParam.showPopMerchant = true;
    },
    SelContract: function () {
        srch.screenParam.showPopContract = true;
    },
    SelBrand: function () {
        srch.screenParam.showPopBrand = true;
    },
    changeKind: function (value, selectedData) {
        srch.screenParam.KINDID = value[value.length - 1];
    },
    changeSrchType: function (value) {
        srch.screenParam.echartData = [];
        if (value == 1) {
            Vue.set(this, "data", []);
            Vue.set(this, "pagedataCount", 0);    //清空分页数据
            Vue.set(srch.screenParam, "colDef", colD);
            Vue.set(srch, "method", "GoodsSale");
            srch.screenParam.echartType = echartTypeList.concat({ label: "按日期", value: "RQ" });
            srch.screenParam.echartRadioVal = "RQ";
        } else {
            Vue.set(this, "data", []);
            Vue.set(this, "pagedataCount", 0);    //清空分页数据
            Vue.set(srch.screenParam, "colDef", colM);
            Vue.set(srch, "method", "GoodsSaleM");
            srch.screenParam.echartType = echartTypeList.concat({ label: "按年月", value: "YEARMONTH" });
            srch.screenParam.echartRadioVal = "YEARMONTH";
        }
    },
};

srch.popCallBack = function (data) {

    if (srch.screenParam.showPopMerchant) {
        srch.screenParam.showPopMerchant = false;
        for (var i = 0; i < data.sj.length; i++) {
            srch.searchParam.MERCHANTID = data.sj[i].MERCHANTID;
            srch.searchParam.MERCHANTNAME = data.sj[i].NAME;
        }
    }

    if (srch.screenParam.showPopContract) {
        srch.screenParam.showPopContract = false;
        for (var i = 0; i < data.sj.length; i++) {
            srch.searchParam.CONTRACTID = data.sj[i].CONTRACTID;
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
};