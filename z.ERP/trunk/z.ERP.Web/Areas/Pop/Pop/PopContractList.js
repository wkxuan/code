search.beforeVue = function () {
    var col = [
        { title: "租约号", key: "CONTRACTID", width: 100 },
        { title: "商户编号", key: "MERCHANTID", width: 100 },
        { title: "商户名称", key: "MERCHANTNAME", width: 200 },
        { title: "分店名称", key: "BRANCHNAME", width: 200 },
        { title: "有效期_起", key: "CONT_START", width: 100 },
        { title: "有效期_止", key: "CONT_END", width: 100 },
        { title: "状态", key: "STATUSMC", width: 100 },
        { title: "登记人", key: "REPORTER_NAME", width: 100 },
        { title: "登记时间", key: "REPORTER_TIME", width: 100 }
    ];
    search.screenParam.colDef = col.concat(search.colMul);
    search.service = "DataService";
    search.method = "GetContract";

    //search.screenParam.TERMID = 0;
    //search.screenParam.srcPopFeeSubject = __BaseUrl + "/" + "Pop/Pop/PopFeeSubjectList/";
    //search.screenParam.showFeeSubject = false;


}
//获取父页面参数
search.popInitParam = function (data) {
    search.searchParam.BRANCHID = data.BRANCHID;
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

