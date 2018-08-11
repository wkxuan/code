search.beforeVue = function () {
    var col = [
        { title: "分店名称", key: 'BRANCHNAME', width: 200 },
        { title: '楼层名称', key: 'FLOORNAME', width: 200 },
        { title: '单元代码', key: 'CODE', width: 100 },
        { title: '单元名称', key: 'NAME', width: 150 }
    ];
    search.screenParam.colDef = col.concat(search.colMul);
    search.service = "XtglService";
    search.method = "GetShop";
}
//获取父页面参数
search.popInitParam = function (data) {
    search.searchParam.BRANCHID = data.BRANCHID;
    search.searchParam.STATUS = data.STATUS;
}

