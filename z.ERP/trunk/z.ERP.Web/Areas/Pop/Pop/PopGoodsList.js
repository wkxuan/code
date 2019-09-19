search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "商品代码", key: 'GOODSDM', width: 100 },
        { title: '商品名称', key: 'NAME', width: 100 },
        { title: '租约号', key: 'CONTRACTID', width: 200 },
        { title: '品牌', key: 'BRANDMC', width: 200 },
    ];
    search.service = "DataService";
    search.method = "GetGoodsList";
}
search.initSearchParam = function () {
    search.searchParam.GOODSDM = "";
    search.searchParam.NAME = "";
}