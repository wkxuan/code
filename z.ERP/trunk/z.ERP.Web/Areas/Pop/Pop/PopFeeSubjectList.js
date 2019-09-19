search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "收费项目代码", key: 'TRIMID', width: 150 },
        { title: '收费项目名称', key: 'NAME', width: 250 }
    ];
    search.service = "XtglService";
    search.method = "GetFeeSubject";
}
search.initSearchParam = function () {
    search.searchParam.TRIMID = "";
    search.searchParam.NAME = "";
    search.searchParam.PYM = "";
    search.searchParam.TYPE = [];
    search.searchParam.ACCOUNT = [];
    search.searchParam.DEDUCTION = [];
}