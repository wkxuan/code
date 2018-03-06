define.beforeVue = function () {
    define.screenParam.dataDef = [];
    define.service = "XtglService";
    define.method = "TreeOrgData";
    define.methodList = "TreeOrgList";
    define.Key = 'ORGCODE';
}

define.newRecord = function () {
    define.dataParam.VOID_FLAG = "1";
    define.dataParam.LEVEL_LAST = "1";
}