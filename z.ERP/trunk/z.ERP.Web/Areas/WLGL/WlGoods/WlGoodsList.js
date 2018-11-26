search.beforeVue = function () {
    search.searchParam.MERCHANTID = "";
    var col = [
        { title: '状态', key: 'STATUSMC', width: 80 },
        { title: "供货商代码", key: 'MERCHANTID', width: 105, sortable: true },
        { title: '供货商名称', key: 'GHSNAME', width: 200 },
        { title: '物料代码', key: 'GOODSDM', width: 100 },
        { title: '物料名称', key: 'NAME', width: 100 },
        { title: '编辑人', key: 'REPORTER_NAME', width: 90 },
        { title: '编辑时间', key: 'REPORTER_TIME', width: 150, sortable: true },
        { title: '审核人', key: 'VERIFY_NAME', width: 90 },
        { title: '审核时间', key: 'VERIFY_TIME', width: 150, sortable: true },
    ];

    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "WyglService";
    search.method = "GetWlGoods";
}

search.browseHref = function (row, index) {
    _.OpenPage({
        id: 10900203,
        title: '浏览物料信息',
        url: "WLGL/WlGoods/WlGoodsMx/" + row.GOODSDM
    });
};
search.addHref = function (row) {

    _.OpenPage({
        id: 10900201,
        title: '新增物料信息',
        url: "WLGL/WlGoods/WlGoodsEdit/"
    });
};
search.modHref = function (row, index) {
    _.OpenPage({
        id: 10900201,
        title: '编辑物料供货商信息',
        url: "WLGL/WlGoods/WlGoodsEdit/" + row.GOODSDM
    });
};




