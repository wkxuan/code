search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: '收款方式代码', key: 'PAYID' },
        { title: '收款方式名称', key: 'NAME' },
    ];
    search.service = "XtglService";
    search.method = "GetPay";
}
search.initSearchParam = function () {
    search.searchParam.PAYID = "";
    search.searchParam.NAME = "";
}

