search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "单据编号", key: "BILLID", width: 105, sortable: true },
        { title: "年月", key: "NIANYUE", width: 100 },
        { title: "状态", key: "STATUSMC", width: 100 },
        { title: "类型", key: "TYPEMC", width: 80 },
        { title: "租约号", key: "CONTRACTID", width: 100, sortable: true },
        { title: "商户编码", key: "MERCHANTID", width: 105, sortable: true },
        { title: "商户名称", key: "MERCHANTNAME", width: 250 },
        { title: "收费单位", key: "FEE_ACCOUNTNAME", width: 200 },
        { title: "门店名称", key: "BRANCHNAME", width: 250 },
        { title: "登记人", key: "REPORTER_NAME" },
        { title: "登记时间", key: "REPORTER_TIME", width: 150, sortable: true },
        { title: "审核人", key: "VERIFY_NAME" },
        { title: "审核时间", key: "VERIFY_TIME", width: 150, sortable: true },
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
                _.OpenPage({
                    id: 10700501,
                    title: '商户缴费通知单详情',
                    url: "JSGL/BILL_NOTICE/Bill_NoticeEdit/" + row.BILLID
                });
            }
        }
    ];
    search.service = "JsglService";
    search.method = "GetBillNoticeList";
}
search.newCondition = function () {
    search.searchParam.BILLID = "";
    search.searchParam.BRANCHID = "";
    search.searchParam.STATUS = "";
    search.searchParam.MERCHANTID = "";
    search.searchParam.MERCHANTNAME = "";
    search.searchParam.CONTRACTID = "";
    search.searchParam.TYPE = "";
    search.searchParam.NIANYUE_START = "";
    search.searchParam.NIANYUE_END = "";
    search.searchParam.REPORTER = "";
    search.searchParam.REPORTERNAME = "";
    search.searchParam.REPORTER_TIME_START = "";
    search.searchParam.REPORTER_TIME_END = "";
    search.searchParam.VERIFY = "";
    search.searchParam.VERIFYNAME = "";
    search.searchParam.VERIFY_TIME_START = "";
    search.searchParam.VERIFY_TIME_END = "";
    search.searchParam.FEE_ACCOUNTID = "";
}
search.addHref = function (row) {
    _.OpenPage({
        id: 10700501,
        title: '添加商户缴费通知单',
        url: "JSGL/BILL_NOTICE/Bill_NoticeEdit/"
    });
}

search.otherMethods = {
    SelReporter: function () {
        search.screenParam.popParam = {};
        search.popConfig.title = "选择登记人";
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopSysuserList/";
        search.popConfig.open = true;
    },
    SelVerify: function () {
        search.screenParam.popParam = {};
        search.popConfig.title = "选择审核人";
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopSysuserList/";
        search.popConfig.open = true;
    },
    SelMerchant: function () {
        search.screenParam.popParam = {};
        search.popConfig.title = "选择商户";
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopMerchantList/";
        search.popConfig.open = true;
    },
    SelContract: function () {
        search.screenParam.popParam = { BRANCHID: search.searchParam.BRANCHID };
        search.popConfig.title = "选择租约";
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopContractList/";
        search.popConfig.open = true;
    },
}

//接收子页面返回值
search.popCallBack = function (data) {
    if (search.popConfig.open) {
        search.popConfig.open = false;
        for (var i = 0; i < data.sj.length; i++) {
            switch (search.popConfig.title) {
                case "选择登记人":
                    search.searchParam.REPORTER = data.sj[i].USERID;
                    search.searchParam.REPORTERNAME = data.sj[i].USERNAME;
                    break;
                case "选择审核人":
                    search.searchParam.VERIFY = data.sj[i].USERID;
                    search.searchParam.VERIFYNAME = data.sj[i].USERNAME;
                    break
                case "选择商户":
                    search.searchParam.MERCHANTID = data.sj[i].MERCHANTID;
                    search.searchParam.MERCHANTNAME = data.sj[i].NAME;
                    break
                case "选择租约":
                    search.searchParam.CONTRACTID = data.sj[i].CONTRACTID;
                    break
            }
        }
    }
};