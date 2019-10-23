search.beforeVue = function () {
    search.searchParam.ORGCODE = "";
    var col = [
        { title: '代码', key: 'ORGCODE', width: 100 },
        { title: '名称', key: 'ORGNAME', width: 200 }
    ];
    search.screenParam.colDef = col;
    search.service = "XtglService";
    search.method = "SrcORG";
    search.indexShow = true;
    search.selectionShow = false;
};
search.newCondition = function () {
    search.searchParam.ORGCODE = "";
    search.searchParam.ORGNAME = "";
};
search.mountedInit = function () {
    search.btnConfig = [{
        id: "search",
        authority: ""
    }, {
        id: "clear",
        authority: ""
    }];
}
