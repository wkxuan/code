srch.beforeVue = function () {
    srch.searchParam.CONTRACTID = "";
    var col = [
        { title: '日期', key: 'RQ', width: 100 },
        { title: '租约号', key: 'CONTRACTID', width: 100 },
        { title: '商户编号', key: 'MERCHANTID', width: 100 },
        { title: '商户名称', key: 'MERCHANTNAME', width: 200 },
        { title: '店铺编号', key: 'SHOPCODE', width: 200 },
        { title: '店铺名称', key: 'SHOPNAME', width: 200 },
        { title: '销售金额', key: 'AMOUNT', width: 150 },
        { title: '折扣金额', key: 'DIS_AMOUNT', width: 150 },
        { title: '优惠金额', key: 'PER_AMOUNT', width: 150 },

    ];
    srch.screenParam.colDef = col;
    srch.service = "ReportService";
    srch.method = "ContractSale";
};

