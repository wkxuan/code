search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "商户代码", key: 'MERCHANTID', width: 105, sortable: true },
        { title: '商户名称', key: 'NAME', width: 200 },
        { title: '物料代码', key: 'GOODSDM', width: 100, sortable: true },
        { title: '物料名称', key: 'GOODSNAME', width: 100, sortable: true },
        { title: '库存数量', key: 'QTY', width: 100, sortable: true },
        { title: '库存金额', key: 'TAXAMOUNT', width: 100, sortable: true },
    ];
    search.service = "WyglService";
    search.method = "WLSrchStock";
    search.indexShow = true;
    search.selectionShow = false;
};
search.mountedInit = function () {
    search.btnConfig = [{
        id: "search",
        authority: ""
    }, {
        id: "clear",
        authority: ""
    }];
}
search.newCondition = function () {
    search.searchParam.MERCHANTID = "";
    search.searchParam.NAME = "";
    search.searchParam.GOODSDM = "";
    search.searchParam.GOODSNAME = "";
};