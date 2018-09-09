srch.beforeVue = function () {
    srch.searchParam.ORGCODE = "";
    var col = [
        { title: '代码', key: 'ORGCODE', width: 100 },
        { title: '名称', key: 'ORGNAME', width: 200 }
    ];
    srch.screenParam.colDef = col;
    srch.service = "XtglService";
    srch.method = "SrcORG";
};




