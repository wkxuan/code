﻿search.beforeVue = function () {
    var col = [
        { title: "单据编号", key: "BILLID", width: 100 },
        { title: "分店编号", key: "BRANCHID", width: 50 },
        { title: "分店名称", key: "BRANCHNAME", width: 100 },
        { title: "租约号", key: "CONTRACTID", width: 50 },
        { title: "商户号", key: "MERCHANTID", width: 100 },
        { title: "商户名称", key: "MERCHANTNAME", width: 100 },
        { title: "状态", key: "STATUSMC", width: 100 },
        { title: "登记人", key: "REPORTER_NAME", width: 100 },
        { title: "登记时间", key: "REPORTER_TIME", width: 100 },
        { title: "审核人", key: "VERIFY_NAME", width: 100 },
        { title: "审核时间", key: "VERIFY_TIME", width: 100 }
    ];
    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "JsglService";
    search.method = "GetBillReturnList";
}

search.browseHref = function (row, index) {
    _.OpenPage("JSGL/BILL_RETURN/Bill_ReturnDetail/" + row.BILLID, function (data) {
    })
}

search.modHref = function (row, index) {
    _.OpenPage("JSGL/BILL_RETURN/Bill_ReturnlEdit/" + row.BILLID, function (data) {
    });
}

search.addHref = function (row) {
    _.OpenPage("JSGL/BILL_RETURN/Bill_ReturnEdit/", function (data) {
    });
}
