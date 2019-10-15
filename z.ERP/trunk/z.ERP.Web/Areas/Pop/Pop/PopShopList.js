search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "门店名称", key: 'BRANCHNAME', width: 200 },
        { title: '楼层名称', key: 'FLOORNAME', width: 200 },
        { title: '单元代码', key: 'CODE', width: 100 },
        { title: '单元名称', key: 'NAME', width: 150 }
    ];
    search.service = "XtglService";
    search.method = "GetShop";
}
search.initSearchParam = function () {
    search.searchParam.CODE = "";
    search.searchParam.NAME = "";
    search.searchParam.RENT_STATUS = "";
    search.searchParam.STATUS = "";
}
