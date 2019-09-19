search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "商品代码", key: 'GOODSDM', width: 100 },
        { title: '商品名称', key: 'NAME', width: 100 },
        { title: '租约号', key: 'CONTRACTID', width: 200 },
        { title: '品牌', key: 'BRANDMC', width: 200 },
        { title: '品类代码', key: 'KINDDM', width: 200 },        
        { title: '品类', key: 'KINDMC', width: 200 },
        { title: '商铺代码', key: 'CODE', width: 100 },
        { title: '商铺名称', key: 'SPMC', width: 100 },
    ];
    search.service = "DataService";
    search.method = "GetGoodsShopList";
}
search.initSearchParam = function () {
    search.searchParam.GOODSDM = "";
    search.searchParam.NAME = "";
}