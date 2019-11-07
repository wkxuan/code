search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "单据编号", key: "BILLID", width: 105, sortable: true },
        { title: "租约号", key: "CONTRACTID", width: 105, sortable: true },
        { title: "商户编码", key: "MERCHANTID", width: 105, sortable: true },
        { title: "商户名称", key: "MERCHANTNAME", width: 200 },
        { title: "年月", key: "NIANYUE", width: 100, sortable: true },
        { title: "状态", key: "STATUSMC", width: 80 },
        { title: "门店编号", key: "BRANCHID", width: 90 },
        { title: "门店名称", key: "BRANCHNAME", width: 150 },
        { title: "登记人", key: "REPORTER_NAME", width: 90 },
        { title: "登记时间", key: "REPORTER_TIME", width: 150, sortable: true },
        { title: "审核人", key: "VERIFY_NAME", width: 90 },
        { title: "审核时间", key: "VERIFY_TIME", width: 150, sortable: true },
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
                _.OpenPage({
                    id: 107006,
                    title: '联营结算单详情',
                    url: "JSGL/JOINBILL/JoinBillEdit/" + row.BILLID
                });
            }
        }
        
    ]
    search.service = "JsglService";
    search.method = "GetJoinBillList";
}
search.newCondition = function () {
    search.searchParam.BILLID = "";
    search.searchParam.CONTRACTID = "";
    search.searchParam.MERCHANTID = "";
    search.searchParam.NIANYUE_START = "";
    search.searchParam.NIANYUE_END = "";
    search.searchParam.STATUS = "";
    search.searchParam.REPORTER = "";
    search.searchParam.REPORTER_TIME_START = "";
    search.searchParam.REPORTER_TIME_END = "";
    search.searchParam.VERIFY = "";
    search.searchParam.VERIFY_TIME_START = "";
    search.searchParam.VERIFY_TIME_END = "";
}
search.addHref = function () {
    _.OpenPage({
        id: 107006,
        title: '添加联营结算单',
        url: "JSGL/JOINBILL/JoinBillEdit/"
    });
}
search.mountedInit = function () {
    search.btnConfig = [{
        id: "search",
        authority: "10700600"
    }, {
        id: "clear",
        authority: "10700600"
    }, {
        id: "add",
        authority: "10700601"
    }, {
        id: "del",
        authority: "10700601"
    }];
};