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




