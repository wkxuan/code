search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "单据编号", key: "BILLID", width: 105, sortable: true },
        { title: "债权发生月", key: "NIANYUE", width: 115, sortable: true },
        { title: "收付实现月", key: "YEARMONTH", width: 115, sortable: true },
        { title: "账单类型", key: "TYPEMC", width: 100, sortable: true },
        { title: "开始日期", key: "START_DATE", width: 150 },
        { title: "结束日期", key: "END_DATE", width: 150 },
        { title: "状态", key: "STATUSMC", width: 80 },
        { title: "门店名称", key: "BRANCHNAME", width: 100 },
        { title: "登记人", key: "REPORTER_NAME", width: 90 },
        { title: "登记时间", key: "REPORTER_TIME", width: 150, sortable: true },
        { title: "审核人", key: "VERIFY_NAME", width: 90 },
        { title: "审核时间", key: "VERIFY_TIME", width: 150, sortable: true },
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
                _.OpenPage({
                    id: 10700201,
                    title: '费用调整单',
                    url: "JSGL/BILL_ADJUST/Bill_AdjustEdit/" + row.BILLID
                });
            }
        }
    ];
    search.service = "JsglService";
    search.method = "GetBillAdjustList";
    search.screenParam.showPopMerchant = false;
    search.screenParam.srcPopMerchant = __BaseUrl + "/" + "Pop/Pop/PopMerchantList/";
    search.screenParam.showPopSysuser = false;
    search.screenParam.srcPopSysuser = __BaseUrl + "/" + "Pop/Pop/PopSysuserList/";
}
search.addHref = function (row) {
    _.OpenPage({
        id: 10700201,
        title: '费用调整单',
        url: "JSGL/BILL_ADJUST/Bill_AdjustEdit/"
    });
}
search.otherMethods = {
    SelMerchant: function () {
        if (!search.searchParam.BRANCHID) {
            iview.Message.info("请选择门店!");
            return;
        };
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

