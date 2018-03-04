define.beforeVue = function () {

    define.screenParam.colDef = [
        {
            title: "代码",
            key: 'ROLECODE', width: 150
        },
        {
            title: '名称',
            key: 'ROLENAME', width: 250
        }];

    define.screenParam.dataDef = [];
    define.service = "XtglService";
    define.method = "GetRoleElement";
    define.methodList = "GetRole";
    define.Key = "ROLEID";
}

define.newRecord = function () {
    define.dataParam.VOID_FLAG = "2";
}
