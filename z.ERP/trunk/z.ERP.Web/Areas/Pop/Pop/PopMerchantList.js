﻿search.beforeVue = function () {
    var col = [
        { title: "商户编号", key: "MERCHANTID" },
        { title: "商户名称", key: "NAME" },
        { title: "状态", key: "STATUSMC" },
        { title: "登记人", key: "REPORTER_NAME" },
        { title: "登记时间", key: "REPORTER_TIME" }
    ];
    search.screenParam.colDef = col.concat(search.colMul);
    search.service = "ShglService";
    search.method = "GetMerchant";
}


