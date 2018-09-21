search.beforeVue = function () {
    var col = [
        { title: "单据编号", key: "BILLID", width: 100 },
        { title: "记账日期", key: "ACCOUNT_DATE", width: 150 },
        { title: "收银员", key: "SYYMC", width: 100 },
        { title: "营业员", key: "YYYMC", width: 100 },
        { title: "状态", key: "STATUSMC", width: 100 },
        { title: "分店编号", key: "BRANCHID", width: 100 },
        { title: "分店名称", key: "BRANCHMC", width: 150 },
        { title: "登记人", key: "REPORTER_NAME", width: 90 },
        { title: "登记时间", key: "REPORTER_TIME", width: 150 },
        { title: "审核人", key: "VERIFY_NAME", width: 90 },
        { title: "审核时间", key: "VERIFY_TIME", width: 150 }
    ];
    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "SpglService";
    search.method = "GetSaleBillList";
    //账单收款
    search.searchParam.TYPE = 3;
}

search.browseHref = function (row, index) {
    _.OpenPage({
        id: 10500401,
        title: '浏览销售补录单',
        url: "SPGL/SALEBILL/SaleBillDetail/" + row.BILLID
    })
}

search.modHref = function (row, index) {
    _.OpenPage({
        id: 10500401,
        title: '编辑销售补录单',
        url: "SPGL/SALEBILL/SaleBillEdit/" + row.BILLID
    })

}

search.addHref = function (row) {
    _.OpenPage({
        id: 10500401,
        title: '新增销售补录单',
        url: "SPGL/SALEBILL/SaleBillEdit/"
    })
}


