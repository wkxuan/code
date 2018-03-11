search.beforeVue = function () {
    search.searchParam.GOODSID = "";
    var col = [
        { title: '商品代码', key: 'GOODSDM', width: 80 },
        { title: "商品条码", key: 'BARCODE', width: 150 },
        { title: '商品名称', key: 'NAME', width: 100 },
        { title: '拼音码', key: 'PYM', width: 100 },
        { title: '租约号', key: 'CONTRACTID', width: 100 },
        { title: '商户代码', key: 'MERCHANTID', width: 250 },
        { title: '商户名称', key: 'SHMC', width: 200 },
        { title: '商品分类', key: 'KINDID', width: 80 },
        { title: '商品类型', key: 'TYPE', width: 100 },
        { title: '状态', key: 'STATUSMC', width: 80 },
        { title: '编辑人', key: 'REPORTER_NAME', width: 80 },
        { title: '编辑时间', key: 'REPORTER_TIME', width: 150 },
        { title: '审核人', key: 'VERIFY_NAME', width: 80 },
        { title: '审核时间', key: 'VERIFY_TIME', width: 150 },
    ];
    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "SpglService";
    search.method = "GetGoods";
}

//浏览双击跳转页面
search.browseHref = function (row, index) {
    _.OpenPage("SPGL/GOODS/GoodsDetail/" + row.GOODSID, function (data) {
    });
}
//添加跳转页面
search.addHref = function (row) {
    _.OpenPage("SPGL/GOODS/GoodsEdit/", function (data) {
    });
}
search.modHref = function (row, index) {
    _.OpenPage("SPGL/GOODS/GoodsEdit/" + row.GOODSID, function (data) {
    });
}