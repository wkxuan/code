define.beforeVue = function () {
    define.screenParam.colDef = [
        { title: "代码", key: "ID", width: 200 },
        { title: "名称", key: "NAME" }
    ];

    define.service = "XtglService";
    define.method = "GetOperationrule";
    define.methodList = "GetOperationrule";
    define.Key = "ID";
}
define.initDataParam = function () {
    define.dataParam.ID = "";
    define.dataParam.NAME = "";
    define.dataParam.WYSIGN = "";
    define.dataParam.PROCESSTYPE = "";
    define.dataParam.LADDERSIGN = "";
}