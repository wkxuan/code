search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: '门店编号', key: 'BRANCHID', width: 120 },
        { title: '门店名称', key: 'NAME', tooltip: true },
        { title: '赠品编号', key: 'ID', tooltip: true },
        { title: '赠品名称', key: 'NAME', tooltip: true },
        { title: '价格', key: 'PRICE', tooltip: true },
        { title: '状态', key: 'STATUSMC', tooltip: true }];
    search.service = "CxglService";
    search.method = "PresentSql";
}
////获取父页面参数
search.popInitParam = function (data) {
    if (data) {
        search.searchParam.BRANCHID = data.BRANCHID;
    }
}