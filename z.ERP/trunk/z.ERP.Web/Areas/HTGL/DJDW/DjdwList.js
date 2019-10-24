search.beforeVue = function () {
    search.service = "HtglService";
    search.method = "GetContract";

    search.screenParam.colDef = [
        { title: '租约号', key: 'CONTRACTID', width: 95, sortable: true },
        { title: '状态', key: 'STATUSMC', width: 80 },
        { title: '资产代码', key: 'SHOPDM', width: 110, sortable: true },
        { title: "商户代码", key: 'MERCHANTID', width: 105, sortable: true },
        { title: '商户名称', key: 'MERNAME', width: 200, ellipsis: true },
        { title: '登记人', key: 'REPORTER_NAME', width: 90 },
        { title: '登记时间', key: 'REPORTER_TIME', width: 150, sortable: true },
        { title: '审核人', key: 'VERIFY_NAME', width: 90, },
        { title: '审核时间', key: 'VERIFY_TIME', width: 150, sortable: true },
        { title: "门店代码", key: 'BRANCHID', width: 90 },
        { title: '门店名称', key: 'NAME', width: 150 },
        {
            title: '操作', key: 'operate', authority: "10600503", onClick: function (index, row, data) {
                _.OpenPage({
                    id: 10600501,
                    title: '多经点位租约详情',
                    url: "HTGL/DJDW/DjdwEdit/" + row.CONTRACTID
                });
            }
        }
    ];
}
search.newCondition = function () {
    search.searchParam.STYLE = "3";
    search.searchParam.BRANCHID = "";
    search.searchParam.MERCHANTID = "";
    search.searchParam.MERCHANTNAME = "";
    search.searchParam.CONTRACTID = "";
    search.searchParam.SHOPDM = "";
    search.searchParam.REPORTER_NAME = "";
    search.searchParam.VERIFY_NAME = "";
}

search.otherMethods = {
    SelReporter: function () {
        search.screenParam.popParam = {};
        search.popConfig.title = "选择登记人";
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopSysuserList/";
        search.popConfig.open = true;
    },
    SelVerify: function () {
        search.screenParam.popParam = {};
        search.popConfig.title = "选择审核人";
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopSysuserList/";
        search.popConfig.open = true;
    },
    SelMerchant: function () {
        search.screenParam.popParam = {};
        search.popConfig.title = "选择商户";
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopMerchantList/";
        search.popConfig.open = true;
    }
}

//接收子页面返回值
search.popCallBack = function (data) {
    if (search.popConfig.open) {
        search.popConfig.open = false;
        for (var i = 0; i < data.sj.length; i++) {
            switch (search.popConfig.title) {
                case "选择登记人":
                    search.searchParam.REPORTER_NAME = data.sj[i].USERNAME;
                    break;
                case "选择审核人":
                    search.searchParam.VERIFY_NAME = data.sj[i].USERNAME;
                    break
                case "选择商户":
                    search.searchParam.MERCHANTID = data.sj[i].MERCHANTID;
                    search.searchParam.MERCHANTNAME = data.sj[i].NAME;
                    break;
            }
        }
    }
};

search.addHref = function (row) {
    _.OpenPage({
        id: 10600401,
        title: '添加多经点位租约',
        url: "HTGL/DJDW/DjdwEdit/"
    });
}