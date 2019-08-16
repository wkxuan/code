search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "单据编号", key: "BILLID",  sortable: true },
        { title: "商户编号", key: "MERCHANTID",  sortable: true },
        { title: "商户名称", key: "MERCHANTNAME", width: 200 },
        { title: "收款年月", key: "NIANYUE",  sortable: true },
        { title: "状态", key: "STATUSMC",  },
        { title: "付款方式", key: "FKFSNAME",  },
        { title: "门店名称", key: "BRANCHNAME", width: 200 },
        { title: "登记人", key: "REPORTER_NAME", },
        { title: "登记时间", key: "REPORTER_TIME",sortable: true },
        { title: "审核人", key: "VERIFY_NAME", },
        { title: "审核时间", key: "VERIFY_TIME", sortable: true },
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
                _.OpenPage({
                    id: 107007,
                    title: '租赁核销单详情',
                    url: "JSGL/Bill_Obtain_Sk/Bill_Obtain_SkEdit/" + row.BILLID
                });
            }
        }
    ];
    search.service = "JsglService";
    search.method = "GetBillObtainList";
    //账单收款
    search.searchParam.TYPE = 3;
    search.screenParam.showPopMerchant = false;
    search.screenParam.srcPopMerchant = __BaseUrl + "/" + "Pop/Pop/PopMerchantList/";
    search.screenParam.showPopSysuser = false;
    search.screenParam.srcPopSysuser = __BaseUrl + "/" + "Pop/Pop/PopSysuserList/";
}
search.addHref = function (row) {
    _.OpenPage({
        id: 107007,
        title: '租赁核销单详情',
        url: "JSGL/BILL_OBTAIN_SK/Bill_Obtain_SkEdit/"
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


