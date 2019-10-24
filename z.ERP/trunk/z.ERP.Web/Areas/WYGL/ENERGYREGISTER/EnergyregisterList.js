search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "单据号", key: "BILLID", width: 95, sortable: true },
        { title: "抄表日期", key: "CHECK_DATE", width: 110, sortable: true },
        { title: "年月", key: "YEARMONTH", width: 100 },
        { title: "登记人", key: "REPORTER_NAME", width: 100 },
        { title: "登记时间", key: "REPORTER_TIME", width: 150, sortable: true },
        { title: "审核人", key: "VERIFY_NAME", width: 100 },
        { title: "审核时间", key: "VERIFY_TIME", width: 150, sortable: true },
        { title: "状态", key: "STATUSMC", width: 100 },
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
                _.OpenPage({
                    id: 103002,
                    title: '能源费用处理',
                    url: "WYGL/ENERGYREGISTER/EnergyreGisterEdit/" + row.BILLID
                });
            }
        }
    ]
    search.service = "WyglService";
    search.method = "GetEnergyreGister";
}
search.addHref = function (row) {
    _.OpenPage({
        id: 103002,
        title: '添加能源费用处理',
        url: "WYGL/ENERGYREGISTER/EnergyreGisterEdit/"
    });
}
search.newCondition = function () {
    search.searchParam.BILLID = "";
    search.searchParam.CHECK_DATE_START = "";
    search.searchParam.CHECK_DATE_END = "";
    search.searchParam.YEARMONTH_START = "";
    search.searchParam.YEARMONTH_END = "";
    search.searchParam.STATUS = "";
    search.searchParam.REPORTER = "";
    search.searchParam.REPORTERNAME = "";
    search.searchParam.REPORTER_TIME_START = "";
    search.searchParam.REPORTER_TIME_END = "";
    search.searchParam.VERIFY = "";
    search.searchParam.VERIFYNAME = "";
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
search.popCallBack = function (data) {
    if (search.popConfig.open) {
        search.popConfig.open = false;
        for (var i = 0; i < data.sj.length; i++) {
            switch (search.popConfig.title) {
                case "选择登记人":
                    search.searchParam.REPORTER = data.sj[i].USERID;
                    search.searchParam.REPORTERNAME = data.sj[i].USERNAME;
                    break;
                case "选择审核人":
                    search.searchParam.VERIFY = data.sj[i].USERID;
                    search.searchParam.VERIFYNAME = data.sj[i].USERNAME;
                    break;
            }
        }
    }
};