define.beforeVue = function () {
    define.screenParam.colDef = [
        { title: "代码", key: "ID", width: 200 },
        { title: "名称", key: "NAME", width: 250 }
    ];
    define.screenParam.dataDef = [];

    define.service = "XtglService";
    define.method = "GetOperationrule";
    define.methodList = "GetOperationrule";
    define.Key = "ID";
}
