search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: '门店名称', key: 'BRANCHNAME', tooltip: true },
        { title: '赠品编号', key: 'ID', tooltip: true },
        { title: '赠品名称', key: 'NAME', tooltip: true },
        { title: '价格', key: 'PRICE', tooltip: true }];
    search.service = "CxglService";
    search.method = "Present";
}
search.initSearchParam = function () {
    search.searchParam.BRANCHID = "";
    search.searchParam.ID = "";
    search.searchParam.NAME = "";
    search.searchParam.PRICE = "";
    search.searchParam.STATUS = "";
}