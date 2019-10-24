search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "单据号", key: "BILLID", width: 95, sortable: true },
        { title: "门店号", key: "BRANCHID", width: 95, sortable: true },
        { title: "门店名称", key: "BRANCHMC", width: 105, sortable: true },
        { title: "登记人", key: "REPORTER_NAME", width: 100 },
        { title: "登记时间", key: "REPORTER_TIME", width: 150, sortable: true },
        { title: "审核人", key: "VERIFY_NAME", width: 100 },
        { title: "审核时间", key: "VERIFY_TIME", width: 150, sortable: true },
        { title: "状态", key: "STATUSMC", width: 100 },
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
                _.OpenPage({
                    id: 103007,
                    title: '店铺投诉单',
                    url: "WYGL/COMPLAIN/ComPlainEdit/" + row.BILLID
                });
            }
        }
    ]
    search.service = "WyglService";
    search.method = "GetComPlain";
}
search.newCondition = function () {
    search.searchParam.BILLID = "";
    search.searchParam.STATUS = "";
    search.searchParam.REPORTER = "";
    search.searchParam.REPORTERNAME = "";
    search.searchParam.REPORTER_TIME_START = "";
    search.searchParam.REPORTER_TIME_END = "";
};
search.addHref = function (row) {
    _.OpenPage({
        id: 103007,
        title: '添加店铺投诉单',
        url: "WYGL/COMPLAIN/ComPlainEdit/"
    });
}

search.otherMethods = {
    SelReporter: function () {
        search.screenParam.popParam = {};
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopSysuserList/";
        search.popConfig.title = "选择登记人";
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
                    search.searchParam.REPORTERNAME = data.sj[i].USERNAME;
                    break;
            }
        }
    }
};