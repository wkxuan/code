srch.beforeVue = function () {
    srch.screenParam.colDef = [
        { title: '终端号', key: 'POSNO', width: 130 ,sortable:true},
        { title: '交易号', key: 'DEALID', width: 130, sortable: true },
        { title: '交易时间', key: 'SALE_TIME', width: 150, cellType: "datetime", sortable: true },
        { title: '记账日期', key: 'ACCOUNT_DATE', width: 130, cellType: "date", sortable: true },
        { title: '收款员编码', key: 'CASHIERCODE', width: 150, sortable: true },
        { title: '收款员名称', key: 'CASHIERNAME', width: 150, sortable: true },
        { title: '收款金额', key: 'SALE_AMOUNT', width: 130, align: "right", sortable: true },
        { title: '找零金额', key: 'CHANGE_AMOUNT', width: 130, align: "right", sortable: true },
        { title: '原终端号', key: 'POSNO_OLD', width: 130, sortable: true },
        { title: '原交易号', key: 'DEALID_OLD', width: 130, sortable: true },

    ];
    srch.service = "ReportService";
    srch.method = "SaleRecord";
    srch.echartResult = true;
    srch.screenParam.showPopMerchant = false;
    srch.screenParam.srcPopMerchant = __BaseUrl + "/Pop/Pop/PopMerchantList/";
    srch.screenParam.showPopShop = false;
    srch.screenParam.srcPopShop = __BaseUrl + "/Pop/Pop/PopShopList/";
    srch.screenParam.popParam = {};

    srch.screenParam.echartType = [{ label: "按终端号", value: "POSNO" },
                      { label: "按交易时间", value: "SALE_TIME" },
                     { label: "按记账日期", value: "ACCOUNT_DATE" },
                     { label: "按收款员", value: "CASHIERNAME" }];;
    srch.screenParam.echartRadioVal = "POSNO";
    srch.screenParam.dataSumTypeList = [{ label: "收款金额", value: "SALE_AMOUNT" }];
    srch.screenParam.echartData = [];

};
srch.newCondition = function () {
    srch.searchParam.BRANCHID = "";
    srch.searchParam.POSNO = "";
    srch.searchParam.SALE_TIME_START = "";
    srch.searchParam.SALE_TIME_END = "";
    srch.searchParam.MERCHANTNAME = "";
    srch.searchParam.SHOPNAME = "";
    srch.searchParam.ACCOUNT_DATE_START = "";
    srch.searchParam.ACCOUNT_DATE_END = "";
};

srch.echartInit = function (data) {
    srch.screenParam.echartData = data;
};

srch.otherMethods = {
    SelMerchant: function () {
        srch.screenParam.showPopMerchant = true;
    },
    SelShop: function () {
        srch.screenParam.showPopShop = true;
        if (srch.searchParam.BRANCHID)
            srch.screenParam.popParam = { BRANCHID: srch.searchParam.BRANCHID };
    }
}

srch.popCallBack = function (data) {
    if (srch.screenParam.showPopMerchant) {
        srch.screenParam.showPopMerchant = false;
        srch.searchParam.MERCHANTID = $.map(data.sj, item => {
            return item.MERCHANTID
        }).join(',');
        srch.searchParam.MERCHANTNAME = $.map(data.sj, item => {
            return item.NAME
        }).join(',');
    }
    debugger
    if (srch.screenParam.showPopShop) {
        srch.screenParam.showPopShop = false;
        srch.searchParam.SHOPID = $.map(data.sj, item => {
            return item.SHOPID
        }).join(',');
        srch.searchParam.SHOPNAME = $.map(data.sj, item => {
            return item.NAME
        }).join(',');
    }
}