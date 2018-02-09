search.beforeVue = function () {
    search.searchParam.BILLID = "";
    var col = [
        { title: "单据编号", key: 'BILLID', width: 100 },
        { title: '门店', key: 'BRANCHID', width: 200 },
        { title: '变更类型', key: 'CHANGE_TYPE', width: 200 },
        { title: '备注', key: 'DESCRIPTION', width: 200 },
        { title: '状态', key: 'STATUS', width: 80 },
        { title: '编辑人', key: 'REPORTER_NAME', width: 200 },
        { title: '编辑时间', key: 'REPORTER_TIME', width: 200 },
        { title: '审核人', key: 'VERIFY_NAME', width: 200 },
        { title: '审核时间', key: 'VERIFY_TIME', width: 200 },
    ];
    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "DpglService";
    search.method = "GetAssetChange";
}
//浏览双击跳转页面
search.browseHref = function (row, index) {
    _.OpenPage("DPGL/ASSETCHANGE/Detail/" + row.BILLID, function (data) {
    });
}
//添加跳转页面
search.addHref = function (row) {
    _.OpenPage("DPGL/ASSETCHANGE/AssetChangeEdit/", function (data) {
    });
}
//修改跳转页面,并且要根据单号查出来相关的数据信息
search.modHref = function (row, index) {
    _.OpenPage("DPGL/ASSETCHANGE/AssetChangeEdit/"+ row.BILLID, function (data) {
    });
}

search.modHref = function (row, index) {

    _.Search({
        Service: search.service,
        Method: search.method,
        Data: { BILLID: row.BILLID },
        Success: function (data) {
            if (1== 1) {
                _.OpenPage("DPGL/ASSETCHANGE/AssetChangeEdit/" + row.BILLID, function (data) {
                });
            } else {
                iview.Message.info('不能编辑!');
            }
        }
    })
}
