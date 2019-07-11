search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "单据编号", key: "ID", width: 100, sortable: true },
        { title: "调整开始时间", key: "STARTTIME", width: 150, sortable: true },
        { title: "调整结束时间", key: "ENDTIME", width: 150, sortable: true },
        { title: "状态", key: "STATUSMC", width: 100 },
        { title: "分店编号", key: "BRANCHID", width: 100, sortable: true },
        { title: "分店名称", key: "BRANCHNAME", width: 150, sortable: true },
        { title: "登记人", key: "REPORTER_NAME", width: 90 },
        { title: "登记时间", key: "REPORTER_TIME", width: 150 },
        { title: "审核人", key: "VERIFY_NAME", width: 90 },
        { title: "审核时间", key: "VERIFY_TIME", width: 150 },
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
    search.screenParam.showPopSysuser = false;
    search.screenParam.srcPopSysuser = __BaseUrl + "/" + "Pop/Pop/PopSysuserList/";
}

search.otherMethods = {
    SelSysuser: function () {
        search.screenParam.showPopSysuser = true;
        btnFlag = "REPORTER";
    },
    SelSysuser_sh: function () {
        search.screenParam.showPopSysuser = true;
        btnFlag = "VERIFY";
    },
}

search.addHref = function (row) {
    _.OpenPage({
        id: 10500701,
        title: '扣率调整单',
        url: "SPGL/RATE_ADJUST/Rate_AdjustEdit/"
    })
}
//接收子页面返回值
search.popCallBack = function (data) {
    if (search.screenParam.showPopSysuser) {
        search.screenParam.showPopSysuser = false;
        for (var i = 0; i < data.sj.length; i++) {
            if (btnFlag == "REPORTER") {
                search.searchParam.REPORTER = data.sj[i].USERID;
                search.searchParam.REPORTER_NAME = data.sj[i].USERNAME;
            }
            else if (btnFlag == "VERIFY") {
                search.searchParam.VERIFY = data.sj[i].USERID;
                search.searchParam.VERIFY_NAME = data.sj[i].USERNAME;
            }

        };
    }
};

