search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "单据编号", key: "ID",  sortable: true },
        { title: "调整开始时间", key: "STARTTIME",  sortable: true },
        { title: "调整结束时间", key: "ENDTIME",sortable: true },
        { title: "状态", key: "STATUSMC", },
        { title: "门店名称", key: "BRANCHNAME", width: 250},
        { title: "登记人", key: "REPORTER_NAME" },
        { title: "登记时间", key: "REPORTER_TIME"},
        { title: "审核人", key: "VERIFY_NAME"},
        { title: "审核时间", key: "VERIFY_TIME"},
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
                _.OpenPage({
                    id: 10500701,
                    title: '扣率调整单',
                    url: "SPGL/RATE_ADJUST/Rate_AdjustEdit/" + row.ID
                });
            }
        }
    ];
    search.service = "SpglService";
    search.method = "GetRateAdjustList";
}
search.newCondition = function () {
    search.searchParam.ID = "";
    search.searchParam.BRANCHID = "";
    search.searchParam.STATUS = "";
    search.searchParam.DATE_START = "";
    search.searchParam.DATE_END = "";
    search.searchParam.REPORTER = "";
    search.searchParam.REPORTER_NAME = "";
    search.searchParam.REPORTER_TIME_START = "";
    search.searchParam.REPORTER_TIME_END = "";
    search.searchParam.VERIFY = "";
    search.searchParam.VERIFY_NAME = "";
    search.searchParam.VERIFY_TIME_START = "";
    search.searchParam.VERIFY_TIME_END = "";
};
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
    }
}

search.addHref = function (row) {
    _.OpenPage({
        id: 10500701,
        title: '添加扣率调整单',
        url: "SPGL/RATE_ADJUST/Rate_AdjustEdit/"
    })
}

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
            }
        }
    }
};

