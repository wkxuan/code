search.beforeVue = function () {
    search.searchParam.SALEGATHER = "";
    var col = [
        { title: '门店', key: 'NAME', width: 150 },
        { title: '交易开始时间', key: 'SALETIME', width: 150 },
        { title: '交易结束时间', key: 'SALETIME', width: 150 },
        { title: '交易号', key: 'DEALID', width: 90 },
        { title: '上传开始时间', key: 'CREATE_TIME', width: 150 },
        { title: '上传结束时间', key: 'CREATE_TIME', width: 150 },
        { title: '终端号', key: 'STATIONBH', width: 90 },
        { title: '处理标记', key: 'FLAGMC', width: 100 },
        { title: '失败原因', key: 'REASON', width: 320 }
    ];
    search.indexShow = true;
    search.selectionShow = false;
    search.screenParam.colDef = col;
    search.service = "ReportService";
    search.method = "SALEGATHER";
};

search.newCondition = function () {
    search.searchParam.BRANCHID = [];
    search.searchParam.SALETIME_START = "";
    search.searchParam.SALETIME_END = "";
    search.searchParam.CREATE_TIME_START = "";
    search.searchParam.CREATE_TIME_END = "";
    search.searchParam.STATIONBH = "";
    search.searchParam.DEALID = "";
    search.searchParam.FLAG = [];
    search.searchParam.REASON = "";
};

search.mountedInit = function () {
    search.btnConfig = [{
        id: "search",
        authority: ""
    }, {
        id: "clear",
        authority: ""
    }, {
        id: "export",
        authority: ""
    }];
}