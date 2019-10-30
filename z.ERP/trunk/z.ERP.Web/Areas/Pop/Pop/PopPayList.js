search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: '费用代码', key: 'PAYID' },
        { title: '费用名称', key: 'NAME' },
    ];
    search.service = "XtglService";
    search.method = "GetPay";
}
search.initSearchParam = function () {
    search.searchParam.PAYID = "";
    search.searchParam.NAME = "";
    search.searchParam.BRANCHID = "";
}

