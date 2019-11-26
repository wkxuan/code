search.beforeVue = function () {
    search.screenParam.colDef = [        
        { title: '门店名称', key: 'BRANCHNAME', width: 250, sortable: true },
        { title: '终端号', key: 'POSNO', width: 130, sortable: true },
        { title: '交易号', key: 'DEALID', width: 130, sortable: true },
        { title: '交易时间', key: 'SALE_TIME', width: 150, sortable: true },
        { title: '支付方式', key: 'PAYNAME', width: 130, sortable: true },
        { title: '金额', key: 'AMOUNT', width: 130, sortable: true },
        { title: '手续费', key: 'CHARGES', width: 130, sortable: true },
    ];
    search.service = "XtglService";
    search.method = "ChargesSearch";
    search.indexShow = true;
    search.selectionShow = false;
};
search.newCondition = function () {
    search.searchParam.STATIONBH = "";
    search.searchParam.BRANCHID = "";
    search.searchParam.DEALID = "";
    search.searchParam.PAY = [];
    search.searchParam.RQ_START = "";
    search.searchParam.RQ_END = "";
};
search.mountedInit = function () {
    search.btnConfig = [{
        id: "search",
        authority: ""
    }, {
        id: "clear",
        authority: ""
    }];
}
search.otherMethods = {
    
};
