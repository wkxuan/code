search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "单据编号", key: "BILLID", width: 105, sortable: true },
        { title: "租约号", key: "CONTRACTID", width: 100, sortable: true },
        { title: "商户代码", key: "MERCHANTID", width: 105, sortable: true },
        { title: "商户名称", key: "MERCHANTNAME", width: 200 },
        { title: "状态", key: "STATUSMC", width: 80 },
        { title: "分店编号", key: "BRANCHID", width: 90 },
        { title: "分店名称", key: "BRANCHNAME", width: 150 },
        { title: "登记人", key: "REPORTER_NAME", width: 90 },
        { title: "登记时间", key: "REPORTER_TIME", width: 150, sortable: true },
        { title: "审核人", key: "VERIFY_NAME", width: 90 },
        { title: "审核时间", key: "VERIFY_TIME", width: 150, sortable: true },
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
                _.OpenPage({
                    id: 107001,
                    title: '编辑保证金返还单',
                    url: "JSGL/BILL_RETURN/Bill_ReturnEdit/" + row.BILLID
                });
            }
        }
    ];
     
    search.service = "JsglService";
    search.method = "GetBillReturnList";

    search.screenParam.showPopMerchant = false;
    search.screenParam.srcPopMerchant = __BaseUrl + "/" + "Pop/Pop/PopMerchantList/";
    search.screenParam.showPopSysuser = false;
    search.screenParam.srcPopSysuser = __BaseUrl + "/" + "Pop/Pop/PopSysuserList/";
    search.screenParam.showPopContract = false;
    search.screenParam.srcPopContract = __BaseUrl + "/" + "Pop/Pop/PopContractList/";

    search.searchParam.REPORTER = "";
    search.searchParam.REPORTERNAME = "";
    search.searchParam.VERIFY = "";
    search.searchParam.VERIFYNAME = "";
    search.screenParam.popParam = {};
}

search.addHref = function (row) {
    _.OpenPage({
        id: 107001,
        title: '新增保证金返还单',
        url: "JSGL/BILL_RETURN/Bill_ReturnEdit/"
    });
}

search.otherMethods = {
    SelSysuser: function () {
        search.screenParam.showPopSysuser = true;
        btnFlag = "REPORTER";
    },
    SelSysuser_sh: function () {
        search.screenParam.showPopSysuser = true;
        btnFlag = "VERIFY";
    },
    SelMerchant: function () {
        search.screenParam.showPopMerchant = true;
    },
    SelContract: function () {
        search.screenParam.showPopContract = true;
    }
}

//接收子页面返回值
search.popCallBack = function (data) {

    if (search.screenParam.showPopSysuser) {
        search.screenParam.showPopSysuser = false;
        for (var i = 0; i < data.sj.length; i++) {
            if (btnFlag == "REPORTER") {
                search.searchParam.REPORTER = data.sj[i].USERID;
                search.searchParam.REPORTERNAME = data.sj[i].USERNAME;
            }
            else if (btnFlag == "VERIFY") {
                search.searchParam.VERIFY = data.sj[i].USERID;
                search.searchParam.VERIFYNAME = data.sj[i].USERNAME;
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

    if (search.screenParam.showPopContract) {
        search.screenParam.showPopContract = false;
        for (var i = 0; i < data.sj.length; i++) {
            search.searchParam.CONTRACTID = data.sj[i].CONTRACTID;
        }
    }
};