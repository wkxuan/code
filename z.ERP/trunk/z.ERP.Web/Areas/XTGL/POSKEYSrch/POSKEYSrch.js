search.beforeVue = function () {
    search.searchParam.POSKEYSrch = "";
    var col = [
        { title: '门店', key: 'NAME', width: 180 },
        { title: '终端号', key: 'STATIONBH', width: 100 },
        { title: '密钥', key: 'ENCRYPTION', width: 300 }
    ];
    search.screenParam.colDef = col;
    search.service = "XtglService";
    search.method = "POSKEYSrch";
    search.indexShow = true;
    search.selectionShow = false;
};
search.newCondition = function () {
    search.searchParam.BRANCHID = "";
    search.searchParam.STATIONBH = ""; 
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


