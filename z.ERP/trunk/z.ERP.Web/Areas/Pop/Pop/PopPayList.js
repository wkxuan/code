search.beforeVue = function () {
    var col = [
        { title: '费用代码', key: 'PAYID' },
        { title: '费用名称', key: 'NAME' },
    ];
    search.screenParam.colDef = col.concat(search.colMul);
    search.service = "XtglService";
    search.method = "GetPay";
}

