search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "单据编号", key: "BILLID",  sortable: true },
        { title: "商户代码", key: "MERCHANTID",  sortable: true },
        { title: "商户名称", key: "MERCHANTNAME", width: 200 },
        { title: "权债发生月", key: "NIANYUE", sortable: true },
        { title: "付款方式", key: "FKFSNAME" },
        { title: "状态", key: "STATUSMC"},
        { title: "门店名称", key: "BRANCHNAME", width: 250 },
        { title: "登记人", key: "REPORTER_NAME"},
        { title: "登记时间", key: "REPORTER_TIME"},
        { title: "审核人", key: "VERIFY_NAME"},
        { title: "审核时间", key: "VERIFY_TIME"},
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
                _.OpenPage({
                    id: 10700401,
                    title: '预收款收取单',
                    url: "JSGL/BILL_OBTAIN_YSK/Bill_Obtain_YskEdit/" + row.BILLID
                });
            }
        }
    ];
    search.service = "JsglService";
    search.method = "GetBillObtainList";
    //预收款收款
    search.searchParam.TYPE = 1;
    search.screenParam.showPopMerchant = false;
    search.screenParam.srcPopMerchant = __BaseUrl + "/" + "Pop/Pop/PopMerchantList/";
    search.screenParam.showPopSysuser = false;
    search.screenParam.srcPopSysuser = __BaseUrl + "/" + "Pop/Pop/PopSysuserList/";
}

search.addHref = function (row) {
    _.OpenPage({
        id: 10700401,
        title: '新增预收款收取单',
        url: "JSGL/BILL_OBTAIN_YSK/Bill_Obtain_YskEdit/"
    });
}
search.otherMethods = {
    SelMerchant: function () {
        search.screenParam.showPopMerchant = true;
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
//接收子页面返回值
search.popCallBack = function (data) {
    if (search.screenParam.showPopMerchant) {
        search.screenParam.showPopMerchant = false;
        for (var i = 0; i < data.sj.length; i++) {
            search.searchParam.MERCHANTID = data.sj[i].MERCHANTID;
            search.searchParam.MERCHANTNAME = data.sj[i].NAME;
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
