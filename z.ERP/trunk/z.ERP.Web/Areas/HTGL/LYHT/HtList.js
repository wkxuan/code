﻿search.beforeVue = function () {
    search.searchParam.STYLE = "2";  //只查询联营合同
    search.service = "HtglService";
    search.method = "GetContract";

    search.screenParam.colDef = [
        { title: '租约号', key: 'CONTRACTID', sortable: true },
        { title: '状态', key: 'STATUSMC' },
        { title: '资产代码', key: 'SHOPDM', sortable: true },
        { title: '品牌名称', key: 'BRANDNAME' },
        { title: '核算方式', key: 'STYLEMC' },
        { title: "商户代码", key: 'MERCHANTID', sortable: true },
        { title: '商户名称', key: 'MERNAME', width: 200 },
        { title: '合同员', key: 'SIGNER_NAME' },
        { title: '登记人', key: 'REPORTER_NAME' },
        { title: '登记时间', key: 'REPORTER_TIME', cellType: "datetime", width: 160, sortable: true },
        { title: '审核人', key: 'VERIFY_NAME', },
        { title: '审核时间', key: 'VERIFY_TIME', cellType: "datetime", width: 160, sortable: true },
        { title: "门店代码", key: 'BRANCHID' },
        { title: '门店名称', key: 'NAME' },
        {
            title: '操作', key: 'operate', authority: "10600100", onClick: function (index, row, data) {
                _.OpenPage({
                    id: 10600101,
                    title: '联营合同详情',
                    url: "HTGL/LYHT/HtEdit/" + row.CONTRACTID
                });
            }
        }
    ];

    search.screenParam.showPopMerchant = false;
    search.screenParam.srcPopMerchant = __BaseUrl + "/Pop/Pop/PopMerchantList/";
    search.screenParam.showPopSysuser = false;
    search.screenParam.srcPopSysuser = __BaseUrl + "/Pop/Pop/PopSysuserList/";
    search.screenParam.popParam = {};
}

search.otherMethods = {
    SelSigner: function () {
        search.screenParam.showPopSysuser = true;
        btnFlag = "SIGNER";
        search.screenParam.popParam = { USER_TYPE: "7" };
    },
    SelReporter: function () {
        search.screenParam.showPopSysuser = true;
        btnFlag = "REPORTER";
        search.screenParam.popParam = {};
    },
    SelVerify: function () {
        search.screenParam.showPopSysuser = true;
        btnFlag = "VERIFY";
        search.screenParam.popParam = {};
    },
    SelMerchant: function () {
        search.screenParam.showPopMerchant = true;
    }
}

//接收子页面返回值
search.popCallBack = function (data) {
    if (search.screenParam.showPopSysuser) {
        search.screenParam.showPopSysuser = false;
        for (var i = 0; i < data.sj.length; i++) {
            if (btnFlag == "SIGNER") {
                search.searchParam.SIGNER_NAME = data.sj[i].USERNAME;
            }
            else if (btnFlag == "REPORTER") {
                search.searchParam.REPORTER_NAME = data.sj[i].USERNAME;
            }
            else if (btnFlag == "VERIFY") {
                search.searchParam.VERIFY_NAME = data.sj[i].USERNAME;
            }
        };
    }

    if (search.screenParam.showPopMerchant) {
        search.screenParam.showPopMerchant = false;
        for (var i = 0; i < data.sj.length; i++) {
            search.searchParam.MERCHANTID = data.sj[i].MERCHANTID;
            search.searchParam.MERCHANTNAME = data.sj[i].NAME;
        }
    }
};

search.addHref = function (row) {
    _.OpenPage({
        id: 10600101,
        title: '联营合同详情',
        url: "HTGL/LYHT/HtEdit/"
    });
}