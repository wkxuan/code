define.beforeVue = function () {

    define.screenParam.colDef = [
        {
            title: "代码",
            key: 'USERCODE', width: 150
        },
        {
            title: '名称',
            key: 'USERNAME', width: 250
        }];

    define.screenParam.dataDef = [];
    define.service = "XtglService";
    define.method = "GetUserElement";
    define.methodList = "GetUser";
    define.Key = "USERID";
}

define.newRecord = function () {
    define.dataParam.USER_FLAG = "1";
    define.dataParam.VOID_FLAG = "2";
}
