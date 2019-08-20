﻿search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: '商品代码', key: 'GOODSDM', width: 105, sortable: true },
        { title: "商品条码", key: 'BARCODE', width: 130, sortable: true },
        { title: '商品名称', key: 'NAME', width: 150 },
        { title: '拼音码', key: 'PYM', width: 100 },
        { title: '租约号', key: 'CONTRACTID', width: 100, sortable: true },
        { title: '商户代码', key: 'MERCHANTID', width: 105, sortable: true },
        { title: '商户名称', key: 'MERCHANTNAME', width: 200 },
        { title: '商品分类', key: 'KINDCODE', width: 105, sortable: true },
        { title: '分类名称', key: 'KINDNAME', width: 120 },
        { title: '商品类型', key: 'TYPEMC', width: 85 },
        { title: '状态', key: 'STATUSMC', width: 80 },
        { title: '编辑人', key: 'REPORTER_NAME', width: 90 },
        { title: '编辑时间', key: 'REPORTER_TIME', width: 150, sortable: true },
        { title: '审核人', key: 'VERIFY_NAME', width: 90 },
        { title: '审核时间', key: 'VERIFY_TIME', width: 150, sortable: true },
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
                _.OpenPage({
                    id: 105002,
                    title: '商品信息',
                    url: "SPGL/GOODS/GoodsEdit/" + row.GOODSID
                });
            }
        }
    ];
    search.service = "SpglService";
    search.method = "GetGoods";
}

search.addHref = function (row) {
    _.OpenPage({
        id: 105002,
        title: '商品信息',
        url: "SPGL/GOODS/GoodsEdit/"
    });
}