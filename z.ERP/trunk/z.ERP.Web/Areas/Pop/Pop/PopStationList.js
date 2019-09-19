search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "门店名称", key: "BRANCHNAME", width: 200 },
        { title: "终端号", key: "POSNO", width: 100 },
        { title: "类型", key: "TYPENAME", width: 100 },
    ];
    search.service = "XtglService";
    search.method = "GetStationList";
}
search.initSearchParam = function () {
    search.searchParam.BRANCHID = "";
    search.searchParam.POSNO = "";
    search.searchParam.POSTYPE = "";
}