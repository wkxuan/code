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
////获取父页面参数
search.popInitParam = function (data) {
    if (data) {
        search.searchParam.BRANCHID = data.BRANCHID;
        search.searchParam.MERCHANTID = data.MERCHANTID;
        search.searchParam.CONTRACTID = data.CONTRACTID;
        search.searchParam.WFDJ = data.WFDJ;
        search.searchParam.FTYPE = data.FTYPE;   //费用项目类型
        search.searchParam.RRETURNFLAG = data.RRETURNFLAG;
        search.searchParam.SCFS_TZD = data.SCFS_TZD;  //出单类型
        search.searchParam.FEE_ACCOUNTID = data.FEE_ACCOUNTID;  //收费单位
    }
}

