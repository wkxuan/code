search.beforeVue = function () {
    search.searchParam.STYLE = "3";
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

    search.screenParam.showPopMerchant = false;
    search.screenParam.srcPopMerchant = __BaseUrl + "/Pop/Pop/PopMerchantList/";

    search.screenParam.showPopSysuser = false;
    search.screenParam.srcPopSysuser = __BaseUrl + "/Pop/Pop/PopSysuserList/";

    search.screenParam.popParam = {};
}

search.otherMethods = {
    SelReporter: function () {
        search.screenParam.showPopSysuser = true;
        btnFlag = "REPORTER";
        search.screenParam.popParam = {};
    },
    SelVerify: function () {
        search.screenParam.showPopSysuser = true;
        btnFlag = "VERIFY";
        search.screenParam.popParam = {};
    },
    SelMerchant: function () {
        search.screenParam.showPopMerchant = true;
    }
}

//接收子页面返回值
search.popCallBack = function (data) {
    if (search.screenParam.showPopSysuser) {
        search.screenParam.showPopSysuser = false;
        for (var i = 0; i < data.sj.length; i++) {
            if (btnFlag == "REPORTER") {
                search.searchParam.REPORTER = data.sj[i].USERID;
                search.searchParam.REPORTER_NAME = data.sj[i].USERNAME;
            }
            else if (btnFlag == "VERIFY") {
                search.searchParam.VERIFY = data.sj[i].USERID;
                search.searchParam.VERIFY_NAME = data.sj[i].USERNAME;
            };
        };
    };

    if (search.screenParam.showPopMerchant) {
        search.screenParam.showPopMerchant = false;
        for (var i = 0; i < data.sj.length; i++) {
            search.searchParam.MERCHANTID = data.sj[i].MERCHANTID;
            search.searchParam.MERCHANTNAME = data.sj[i].NAME;
        };
    };
};

search.addHref = function (row) {
    _.OpenPage({
        id: 10600401,
        title: '多经点位租约详情',
        url: "HTGL/DJDW/DjdwEdit/"
    });
}