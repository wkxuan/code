search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "单据编号", key: "BILLID",  sortable: true },
        { title: "债权发生月", key: "NIANYUE", sortable: true },
        { title: "收付实现月", key: "YEARMONTH" ,sortable: true },
        { title: "账单类型", key: "TYPEMC", sortable: true },
        { title: "开始日期", key: "START_DATE", width: 150 },
        { title: "结束日期", key: "END_DATE", width: 150 },
        { title: "状态", key: "STATUSMC", width: 80 },
        { title: "门店名称", key: "BRANCHNAME", width: 250 },
        { title: "登记人", key: "REPORTER_NAME"},
        { title: "登记时间", key: "REPORTER_TIME", sortable: true },
        { title: "审核人", key: "VERIFY_NAME" },
        { title: "审核时间", key: "VERIFY_TIME", sortable: true },
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
                _.OpenPage({
                    id: 10700201,
                    title: '费用调整单详情',
                    url: "JSGL/BILL_ADJUST/Bill_AdjustEdit/" + row.BILLID
                });
            }
        }
    ];
    search.service = "JsglService";
    search.method = "GetBillAdjustList";
}
search.newCondition = function () {
    search.searchParam.TYPE = 2;  
    search.searchParam.BILLID = "";
    search.searchParam.BRANCHID = "";
    search.searchParam.STATUS = "";
    search.searchParam.NIANYUE_START = "";
    search.searchParam.NIANYUE_END = "";
    search.searchParam.YEARMONTH_START = "";
    search.searchParam.YEARMONTH_END = "";
    search.searchParam.REPORTER = "";
    search.searchParam.REPORTER_NAME = "";
    search.searchParam.REPORTER_TIME_START = "";
    search.searchParam.REPORTER_TIME_END = "";
    search.searchParam.VERIFY = "";
    search.searchParam.VERIFY_NAME = "";
    search.searchParam.VERIFY_TIME_START = "";
    search.searchParam.VERIFY_TIME_END = "";
}
search.addHref = function (row) {
    _.OpenPage({
        id: 10700201,
        title: '添加费用调整单',
        url: "JSGL/BILL_ADJUST/Bill_AdjustEdit/"
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
    }
}
search.popCallBack = function (data) {
    if (search.popConfig.open) {
        search.popConfig.open = false;
        for (var i = 0; i < data.sj.length; i++) {
            switch (search.popConfig.title) {
                case "选择登记人":
                    search.searchParam.REPORTER = data.sj[i].USERID;
                    search.searchParam.REPORTER_NAME = data.sj[i].USERNAME;
                    break;
                case "选择审核人":
                    search.searchParam.VERIFY = data.sj[i].USERID;
                    search.searchParam.VERIFY_NAME = data.sj[i].USERNAME;
                    break
            }
        }
    }
};

