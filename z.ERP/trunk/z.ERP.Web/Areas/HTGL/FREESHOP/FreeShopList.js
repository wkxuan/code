search.beforeVue = function () {
    var col = [
        { title: "单据编号", key: "BILLID", width: 100 },
        { title: "分店编号", key: "BRANCHID", width: 100 },
        { title: "分店名称", key: "BRANCHNAME", width: 150 },
        { title: "租约", key: "CONTRACTID", width: 100 },
        { title: "商户号", key: "MERCHANTID", width: 100 },
        { title: "商户名称", key: "MERCHANTNAME", width: 200 },
        { title: "退铺日期", key: "FREEDATE", width: 100 },
        { title: "状态", key: "STATUSMC", width: 100 },
        { title: "登记人", key: "REPORTER_NAME", width: 100 },
        { title: "登记时间", key: "REPORTER_TIME", width: 100 },
        { title: "审核人", key: "VERIFY_NAME", width: 100 },
        { title: "审核时间", key: "VERIFY_TIME", width: 100 },
        { title: "终止人", key: "TERMINATE_NAME", width: 100 },
        { title: "终止时间", key: "TERMINATE_TIME", width: 100 }
    ];
    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "HtglService";
    search.method = "GetFreeShopList";
    //账单收款
    search.searchParam.TYPE = 3;
}

search.browseHref = function (row, index) {
    _.OpenPage("HTGL/FREESHOP/FreeShopDetail/" + row.BILLID, function (data) {
    })
}

search.modHref = function (row, index) {
    _.OpenPage("HTGL/FREESHOP/FreeShopEdit/" + row.BILLID, function (data) {
    });
}

search.addHref = function (row) {
    _.OpenPage("HTGL/FREESHOP/FreeShopEdit/", function (data) {
    });
}


