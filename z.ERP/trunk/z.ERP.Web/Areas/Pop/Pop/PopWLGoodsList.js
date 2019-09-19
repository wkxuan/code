search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "供应商编号", key: "MERCHANTID", width: 100 },
        { title: "供应商名称", key: "GHSNAME", width: 200 },
        { title: "物料编号", key: "GOODSDM", width: 100 },
        { title: "物料名称", key: "NAME", width: 200 },
        { title: "状态", key: "STATUSMC", width: 80 },
        { title: "含税采购价", key: "TAXINPRICE", width: 100 },
        { title: "使用价", key: "USEPRICE", width: 100 }
    ];
    search.service = "WyglService";
    search.method = "GetWlGoods";
}
search.initSearchParam = function () {
    search.searchParam.MERCHANTID = "";
    search.searchParam.NAME = "";
    search.searchParam.GOODSDM = "";
    search.searchParam.GOODSNAME = "";
}

