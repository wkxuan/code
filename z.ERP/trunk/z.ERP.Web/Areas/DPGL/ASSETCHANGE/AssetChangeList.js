search.beforeVue = function () {
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
    _.OpenPage("DPGL/ASEETCHANGE/Detail/" + row.BILLID, function (data) {
    });
}
//添加跳转页面
search.addHref = function (row) {
    _.OpenPage("DPGL/ASSETCHANGE/AssetChangeEdit/", function (data) {
    });
}
//修改跳转页面,并且要根据单号查出来相关的数据信息
search.modHref = function (row, index) {
    _.OpenPage("DPGL/MASSETCHANGEERCHANT/AssetChangeEdit/"+ row.BILLID, function (data) {
    });
}
search.deleteData = function (row, index,_self) {
    _.Ajax('Delete', {
        DeleteData: {BILLID:row.BILLID}
    }, function (data) {
        _self.remove(params.index)
    });
}



