search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "单据编号", key: "BILLID", width: 105, sortable: true },
        { title: "租约", key: "CONTRACTID", width: 100, sortable: true },
        { title: "商户代码", key: "MERCHANTID", width: 105, sortable: true },
        { title: "商户名称", key: "MERCHANTNAME", width: 200 },
        { title: "退铺日期", key: "FREEDATE", width: 150, dataType: "date", sortable: true },
        { title: "状态", key: "STATUSMC", width: 100 },
        { title: "门店编号", key: "BRANCHID", width: 90 },
        { title: "门店名称", key: "BRANCHNAME", width: 150 },
        { title: "登记人", key: "REPORTER_NAME", width: 100 },
        { title: "登记时间", key: "REPORTER_TIME", width: 150, sortable: true },
        { title: "审核人", key: "VERIFY_NAME", width: 100 },
        { title: "审核时间", key: "VERIFY_TIME", width: 150, sortable: true },
        { title: "终止人", key: "TERMINATE_NAME", width: 100 },
        { title: "终止时间", key: "TERMINATE_TIME", width: 150, sortable: true },
        {
            title: '操作', key: 'operate', authority: "10600300", onClick: function (index, row, data) {
                _.OpenPage({
                    id: 106003,
                    title: '退铺单详情',
                    url: "HTGL/FREESHOP/FreeShopEdit/" + row.BILLID
                });
            }
        }
    ];
    search.service = "HtglService";
    search.method = "GetFreeShopList";
}
search.newCondition = function () {
    search.searchParam.TYPE = "3";
    search.searchParam.BILLID = "";
    search.searchParam.BRANCHID = "";
    search.searchParam.STATUS = "";
    search.searchParam.CONTRACTID = "";
    search.searchParam.FREEDATE_START = "";
    search.searchParam.FREEDATE_END = "";
    search.searchParam.REPORTER = "";
    search.searchParam.REPORTER_TIME_START = "";
    search.searchParam.REPORTER_TIME_END = "";
    search.searchParam.VERIFY = "";
    search.searchParam.VERIFY_TIME_START = "";
    search.searchParam.VERIFY_TIME_END = "";
}
search.addHref = function (row) {
    _.OpenPage({
        id: 106003,
        title: '添加退铺单',
        url: "HTGL/FREESHOP/FreeShopEdit/"
    });
}
search.mountedInit = function () {
    search.btnConfig = [{
        id: "search",
        authority: "10600300"
    }, {
        id: "clear",
        authority: "10600300"
    }, {
        id: "add",
        authority: "10600301"
    }, {
        id: "del",
        authority: "10600301"
    }];
}

