search.beforeVue = function () {
    var col = [
        { title: "单据编号", key: "BILLID", width: 100 },
        { title: "分店编号", key: "BRANCHID", width: 100 },
        { title: "分店名称", key: "BRANCHNAME", width: 200 },
        { title: "债权发生月", key: "NIANYUE", width: 100 },
        { title: "收付实现月", key: "YEARMONTH", width: 100 },
        { title: "开始日期", key: "START_DATE", width: 100 },
        { title: "结束日期", key: "END_DATE", width: 100 },
        { title: "状态", key: "STATUSMC", width: 100 },
        { title: "登记人", key: "REPORTER_NAME", width: 100 },
        { title: "登记时间", key: "REPORTER_TIME", width: 100 },
        { title: "审核人", key: "VERIFY_NAME", width: 100 },
        { title: "审核时间", key: "VERIFY_TIME", width: 100 }
    ];
    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "JsglService";
    search.method = "GetBillAdjustList";
}

search.browseHref = function (row, index) {
    _.OpenPage("JSGL/BILL_ADJUST/Bill_AdjustDetail/" + row.BILLID, function (data) {
    })
}

search.modHref = function (row, index) {
    _.OpenPage("JSGL/BILL_ADJUST/Bill_AdjustEdit/" + row.BILLID, function (data) {
    });
}

search.addHref = function (row) {
    _.OpenPage("JSGL/BILL_ADJUST/Bill_AdjustEdit/", function (data) {
    });
}

