search.beforeVue = function () {
    search.searchParam.MERCHANTID = "";
    var col = [
        { title: '状态', key: 'STATUSMC', width: 80 },
        { title: "商户代码", key: 'MERCHANTID', width: 105, sortable: true },
        { title: '商户名称', key: 'NAME', width: 200 },
        { title: '税号', key: 'SH', width: 100 },
        { title: '银行账号', key: 'BANK', width: 100 },
        { title: '银行', key: 'BANK_NAME', width: 250 },
        { title: '地址', key: 'ADRESS', width: 200 },
        { title: '联系人', key: 'CONTACTPERSON', width: 80 },
        { title: '联系人电话', key: 'PHONE', width: 100 },
        { title: '编辑人', key: 'REPORTER_NAME', width: 90 },
        { title: '编辑时间', key: 'REPORTER_TIME', width: 150, sortable: true },
        { title: '审核人', key: 'VERIFY_NAME', width: 90 },
        { title: '审核时间', key: 'VERIFY_TIME', width: 150, sortable: true },
    ];

    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "WyglService";
    search.method = "GetWlMerchant";
}

search.browseHref = function (row, index) {
    _.OpenPage({
        id: 10900103,
        title: '浏览物料供货商信息',
        url: "WLGL/WlGoods/WlGoodsMx/" + row.MERCHANTID
    });
};
search.addHref = function (row) {

    _.OpenPage({
        id: 10900101,
        title: '新增物料供货商信息',
        url: "WLGL/WlGoods/WlGoodsEdit/"
    });
};
search.modHref = function (row, index) {
    _.OpenPage({
        id: 10900101,
        title: '编辑物料供货商信息',
        url: "WLGL/WlGoods/WlGoodsEdit/" + row.MERCHANTID
    });
};




