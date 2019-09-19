search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "租约号", key: "CONTRACTID", width: 100 },
        { title: "商户编号", key: "MERCHANTID", width: 100 },
        { title: "商户名称", key: "MERCHANTNAME", width: 200 },
        { title: "门店名称", key: "BRANCHNAME", width: 200 },
        { title: "有效期_起", key: "CONT_START", width: 100 },
        { title: "有效期_止", key: "CONT_END", width: 100 },
        { title: "状态", key: "STATUSMC", width: 100 },
        { title: "登记人", key: "REPORTER_NAME", width: 100 },
        { title: "登记时间", key: "REPORTER_TIME", width: 100 }
    ];
    search.service = "DataService";
    search.method = "GetContract";
}
search.initSearchParam = function () {
    search.searchParam.CONTRACTID = "";
    search.searchParam.BRANCHID = "";
    search.searchParam.STATUS = "";
    search.searchParam.MERCHANTID = "";
    search.searchParam.MERCHANTNAME = "";
    search.searchParam.REPORTER = "";
    search.searchParam.REPORTER_TIME_START = "";
    search.searchParam.REPORTER_TIME_END = "";
}

