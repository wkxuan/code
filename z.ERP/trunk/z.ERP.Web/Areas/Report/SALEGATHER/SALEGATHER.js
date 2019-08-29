srch.beforeVue = function () {
    srch.searchParam.SALEGATHER = "";
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

    srch.screenParam.colDef = col;
    srch.service = "ReportService";
    srch.method = "SALEGATHER";
};

srch.newCondition = function () {
    srch.searchParam.BRANCHID = "";
    srch.searchParam.SALETIME_START = "";
    srch.searchParam.SALETIME_END = "";
    srch.searchParam.CREATE_TIME_START = "";
    srch.searchParam.CREATE_TIME_END = "";
    srch.searchParam.STATIONBH = "";
    srch.searchParam.DEALID = "";
    srch.searchParam.FLAG = "";
    srch.searchParam.REASON = "";
   
};




