search.beforeVue = function () {
    var col = [
        { title: "门店名称", key: "BRANCHNAME", width: 200 },
        { title: "终端号", key: "POSNO", width: 100 },
        { title: "类型", key: "TYPE", width: 100 },
    ];
    search.screenParam.colDef = col.concat(search.colMul);
    search.service = "XtglService";
    search.method = "GetStationList";
}