search.beforeVue = function () {
    search.searchParam.MERCHANTID = "";
    var col = [
        { title: '状态', key: 'STATUSMC', width: 80 },
        { title: "购进冲红单单号", key: 'BILLID', width: 100 },
        { title: "商户代码", key: 'MERCHANTID', width: 105, sortable: true },
        { title: '商户名称', key: 'NAME', width: 200 },
        { title: '编辑人', key: 'REPORTER_NAME', width: 90 },
        { title: '编辑时间', key: 'REPORTER_TIME', width: 150, sortable: true },
        { title: '审核人', key: 'VERIFY_NAME', width: 90 },
        { title: '审核时间', key: 'VERIFY_TIME', width: 150, sortable: true },
    ];

    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "WyglService";
    search.method = "GetWlOutStock";

    search.screenParam.showPopSysuser = false;
    search.screenParam.srcPopSysuser = __BaseUrl + "/" + "Pop/Pop/PopSysuserList/";
    search.screenParam.popParam = {};

};

search.otherMethods = {
    SelReporter: function () {
        search.screenParam.showPopSysuser = true;
        btnFlag = "REPORTER";
        search.screenParam.popParam = {};
    },
    SelVerify: function () {
        search.screenParam.showPopSysuser = true;
        btnFlag = "VERIFY";
        search.screenParam.popParam = {};
    }
}


search.popCallBack = function (data) {
    if (search.screenParam.showPopSysuser) {
        search.screenParam.showPopSysuser = false;
        for (var i = 0; i < data.sj.length; i++) {
            if (btnFlag == "REPORTER") {
                search.searchParam.REPORTER = data.sj[i].USERID;
                search.searchParam.REPORTER_NAME = data.sj[i].USERNAME;
            }
            else if (btnFlag == "VERIFY") {
                search.searchParam.VERIFY = data.sj[i].USERID;
                search.searchParam.VERIFY_NAME = data.sj[i].USERNAME;
            }
        };
    };
};

search.browseHref = function (row, index) {
    _.OpenPage({
        id: 10900403,
        title: '浏览物料购进冲红单信息',
        url: "WLGL/WLOutStock/WLOutStockMx/" + row.BILLID
    });
};
search.addHref = function (row) {

    _.OpenPage({
        id: 10900401,
        title: '新增物料购进冲红单',
        url: "WLGL/WLOutStock/WLOutStockEdit/"
    });
};
search.modHref = function (row, index) {
    _.OpenPage({
        id: 10900402,
        title: '编辑物料购进冲红单',
        url: "WLGL/WLOutStock/WLOutStockEdit/" + row.BILLID
    });
};