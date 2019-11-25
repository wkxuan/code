search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "单据编号", key: "BILLID",  sortable: true },
        { title: "商户代码", key: "MERCHANTID",  sortable: true },
        { title: "商户名称", key: "MERCHANTNAME", width: 200 },
        { title: "权债发生月", key: "NIANYUE", sortable: true },
        { title: "付款方式", key: "FKFSNAME" },
        { title: "状态", key: "STATUSMC"},
        { title: "门店名称", key: "BRANCHNAME", width: 250 },
        { title: "登记人", key: "REPORTER_NAME"},
        { title: "登记时间", key: "REPORTER_TIME"},
        { title: "审核人", key: "VERIFY_NAME"},
        { title: "审核时间", key: "VERIFY_TIME"},
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
                _.OpenPage({
                    id: 10700401,
                    title: '预收款收取单详情',
                    url: "JSGL/BILL_OBTAIN_YSK/Bill_Obtain_YskEdit/" + row.BILLID
                });
            }
        }
    ];
    search.service = "JsglService";
    search.method = "GetBillObtainList";
}
search.newCondition = function () {
    search.searchParam.TYPE = 1;
    search.searchParam.BILLID = "";
    search.searchParam.BRANCHID = "";
    search.searchParam.STATUS = "";
    search.searchParam.MERCHANTID = "";
    search.searchParam.MERCHANTNAME = "";
    search.searchParam.NIANYUE = "";
    search.searchParam.FKFSID = "";
    search.searchParam.REPORTER = "";
    search.searchParam.REPORTER_NAME = "";
    search.searchParam.REPORTER_TIME_START = "";
    search.searchParam.REPORTER_TIME_END = "";
    search.searchParam.VERIFY = "";
    search.searchParam.VERIFY_NAME = "";
    search.searchParam.VERIFY_TIME_START = "";
    search.searchParam.VERIFY_TIME_END = "";
    search.searchParam.SHOPDM = "";
}
search.addHref = function (row) {
    _.OpenPage({
        id: 10700401,
        title: '添加预收款收取单',
        url: "JSGL/BILL_OBTAIN_YSK/Bill_Obtain_YskEdit/"
    });
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
                    search.searchParam.REPORTER = data.sj[i].USERID;
                    search.searchParam.REPORTER_NAME = data.sj[i].USERNAME;
                    break;
                case "选择审核人":
                    search.searchParam.VERIFY = data.sj[i].USERID;
                    search.searchParam.VERIFY_NAME = data.sj[i].USERNAME;
                    break
                case "选择商户":
                    search.searchParam.MERCHANTID = data.sj[i].MERCHANTID;
                    search.searchParam.MERCHANTNAME = data.sj[i].NAME;
                    break
            }
        }
    }
};
search.mountedInit = function () {
    search.btnConfig = [{
        id: "search",
        authority: "10700400"
    }, {
        id: "clear",
        authority: "10700400"
    }, {
        id: "add",
        authority: "10700401"
    }, {
        id: "del",
        authority: "10700401"
    }];
};
