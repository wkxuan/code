﻿search.beforeVue = function () {
    search.searchParam.BILLID = "";
    CHANGE_TYPE = "";
    search.searchParam.TYPE = true;
    search.screenParam.colDef = [
        { title: "单据编号", key: 'BILLID', width: 105, sortable: true },
        { title: '门店编号', key: 'BRANCHID', width: 85 },
        { title: '门店名称', key: 'BRANCHNAME' },
        //{ title: '变更类型', key: 'CHANGE_TYPE', width: 200 },
        { title: '状态', key: 'STATUSMC' },
        { title: '编辑人', key: 'REPORTER_NAME'},
        { title: '编辑时间', key: 'REPORTER_TIME',  sortable: true },
        { title: '审核人', key: 'VERIFY_NAME' },
        { title: '审核时间', key: 'VERIFY_TIME' ,sortable: true },
        { title: '备注', key: 'DESCRIPTION'},
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
                _.OpenPage({
                    id: 104001,
                    title: '资产面积变更单详情',
                    url: "DPGL/ASSETCHANGE/AssetChangeEdit/" + row.BILLID
                });
            }
        }
    ];
    search.service = "DpglService";
    search.method = "GetAssetChangeList";
}
//search.searchParam.TYPE = false;
//searchParam.CHANGE_TYPE = ViewBag.Type;
//浏览双击跳转页面

search.addHref = function (row) {
    _.OpenPage({
        id: 104001,
        title: '资产面积变更单详情',
        url: "DPGL/ASSETCHANGE/AssetChangeEdit/"
    });
}