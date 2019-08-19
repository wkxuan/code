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
//获取父页面参数
search.popInitParam = function (data) {
    search.searchParam.BRANCHID = data.BRANCHID;
    search.searchParam.REGIONID = data.REGIONID;
    search.searchParam.FLOORID = data.FLOORID;
    search.searchParam.STATUS = data.STATUS;
    search.searchParam.RENT_STATUS = data.RENT_STATUS;
    search.searchParam.SqlCondition = data.SqlCondition;
}

