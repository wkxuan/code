search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "供应商编号", key: "MERCHANTID", width: 100 },
        { title: "供应商名称", key: "NAME", width: 200 },
        { title: "状态", key: "STATUSMC", width: 80 },
        { title: "登记人", key: "REPORTER_NAME", width: 100 },
        { title: "登记时间", key: "REPORTER_TIME", width: 150 }
    ];
    search.service = "WyglService";
    search.method = "GetWlMerchant";
}
search.initSearchParam = function () {
    search.searchParam.MERCHANTID = "";
    search.searchParam.MERCHANTNAME = "";
    search.searchParam.REPORTER = "";
    search.searchParam.REPORTER_TIME_START = "";
    search.searchParam.REPORTER_TIME_END = "";
}

