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
    search.screenParam.showPop = false;
    search.screenParam.srcPop = "";
    search.screenParam.title = "";
    search.screenParam.popParam = {};
}
search.otherMethods = {
    SelReporter: function () {
        search.screenParam.srcPop = __BaseUrl + "/Pop/Pop/PopSysuserList/";
        search.screenParam.title = "选择录入人";
        search.screenParam.showPop = true;
    },
    SelVerify: function () {
        search.screenParam.srcPop = __BaseUrl + "/Pop/Pop/PopSysuserList/";
        search.screenParam.title = "选择审核人";
        search.screenParam.showPop = true;
    }
}

//接收子页面返回值
search.popCallBack = function (data) {
    if (search.screenParam.showPop) {
        search.screenParam.showPop = false;
        for (let i = 0; i < data.sj.length; i++) {
            if (search.screenParam.title == "选择录入人") {
                search.searchParam.REPORTER_NAME = data.sj[i].USERNAME;
            }
            if (search.screenParam.title == "选择审核人") {
                search.searchParam.VERIFY_NAME = data.sj[i].USERNAME;
            }
        };
    }
};

search.addHref = function (row) {
    _.OpenPage({
        id: 10600401,
        title: '赠品发放单',
        url: "CXGL/Present_Send/Present_SendEdit/"
    });
}