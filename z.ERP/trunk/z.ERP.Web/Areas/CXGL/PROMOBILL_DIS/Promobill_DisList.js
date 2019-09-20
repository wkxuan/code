search.beforeVue = function () {
    search.service = "CxglService";
    search.method = "GetPromobill";
    search.screenParam.colDef = [
        { title: '促销单号', key: 'BILLID' },
        { title: '营销活动', key: 'PROMOTIONNAME'},
        { title: "开始日期", key: 'START_DATE', cellType: "date", width: 120 },
        { title: '结束日期', key: 'END_DATE', cellType: "date", width: 120 },
        { title: '促销周期', key: 'WEEKMC', width: 240 },
        { title: '开始时间', key: 'START_TIME', cellType: "time" },
        { title: '结束时间', key: 'END_TIME', cellType: "time" },
        { title: "门店代码", key: 'BRANCHID', },
        { title: '门店名称', key: 'BRANCHNAME', },
        { title: '状态', key: 'STATUSMC' },
        { title: '登记人', key: 'REPORTER_NAME', },
        { title: '登记时间', key: 'REPORTER_TIME' },
        { title: '审核人', key: 'VERIFY_NAME', },
        { title: '审核时间', key: 'VERIFY_TIME' },
        { title: '启动人', key: 'INITINATE_NAME', },
        { title: '启动时间', key: 'INITINATE_TIME' },
        { title: '终止人', key: 'TERMINATE_NAME', },
        { title: '终止时间', key: 'TERMINATE_TIME' },
        {
            title: '操作', key: 'operate', authority: "10600503", onClick: function (index, row, data) {
                _.OpenPage({
                    id: 10600501,
                    title: '促销折扣单详情',
                    url: "CXGL/PROMOBILL_DIS/Promobill_DisEdit/" + row.BILLID
                });
            }
        }
    ];
    search.screenParam.showPop = false;
    search.screenParam.srcPop = "";
    search.screenParam.title = "";
    search.screenParam.popParam = {};
    search.searchParam.PROMOTYPE = 2;
}
search.newCondition = function () {
    search.searchParam.PROMOTYPE = 2;
};
search.otherMethods = {
    SelPromotion: function () {
        search.screenParam.srcPop = __BaseUrl + "/Pop/Pop/PopPromotionList/";
        search.screenParam.title = "选择营销活动";
        search.screenParam.showPop = true;
    },
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
            if (search.screenParam.title == "选择营销活动") {
                search.searchParam.PROMOTIONNAME = data.sj[i].NAME;
            }
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
        title: '促销折扣单详情',
        url: "CXGL/PROMOBILL_DIS/Promobill_DisEdit/"
    });
}