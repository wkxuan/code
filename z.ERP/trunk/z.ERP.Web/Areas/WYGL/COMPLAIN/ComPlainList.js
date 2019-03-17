search.beforeVue = function () {
    var col = [
        { title: "单据号", key: "BILLID", width: 95, sortable: true },
        { title: "分店号", key: "BRANCHID", width: 95, sortable: true },
        { title: "分店名称", key: "BRANCHMC", width: 105, sortable: true },
        { title: "登记人", key: "REPORTER_NAME", width: 100 },
        { title: "登记时间", key: "REPORTER_TIME", width: 150, sortable: true },
        { title: "审核人", key: "VERIFY_NAME", width: 100 },
        { title: "审核时间", key: "VERIFY_TIME", width: 150, sortable: true },
        { title: "状态", key: "STATUSMC", width: 100 },
    ]

    search.windowParam = {
        terst: false
    }

    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "WyglService";
    search.method = "GetComPlain";


    search.screenParam.showPopSysuser = false;
    search.screenParam.srcPopSysuser = __BaseUrl + "/" + "Pop/Pop/PopSysuserList/";
    search.screenParam.popParam = {};
    search.searchParam.REPORTER = "";
    search.searchParam.REPORTERNAME = "";
    search.searchParam.VERIFY = "";
    search.searchParam.VERIFYNAME = "";
}


search.browseHref = function (row, index) {
    _.OpenPage({
        id: 103007,
        title: '店铺投诉单',
        url: "WYGL/COMPLAIN/ComPlainDetail/" + row.BILLID
    });
}

search.addHref = function (row) {
    _.OpenPage({
        id: 103007,
        title: '新增店铺投诉单',
        url: "WYGL/COMPLAIN/ComPlainEdit/"
    });
}
search.modHref = function (row, index) {
    _.OpenPage({
        id: 103007,
        title: '编辑店铺投诉单',
        url: "WYGL/COMPLAIN/ComPlainEdit/" + row.BILLID
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