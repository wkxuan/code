search.beforeVue = function () {
    search.searchParam.BILLID = "";
    search.searchParam.CHANGE_TYPE = 3;
    //search.searchParam.TYPE = true;
    search.screenParam.colDef = [
        { title: "单据编号", key: 'BILLID', width: 105, sortable: true },
        { title: '门店编号', key: 'BRANCHID', width: 85 },
        { title: '门店名称', key: 'BRANCHNAME', width: 150 },
        //{ title: '变更类型', key: 'CHANGE_TYPE', width: 200 },
        { title: '状态', key: 'STATUSMC', width: 80 },
        { title: '编辑人', key: 'REPORTER_NAME', width: 100 },
        { title: '编辑时间', key: 'REPORTER_TIME', width: 150, sortable: true },
        { title: '审核人', key: 'VERIFY_NAME', width: 100 },
        { title: '审核时间', key: 'VERIFY_TIME', width: 150, sortable: true },
        { title: '备注', key: 'DESCRIPTION', width: 200 },
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
                _.OpenPage({
                    id: 104002,
                    title: '资产拆分单',
                    url: "DPGL/ASSETSPILT/AssetSpiltEdit/" + row.BILLID
                });
            }
        }
    ];

    search.service = "DpglService";
    search.method = "GetAssetChangeList";
}

search.addHref = function (row) {
    _.OpenPage({
        id: 104002,
        title: '新增资产拆分单',
        url: "DPGL/ASSETSPILT/AssetSpiltEdit/"
    });
}
