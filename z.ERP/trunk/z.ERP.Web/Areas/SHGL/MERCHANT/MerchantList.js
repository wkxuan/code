search.beforeVue = function () {
    var col = [
        {
            title: "商户代码",
            key: 'MERCHANTID', width: 150
        },
        {
            title: '商户名称',
            key: 'NAME', width: 250
        },
    ];
    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "ShglService";
    search.method = "GetMerchant";
}

