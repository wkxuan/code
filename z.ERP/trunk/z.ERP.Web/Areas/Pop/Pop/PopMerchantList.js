search.beforeVue = function () {
    var col = [
        { title: "商户编号", key: "MERCHANTID", width: 100 },
        { title: "商户名称", key: "NAME", width: 200 },
        { title: "状态", key: "STATUSMC", width: 100 },
        { title: "登记人", key: "REPORTER_NAME", width: 100 },
        { title: "登记时间", key: "REPORTER_TIME", width: 100 }
    ];
    search.screenParam.colDef = col.concat(search.colMul);
    search.service = "ShglService";
    search.method = "GetMerchant";
}


