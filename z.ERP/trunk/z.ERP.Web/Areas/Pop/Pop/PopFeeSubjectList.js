search.beforeVue = function () {
    var col = [
        { title: "收费项目代码", key: 'TRIMID', width: 150 },
        { title: '收费项目名称', key: 'NAME', width: 250 }
    ];
    search.screenParam.colDef = col.concat(search.colMul);
    search.service = "XtglService";
    search.method = "GetFeeSubject";
}

