search.beforeVue = function () {
    search.searchParam.BILLID = "";
    var col = [
        { title: "角色编码", key: 'ROLEID', width: 100 },
        { title: '角色代码', key: 'ROLECODE', width: 200 },
        { title: '角色名称', key: 'ROLENAME', width: 200 }, 
        { title: '状态', key: 'STATUSMC', width: 80 },
        { title: '编辑人', key: 'REPORTER_NAME', width: 200 },
        { title: '编辑时间', key: 'REPORTER_TIME', width: 200 },
        { title: '审核人', key: 'VERIFY_NAME', width: 200 },
        { title: '审核时间', key: 'VERIFY_TIME', width: 200 },
    ];
    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "DpglService";
    search.method = "GetAssetChange";
}
//search.searchParam.TYPE = false;
//searchParam.CHANGE_TYPE = ViewBag.Type;
//浏览双击跳转页面
search.browseHref = function (row, index) {
    _.OpenPage("DPGL/ASSETSPILT/Detail/" + row.BILLID, function (data) {
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
