search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "收费项目代码", key: 'TRIMID', width: 150 },
        { title: '收费项目名称', key: 'NAME', width: 250 }
    ];
    search.service = "XtglService";
    search.method = "GetFeeSubject";
}
////获取父页面参数
search.popInitParam = function (data) {
    if (data) {
        search.searchParam.SqlCondition = data.SqlCondition;
    }
}
