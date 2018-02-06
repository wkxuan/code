search.beforeVue = function () {
    var col = [
        { title: "单据号", key: "BILLID", width: 100 },
        { title: "抄表日期", key: "CHECK_DATE", width: 100 },
        { title: "年月", key: "YEARMONTH", width: 100 },
        { title: "登记人", key: "REPORTER_NAME", width: 100 },
        { title: "登记时间", key: "REPORTER_TIME", width: 100 },
        { title: "审核人", key: "VERIFY_NAME", width: 100 },
        { title: "审核时间", key: "VERIFY_TIME", width: 100 },
        { title: "状态", key: "STATUSMC", width: 100 },
    ]

    search.windowParam = {
        terst: false
    }

    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "WyglService";
    search.method = "GetEnergyreGister";
}

search.browseHref = function (row, index) {
    _.OpenPage("WYGL/ENERGYREGISTER/EnergyreGisterDetail/" + row.BILLID, function (data) {
    })
}
search.addHref = function (row) {
    _.OpenPage("WYGL/ENERGYREGISTER/EnergyreGisterEdit/", function (data) {
    })
}

search.modHref = function (row, index) {
    _.OpenPage("WYGL/ENERGYREGISTER/EnergyreGisterEdit/" + row.BILLID, function (data) {
    });
}