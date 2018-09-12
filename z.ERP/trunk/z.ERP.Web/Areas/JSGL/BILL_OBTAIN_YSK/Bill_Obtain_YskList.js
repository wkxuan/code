search.beforeVue = function () {
    var col = [
        { title: "单据编号", key: "BILLID", width: 105, sortable: true },
        { title: "商户代码", key: "MERCHANTID", width: 105, sortable: true },
        { title: "商户名称", key: "MERCHANTNAME", width: 200 },
        { title: "权债发生月", key: "NIANYUE", width: 115, sortable: true },
        { title: "状态", key: "STATUSMC", width: 80 },
        { title: "分店编号", key: "BRANCHID", width: 90 },
        { title: "分店名称", key: "BRANCHNAME", width: 150 },
        { title: "登记人", key: "REPORTER_NAME", width: 90 },
        { title: "登记时间", key: "REPORTER_TIME", width: 150 },
        { title: "审核人", key: "VERIFY_NAME", width: 90 },
        { title: "审核时间", key: "VERIFY_TIME", width: 150 }
    ];
    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "JsglService";
    search.method = "GetBillObtainList";
    //预收款收款
    search.searchParam.TYPE = 1;
}

search.browseHref = function (row, index) {
    _.OpenPage({
        id: 107004,
        title: '浏览预收款收取单',
        url: "JSGL/BILL_OBTAIN_YSK/Bill_Obtain_YskDetail/" + row.BILLID
    });
}

search.addHref = function (row) {
    _.OpenPage({
        id: 107004,
        title: '新增预收款收取单',
        url: "JSGL/BILL_OBTAIN_YSK/Bill_Obtain_YskEdit/"
    });
}
search.modHref = function (row, index) {
    _.OpenPage({
        id: 107004,
        title: '编辑预收款收取单',
        url: "JSGL/BILL_OBTAIN_YSK/Bill_Obtain_YskEdit/" + row.BILLID
    });
}
