search.beforeVue = function () {
    var col = [
        { title: "供应商编号", key: "MERCHANTID", width: 100 },
        { title: "供应商名称", key: "GHSNAME", width: 200 },
        { title: "物料编号", key: "GOODSDM", width: 100 },
        { title: "物料名称", key: "NAME", width: 200 },
        { title: "状态", key: "STATUSMC", width: 80 },
        { title: "含税采购价", key: "TAXINPRICE", width: 100 },
        { title: "使用价", key: "USEPRICE", width: 100 }
    ];
    search.screenParam.colDef = col.concat(search.colMul);
    search.service = "WyglService";
    search.method = "GetWlGoods";
}


search.popInitParam = function (data) {
    if (data) {
        search.searchParam.MERCHANTID = data.MERCHANTID;
    }
}


