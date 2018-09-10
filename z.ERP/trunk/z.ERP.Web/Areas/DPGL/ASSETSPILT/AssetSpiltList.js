search.beforeVue = function () {
    search.searchParam.BILLID = "";
    search.searchParam.CHANGE_TYPE = 3;
    //search.searchParam.TYPE = true;
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
    _.OpenPage("DPGL/ASSETSPILT/AssetSpiltDetail/" + row.BILLID, function (data) {
    });
}
//添加跳转页面
search.addHref = function (row) {
    _.OpenPage("DPGL/ASSETSPILT/AssetSpiltEdit/", function (data) {
    });
}

search.modHref = function (row, index) {
    _.OpenPage("DPGL/ASSETSPILT/AssetSpiltEdit/" + row.BILLID, function (data) {
   });
}
