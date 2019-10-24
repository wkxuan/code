search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: '状态', key: 'STATUSMC', width: 80 },
        { title: "供货商代码", key: 'MERCHANTID', width: 105, sortable: true },
        { title: '供货商名称', key: 'GHSNAME', width: 200 },
        { title: '物料代码', key: 'GOODSDM', width: 100 },
        { title: '物料名称', key: 'NAME', width: 100 },
        { title: '编辑人', key: 'REPORTER_NAME', width: 90 },
        { title: '编辑时间', key: 'REPORTER_TIME', width: 150, sortable: true },
        { title: '审核人', key: 'VERIFY_NAME', width: 90 },
        { title: '审核时间', key: 'VERIFY_TIME', width: 150, sortable: true },
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
                _.OpenPage({
                    id: 10900201,
                    title: '物料供货商信息',
                    url: "WLGL/WlGoods/WlGoodsEdit/" + row.GOODSDM
                });
            }
        }
    ];
    search.service = "WyglService";
    search.method = "GetWlGoods";
}
search.newCondition = function () {
    search.searchParam.MERCHANTID = "";
    search.searchParam.NAME = "";
    search.searchParam.GOODSDM = "";
};
search.addHref = function (row) {
    _.OpenPage({
        id: 10900201,
        title: '添加物料信息',
        url: "WLGL/WlGoods/WlGoodsEdit/"
    });
};




