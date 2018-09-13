search.beforeVue = function () {
    var col = [
        { title: "单据编号", key: "BILLID", width: 105, sortable: true },
        { title: "债权发生月", key: "NIANYUE", width: 115,sortable: true},
        { title: "收付实现月", key: "YEARMONTH", width: 115, sortable: true },
        { title: "开始日期", key: "START_DATE", width: 150},
        { title: "结束日期", key: "END_DATE", width: 150},
        { title: "状态", key: "STATUSMC", width: 80 },
        { title: "分店编号", key: "BRANCHID", width: 90 },
        { title: "分店名称", key: "BRANCHNAME", width: 100 },
        { title: "登记人", key: "REPORTER_NAME", width: 90 },
        { title: "登记时间", key: "REPORTER_TIME", width: 150, sortable: true },
        { title: "审核人", key: "VERIFY_NAME", width: 90 },
        { title: "审核时间", key: "VERIFY_TIME", width: 150, sortable: true }
    ];
    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "JsglService";
    search.method = "GetBillAdjustList";
}

search.browseHref = function (row, index) {
    _.OpenPage({
        id: 107002,
        title: '浏览费用调整单',
        url: "JSGL/BILL_ADJUST/Bill_AdjustDetail/" + row.BILLID
    });
}

search.addHref = function (row) {
    _.OpenPage({
        id: 107002,
        title: '新增费用调整单',
        url: "JSGL/BILL_ADJUST/Bill_AdjustEdit/"
    });
}
search.modHref = function (row, index) {
    _.OpenPage({
        id: 107002,
        title: '编辑费用调整单',
        url: "JSGL/BILL_ADJUST/Bill_AdjustEdit/" + row.BILLID
    });
}

