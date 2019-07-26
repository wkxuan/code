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

    search.windowParam = {
        terst: false
    }

    search.service = "WyglService";
    search.method = "GetEnergyreGister";


    search.screenParam.showPopSysuser = false;
    search.screenParam.srcPopSysuser = __BaseUrl + "/" + "Pop/Pop/PopSysuserList/";
    search.screenParam.popParam = {};
    search.searchParam.REPORTER = "";
    search.searchParam.REPORTERNAME = "";
    search.searchParam.VERIFY = "";
    search.searchParam.VERIFYNAME = "";
}


search.addHref = function (row) {
    _.OpenPage({
        id: 103002,
        title: '新增能源费用处理',
        url: "WYGL/ENERGYREGISTER/EnergyreGisterEdit/"
    });
}

search.otherMethods = {
    SelSysuser: function () {
        search.screenParam.showPopSysuser = true;
        btnFlag = "REPORTER";
    },
    SelSysuser_sh: function () {
        search.screenParam.showPopSysuser = true;
        btnFlag = "VERIFY";
}
}

//接收子页面返回值
search.popCallBack = function (data) {
    search.screenParam.showPopSysuser = false;
    for (var i = 0; i < data.sj.length; i++) {
        if (btnFlag == "REPORTER") {
            search.searchParam.REPORTER = data.sj[i].USERID;
            search.searchParam.REPORTERNAME = data.sj[i].USERNAME;
        }
        else if (btnFlag == "VERIFY") {
            search.searchParam.VERIFY = data.sj[i].USERID;
            search.searchParam.VERIFYNAME = data.sj[i].USERNAME;
    }

    };
};