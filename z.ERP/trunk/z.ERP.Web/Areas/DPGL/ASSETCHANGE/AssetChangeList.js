search.beforeVue = function () {
    search.searchParam.BILLID = "";
    CHANGE_TYPE = "";
    search.searchParam.TYPE = true;
    var col = [
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
    ];
    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "DpglService";
    search.method = "GetAssetChangeList";
}
//search.searchParam.TYPE = false;
//searchParam.CHANGE_TYPE = ViewBag.Type;
//浏览双击跳转页面

search.browseHref = function (row, index) {
    _.OpenPage({
        id: 104001,
        title: '浏览资产面积变更单',
        url: "DPGL/ASSETCHANGE/AssetChangeDetail/" + row.BILLID
    });
}

search.addHref = function (row) {
    _.OpenPage({
        id: 104001,
        title: '新增资产面积变更单',
        url: "DPGL/ASSETCHANGE/AssetChangeEdit/"
    });
}
search.modHref = function (row, index) {
    _.OpenPage({
        id: 104001,
        title: '编辑资产面积变更单',
        url: "DPGL/ASSETCHANGE/AssetChangeEdit/" + row.BILLID
    });
}