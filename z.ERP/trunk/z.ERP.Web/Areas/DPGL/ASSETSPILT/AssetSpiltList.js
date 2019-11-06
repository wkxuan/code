search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "单据编号", key: 'BILLID', width: 105, sortable: true },
        { title: '门店编号', key: 'BRANCHID', width: 85 },
        { title: '门店名称', key: 'BRANCHNAME' },
        { title: '状态', key: 'STATUSMC'},
        { title: '编辑人', key: 'REPORTER_NAME' },
        { title: '编辑时间', key: 'REPORTER_TIME',  sortable: true },
        { title: '审核人', key: 'VERIFY_NAME'},
        { title: '审核时间', key: 'VERIFY_TIME', sortable: true },
        { title: '备注', key: 'DESCRIPTION'},
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
                _.OpenPage({
                    id: 104002,
                    title: '资产拆分单详情',
                    url: "DPGL/ASSETSPILT/AssetSpiltEdit/" + row.BILLID
                });
            }
        }
    ];

    search.service = "DpglService";
    search.method = "GetAssetChangeList";
}
search.newCondition = function () {
    search.searchParam.CHANGE_TYPE = 3;
    search.searchParam.BILLID = "";
    search.searchParam.BRANCHID = "";
    search.searchParam.STATUS = "";
    search.searchParam.DESCRIPTION = "";
};
search.addHref = function (row) {
    _.OpenPage({
        id: 104002,
        title: '添加资产拆分单',
        url: "DPGL/ASSETSPILT/AssetSpiltEdit/"
    });
}
search.mountedInit = function () {
    search.btnConfig = [{
        id: "search",
        authority: "10400200"
    }, {
        id: "clear",
        authority: "10400200"
    }, {
        id: "add",
        authority: "10400201"
    }, {
        id: "del",
        authority: "10400201"
    }];
}
