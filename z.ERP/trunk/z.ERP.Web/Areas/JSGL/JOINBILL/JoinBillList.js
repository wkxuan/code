search.beforeVue = function () {
    var col = [
        { title: "单据编号", key: "BILLID", width: 105, sortable: true },
        { title: "租约号", key: "CONTRACTID", width: 105, sortable: true },
        { title: "商户编码", key: "MERCHANTID", width: 105, sortable: true },
        { title: "商户名称", key: "MERCHANTNAME", width: 200 },
        { title: "年月", key: "NIANYUE", width: 100, sortable: true },
        { title: "状态", key: "STATUSMC", width: 80 },
        { title: "门店编号", key: "BRANCHID", width: 90 },
        { title: "门店名称", key: "BRANCHNAME", width: 150 },
        { title: "登记人", key: "REPORTER_NAME", width: 90 },
        { title: "登记时间", key: "REPORTER_TIME", width: 150, sortable: true },
        { title: "审核人", key: "VERIFY_NAME", width: 90 },
        { title: "审核时间", key: "VERIFY_TIME", width: 150, sortable: true },
        
    ]


    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "JsglService";
    search.method = "GetJoinBillList";
}

search.browseHref = function (row, index) {
    _.OpenPage({
        id: 107006,
        title: '浏览联营结算单',
        url: "JSGL/JOINBILL/JoinBillDetail/" + row.BILLID
    });
}

search.modHref = function (row, index) {
    _.OpenPage({
        id: 107006,
        title: '编辑联营结算单',
        url: "JSGL/JOINBILL/JoinBillEdit/" + row.BILLID
    });
}