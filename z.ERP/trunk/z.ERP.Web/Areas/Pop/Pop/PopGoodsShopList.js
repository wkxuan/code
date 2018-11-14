search.beforeVue = function () {
    var col = [
        { title: "商品代码", key: 'GOODSDM', width: 100 },
        { title: '商品名称', key: 'NAME', width: 100 },
        { title: '租约号', key: 'CONTRACTID', width: 200 },
        { title: '品牌', key: 'BRANDMC', width: 200 },
        { title: '品类代码', key: 'KINDDM', width: 200 },        
        { title: '品类', key: 'KINDMC', width: 200 },
        { title: '商铺代码', key: 'CODE', width: 100 },
        { title: '商铺名称', key: 'SPMC', width: 100 },
    ];
    search.screenParam.colDef = col.concat(search.colMul);
    search.service = "DataService";
    search.method = "GetGoodsShopList";
}


//获取父页面参数
search.popInitParam = function (data) {
    if (data) {
        search.searchParam.CONTRACTID = data.CONTRACTID;        
        search.searchParam.YYY = data.YYY;
        search.searchParam.STATUS = data.STATUS;
    }
}