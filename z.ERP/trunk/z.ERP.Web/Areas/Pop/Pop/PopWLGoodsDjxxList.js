search.beforeVue = function () {
    var col = [
        { title: "业务单单号", key: "DH", width: 100 },
        { title: "业务类型", key: "LXMC", width: 100 },
        { title: "物料编号", key: "GOODSDM", width: 100 },
        { title: "物料名称", key: "NAME", width: 200 },
        { title: "状态", key: "STATUSMC", width: 80 },
        { title: "含税采购价", key: "TAXINPRICE", width: 100 },
        { title: "数量", key: "QUANTITY", width: 100 }
    ];
    search.screenParam.colDef = col.concat(search.colMul);
    search.service = "WyglService";
    search.method = "GetWlGoodsDjxx";
}


search.popInitParam = function (data) {
    if (data) {
        search.searchParam.MERCHANTID = data.MERCHANTID;
    }
}


