search.beforeVue = function () {
    var col = [
        { title: "门店名称", key: "BRANCHNAME", width: 200 },
        { title: "终端号", key: "POSNO", width: 100 },
        { title: "类型", key: "TYPENAME", width: 100 },
    ];
    search.screenParam.colDef = col.concat(search.colMul);
    search.service = "XtglService";
    search.method = "GetStationList";
}



////获取父页面参数
search.popInitParam = function (data) {
    if (data) {
        search.searchParam.SqlCondition = data.SqlCondition;
    }
}