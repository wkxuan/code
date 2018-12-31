srch.beforeVue = function () {
    srch.searchParam.MERCHANTID = "";
    var col = [
        { title: "商户代码", key: 'MERCHANTID', width: 105, sortable: true },
        { title: '商户名称', key: 'NAME', width: 200 },
        { title: '物料代码', key: 'GOODSDM', width: 100, sortable: true },
        { title: '物料名称', key: 'GOODSNAME', width: 100, sortable: true },
        { title: '库存数量', key: 'QTY', width: 100, sortable: true },
        { title: '库存金额', key: 'TAXAMOUNT', width: 100, sortable: true },
    ];
    srch.screenParam.colDef = col;
    srch.service = "WyglService";
    srch.method = "WLSrchStock";
};