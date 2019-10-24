﻿search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "单据号", key: "BILLID", width: 95, sortable: true },
        { title: "进场日期", key: "MARCHINDATE", width: 110, sortable: true },
        { title: "登记人", key: "REPORTER_NAME", width: 100 },
        { title: "登记时间", key: "REPORTER_TIME", width: 150, sortable: true },
        { title: "审核人", key: "VERIFY_NAME", width: 100 },
        { title: "审核时间", key: "VERIFY_TIME", width: 150, sortable: true },
        { title: "状态", key: "STATUSMC", width: 100 },
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
                _.OpenPage({
                    id: 103005,
                    title: '商户进场管理处理',
                    url: "WYGL/MARCHINAREAR/MarchinArearEdit/" + row.BILLID
                });
            }
        }
    ];
    search.service = "WyglService";
    search.method = "GetMarchinArear";
}

search.addHref = function (row) {
    _.OpenPage({
        id: 103005,
        title: '添加商户进场管理处理',
        url: "WYGL/MARCHINAREAR/MarchinArearEdit/"
    });
}
search.newCondition = function () {
    search.searchParam.BILLID = "";
    search.searchParam.MARCHINDATE_START = "";
    search.searchParam.MARCHINDATE_END = "";
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