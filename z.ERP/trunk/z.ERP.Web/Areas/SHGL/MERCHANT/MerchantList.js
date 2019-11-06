search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "商户代码", key: 'MERCHANTID', width: 105, sortable: true },
        { title: '商户名称', key: 'NAME', width: 250 },
        { title: '商户类型', key: 'TYPENAME', width: 100 },
        { title: '营业执照号', key: 'LICENSE', width: 170 },
        { title: '身份证号', key: 'IDCARD', width: 170 },
        { title: '税号', key: 'SH', width: 100 },
        { title: '银行账号', key: 'BANK', width: 100 },
        { title: '银行', key: 'BANK_NAME', width: 250 },
        { title: '地址', key: 'ADRESS', width: 250 },
        { title: '联系人', key: 'CONTACTPERSON', width: 80 },
        { title: '联系人电话', key: 'PHONE', width: 100 },
        { title: '状态', key: 'STATUSMC', width: 80 },
        { title: '编辑人', key: 'REPORTER_NAME', width: 90 },
        { title: '编辑时间', key: 'REPORTER_TIME', width: 150, sortable: true },
        { title: '审核人', key: 'VERIFY_NAME', width: 90 },
        { title: '审核时间', key: 'VERIFY_TIME', width: 150, sortable: true },
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
                _.OpenPage({
                    id: 10200101,
                    title: '商户信息',
                    url: "SHGL/MERCHANT/MerchantEdit/" + row.MERCHANTID
                });
            }
        }
    ];
    search.service = "ShglService";
    search.method = "GetMerchant";
}
search.newCondition = function () {
    search.searchParam.MERCHANTID = "";
    search.searchParam.MERCHANTNAME = "";
    search.searchParam.TYPE = [];
    search.searchParam.BANK = "";
    search.searchParam.SH = "";
    search.searchParam.STATUS = [];
}
search.addHref = function (row) {
    _.OpenPage({
        id: 10200101,
        title: '添加商户信息',
        url: "SHGL/MERCHANT/MerchantEdit/"
    });
};
search.mountedInit = function () {
    search.btnConfig = [{
        id: "search",
        authority: "10200100"
    }, {
        id: "clear",
        authority: "10200100"
    }, {
        id: "add",
        authority: "10200101"
    }, {
        id: "del",
        authority: "10200101"
    }];
};



