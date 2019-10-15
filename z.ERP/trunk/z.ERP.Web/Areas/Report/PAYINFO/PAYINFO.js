search.beforeVue = function () {
    var col = [
        { title: '门店', key: 'BRANCHNAME', width: 150 },
        { title: '终端号', key: 'POSNO', width: 90 },
        { title: '交易号', key: 'DEALID', width: 90 },
        { title: '收款方式', key: 'NAME', width: 110 },
        { title: '支付金额', key: 'AMOUNT', width: 90, align: 'right' },
        { title: '交易开始时间', key: 'OPERTIME', width: 150 },
        { title: '交易结束时间', key: 'OPERTIME', width: 150 },
        { title: '流水号', key: 'SERIALNO', width: 150 },
        { title: '参考号', key: 'REFNO', width: 250 }
    ];
    search.indexShow = true;
    search.selectionShow = false;
    search.screenParam.colDef = col;
    search.service = "ReportService";
    search.method = "PAYINFO";
    search.searchParam.PAYINFO = "";
};

search.newCondition = function () {
    search.searchParam.BRANCHID = "";
    search.searchParam.POSNO = "";
    search.searchParam.DEALID = "";
    search.searchParam.PAYID = "";
    search.searchParam.AMOUNT = "";
    search.searchParam.START = "";
    search.searchParam.END = "";
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




