search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "账单号", key: "BILLID", width: 80 },
        { title: "租约号", key: "CONTRACTID", width: 100 },
        { title: "费用项目", key: "TERMMC", width: 100 },
        { title: "债权发生月", key: "NIANYUE", width: 100 },
        { title: "收付实现月", key: "YEARMONTH", width: 100 },
        { title: '应收金额', key: 'MUST_MONEY', width: 100 },
        { title: '未付金额', key: 'UNPAID_MONEY', width: 100 },
        { title: '已付金额', key: 'RECEIVE_MONEY', width: 100 },
        { title: "开始日期", key: "START_DATE", width: 100 },
        { title: "结束日期", key: "END_DATE", width: 100 },
        { title: "状态", key: "STATUSMC", width: 80 },
        { title: "门店编号", key: "BRANCHID", width: 90 },
        { title: "门店名称", key: "BRANCHNAME", width: 150 },
        { title: "登记人", key: "REPORTER_NAME", width: 90 },
        { title: "登记时间", key: "REPORTER_TIME", width: 150 }
    ];
    search.service = "DataService";
    search.method = "GetBillPart";
}
search.initSearchParam = function () {
    search.searchParam.BILLID = "";
    search.searchParam.BRANCHID = "";
    search.searchParam.STATUS = [];
    search.searchParam.CONTRACTID = "";
    search.searchParam.MERCHANTID = "";
    search.searchParam.TYPE = [];
    search.searchParam.TRIMID = [];
    search.searchParam.NIANYUE = "";
    search.searchParam.YEARMONTH = "";
    search.searchParam.REPORTER = "";
    search.searchParam.REPORTER_TIME_START = "";
    search.searchParam.REPORTER_TIME_END = "";
}

