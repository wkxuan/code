﻿search.beforeVue = function () {
    var col = [
        { title: "单据编号", key: "BILLID", width: 105, sortable: true },
        { title: "租约", key: "CONTRACTID", width: 100, sortable: true },
        { title: "商户代码", key: "MERCHANTID", width: 105, sortable: true },
        { title: "商户名称", key: "MERCHANTNAME", width: 200 },
        { title: "退铺日期", key: "FREEDATE", width: 150, sortable: true },
        { title: "状态", key: "STATUSMC", width: 80 },
        { title: "分店编号", key: "BRANCHID", width: 90 },
        { title: "分店名称", key: "BRANCHNAME", width: 150 },
        { title: "登记人", key: "REPORTER_NAME", width: 90 },
        { title: "登记时间", key: "REPORTER_TIME", width: 150, sortable: true },
        { title: "审核人", key: "VERIFY_NAME", width: 90 },
        { title: "审核时间", key: "VERIFY_TIME", width: 150, sortable: true },
        { title: "终止人", key: "TERMINATE_NAME", width: 90 },
        { title: "终止时间", key: "TERMINATE_TIME", width: 150, sortable: true }
    ];
    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "HtglService";
    search.method = "GetFreeShopList";
    //账单收款
    search.searchParam.TYPE = 3;
}

search.browseHref = function (row, index) {
    _.OpenPage({
        id: 106003,
        title: '浏览退铺单',
        url: "HTGL/FREESHOP/FreeShopDetail/" + row.BILLID
    });
}

search.addHref = function (row) {
    _.OpenPage({
        id: 106003,
        title: '新增退铺单',
        url: "HTGL/FREESHOP/FreeShopEdit/"
    });
}
search.modHref = function (row, index) {
    _.OpenPage({
        id: 106003,
        title: '编辑退铺单',
        url: "HTGL/FREESHOP/FreeShopEdit/" + row.BILLID
    });
}

