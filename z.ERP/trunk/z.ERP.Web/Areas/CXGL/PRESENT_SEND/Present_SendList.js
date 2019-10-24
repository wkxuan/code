search.beforeVue = function () {
    search.service = "CxglService";
    search.method = "Present_SendList";
    search.screenParam.colDef = [
        { title: '促销单号', key: 'BILLID' },
        { title: "门店代码", key: 'BRANCHID', },
        { title: '门店名称', key: 'BRANCHNAME', },
        { title: '状态', key: 'STATUSMC' },
        { title: '登记人', key: 'REPORTER_NAME', },
        { title: '登记时间', key: 'REPORTER_TIME' },
        { title: '审核人', key: 'VERIFY_NAME', },
        { title: '审核时间', key: 'VERIFY_TIME' },
        {
            title: '操作', key: 'operate', authority: "10600503", onClick: function (index, row, data) {
                _.OpenPage({
                    id: 10600501,
                    title: '赠品发放单',
                    url: "CXGL/Present_Send/Present_SendEdit/" + row.BILLID
                });
            }
        }
    ];
}
search.newCondition = function () {
    search.searchParam.BILLID = "";
    search.searchParam.BRANCHID = "";
    search.searchParam.STATUS = "";
    search.searchParam.REPORTER_NAME = "";
    search.searchParam.REPORTER_TIME_START = "";
    search.searchParam.REPORTER_TIME_END = "";
    search.searchParam.VERIFY_NAME = "";
    search.searchParam.VERIFY_TIME_START = "";
    search.searchParam.VERIFY_TIME_END = "";
};
search.otherMethods = {
    SelReporter: function () {
        search.screenParam.popParam = {};
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopSysuserList/";
        search.popConfig.title = "选择登记人";
        search.popConfig.open = true;
    },
    SelVerify: function () {
        search.screenParam.popParam = {};
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopSysuserList/";
        search.popConfig.title = "选择审核人";
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
                    break;
            }
        }
    }
};

search.addHref = function (row) {
    _.OpenPage({
        id: 10600401,
        title: '赠品发放单',
        url: "CXGL/Present_Send/Present_SendEdit/"
    });
}