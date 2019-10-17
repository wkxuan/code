﻿search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "单据编号", key: "BILLID",sortable: true },
        { title: "记账日期", key: "ACCOUNT_DATE", sortable: true, cellType: "date" },
        { title: "收银员", key: "SYYMC", sortable: true },
        { title: "营业员", key: "YYYMC",  sortable: true },
        { title: "状态", key: "STATUSMC"},
        { title: "门店名称", key: "BRANCHMC",width:250 },
        { title: "登记人", key: "REPORTER_NAME" },
        { title: "登记时间", key: "REPORTER_TIME"},
        { title: "审核人", key: "VERIFY_NAME" },
        { title: "审核时间", key: "VERIFY_TIME" },
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
                _.OpenPage({
                    id: 10500401,
                    title: '销售补录单详情',
                    url: "SPGL/SALEBILL/SaleBillEdit/" + row.BILLID
                });
            }
        }
    ];
    search.service = "SpglService";
    search.method = "GetSaleBillList";

    search.screenParam.showPopMerchant = false;
    search.screenParam.srcPopMerchant = __BaseUrl + "/" + "Pop/Pop/PopMerchantList/";
    search.screenParam.showPopBrand = false;
    search.screenParam.srcPopBrand = __BaseUrl + "/" + "Pop/Pop/PopBrandList/";
    search.screenParam.showPopSysuser = false;
    search.screenParam.srcPopSysuser = __BaseUrl + "/" + "Pop/Pop/PopSysuserList/";
    search.screenParam.popParam = {};
    search.screenParam.KINDID = [];
    search.searchParam.TYPE = 3;
}

search.mountedInit = function () {
    search.btnConfig = [{
        id: "search",
        authority: ""
    }, {
        id: "clear",
        authority: ""
    }, {
        id: "add",
        authority: ""
    }, {
        id: "del",
        authority: ""
    }, {
        id: "upload",
        authority: ""
    }];
}

search.otherMethods = {
    SelMerchant: function () {
        search.screenParam.showPopMerchant = true;
    },
    SelBrand: function () {
        search.screenParam.showPopBrand = true;
    },
    SelSysuser: function () {
        search.screenParam.showPopSysuser = true;
        btnFlag = "REPORTER";
    },
    SelSysuser_sh: function () {
        search.screenParam.showPopSysuser = true;
        btnFlag = "VERIFY";
    },
}

search.popCallBack = function (data) {
    if (search.screenParam.showPopMerchant) {
        search.screenParam.showPopMerchant = false;
        for (var i = 0; i < data.sj.length; i++) {
            search.searchParam.MERCHANTID = data.sj[i].MERCHANTID;
            search.searchParam.MERCHANTNAME = data.sj[i].NAME;
        }
    }
    if (search.screenParam.showPopBrand) {
        search.screenParam.showPopBrand = false;
        for (var i = 0; i < data.sj.length; i++) {
            search.searchParam.BRANDID = data.sj[i].BRANDID;
            search.searchParam.BRANDNAME = data.sj[i].NAME;
        }
    }
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
    }
};
search.addHref = function (row) {
    _.OpenPage({
        id: 105004,
        title: '添加销售补录单',
        url: "SPGL/SALEBILL/SaleBillEdit/"
    })
}


