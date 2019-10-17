search.beforeVue = function () {
    search.screenParam.colDef = [
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
    search.service = "ReportService";
    search.method = "SaleRecord";
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

    search.screenParam.echartType = [{ label: "按终端号", value: "POSNO" },
                      { label: "按交易时间", value: "SALE_TIME" },
                     { label: "按记账日期", value: "ACCOUNT_DATE" },
                     { label: "按收款员", value: "CASHIERNAME" }];;
    search.screenParam.echartRadioVal = "POSNO";
    search.screenParam.dataSumTypeList = [{ label: "收款金额", value: "SALE_AMOUNT" }];
    search.screenParam.echartData = [];
};
search.newCondition = function () {
    search.searchParam.BRANCHID = [];
    search.searchParam.POSNO = "";
    search.searchParam.SALE_TIME_START = "";
    search.searchParam.SALE_TIME_END = "";
    search.searchParam.MERCHANTID = "";
    search.searchParam.MERCHANTNAME = "";
    search.searchParam.SHOPID = "";
    search.searchParam.SHOPNAME = "";
    search.searchParam.ACCOUNT_DATE_START = "";
    search.searchParam.ACCOUNT_DATE_END = "";
};

search.searchDataAfter = function (data) {
    search.screenParam.echartData = data;
};

search.otherMethods = {
    SelMerchant: function () {
        search.screenParam.popParam = {};
        search.popConfig.title = "选择商户";
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopMerchantList/";
        search.popConfig.open = true;
    },
    SelShop: function () {
        search.screenParam.popParam = {};
        search.popConfig.title = "选择店铺";
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopShopList/";
        search.popConfig.open = true;
    }
}
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
search.popCallBack = function (data) {
    if (search.popConfig.open) {
        search.popConfig.open = false;
        switch (search.popConfig.title) {
            case "选择商户":
                search.searchParam.MERCHANTID = $.map(data.sj, item => {
                    return item.MERCHANTID
                }).join(',');
                search.searchParam.MERCHANTNAME = $.map(data.sj, item => {
                    return item.NAME
                }).join(',');
                break;
            case "选择店铺":
                search.searchParam.SHOPID = $.map(data.sj, item => {
                    return item.SHOPID
                }).join(',');
                search.searchParam.SHOPNAME = $.map(data.sj, item => {
                    return item.NAME
                }).join(',');
                break;
        }
    }
}