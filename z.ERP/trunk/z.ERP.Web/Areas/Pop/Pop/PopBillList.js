search.beforeVue = function () {
    var col = [
        { title: "单据编号", key: "BILLID", width: 100 },
        { title: "分店编号", key: "BRANCHID", width: 100 },
        { title: "分店名称", key: "BRANCHNAME", width: 200 },
        { title: "债权发生月", key: "NIANYUE", width: 100 },
        { title: "收付实现月", key: "YEARMONTH", width: 100 },
        { title: "开始日期", key: "START_DATE", width: 100 },
        { title: "结束日期", key: "END_DATE", width: 100 },
        { title: "状态", key: "STATUSMC", width: 100 },
        { title: "登记人", key: "REPORTER_NAME", width: 100 },
        { title: "登记时间", key: "REPORTER_TIME", width: 100 }
    ];
    search.screenParam.colDef = col.concat(search.colMul);
    search.service = "DataService";
    search.method = "GetBill";

    search.screenParam.TERMID = 0;

    search.screenParam.srcPopFeeSubject = __BaseUrl + "/" + "Pop/Pop/PopFeeSubjectList/";
    search.screenParam.showFeeSubject = false;


}
////获取父页面参数
search.popInitParam = function (data) {
    if (data)
    {
        search.searchParam.BRANCHID = data.BRANCHID;
        search.searchParam.MERCHANTID = data.MERCHANTID;
    }
}
search.otherMethods = {
    SelFeeSubject: function () {
        search.screenParam.showPopFeeSubject = true;
    }
}
//接收子页面返回值
search.popCallBack = function (data) {
    search.screenParam.showFeeSubject = false;
    for (var i = 0; i < data.sj.length; i++) {
        search.screenParam.TERMID = data.sj[i].TERMID;
        search.screenParam.TERMNAME = data.sj[i].NAME;
    };

};

