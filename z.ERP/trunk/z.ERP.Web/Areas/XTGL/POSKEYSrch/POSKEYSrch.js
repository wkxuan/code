srch.beforeVue = function () {
    srch.searchParam.POSKEYSrch = "";
    var col = [
        { title: '门店', key: 'NAME', width: 120 },
        { title: '终端号', key: 'STATIONBH', width: 100 },
     { title: '密钥', key: 'ENCRYPTION', width: 300 }
    ];
    srch.screenParam.colDef = col;
    srch.service = "XtglService";
    srch.method = "POSKEYSrch";
};




