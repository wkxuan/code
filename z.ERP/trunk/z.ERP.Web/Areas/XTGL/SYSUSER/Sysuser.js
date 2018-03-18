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
    define.dataParam.colDef = [
        {
            title: "角色代码",
            key: 'ROLECODE', width: 150
        },
        {
            title: '角色名称',
            key: 'ROLENAME', width: 250
        },
        {
            title: '所属机构',
            key: 'ORGNAME', width: 250
        }];
    define.screenParam.dataDef = [];
    define.dataParam.USER_ROLE = [];
    define.service = "XtglService";
    define.method = "GetUserElement";
    define.methodList = "GetUser";
    define.Key = "USERID";
}

define.newRecord = function () {
    define.dataParam.USER_FLAG = "1";
    define.dataParam.VOID_FLAG = "2";
}
