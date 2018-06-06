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
    define.screenParam.colUserRoleDef = [
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
    define.screenParam.componentVisible = false;
    define.dataParam.USER_ROLE = [];
    define.service = "UserService";
    define.method = "GetUserElement";
    define.methodList = "GetUser";
    define.Key = "USERID";

}

define.newRecord = function () {
    define.dataParam.USER_FLAG = "1";
    define.dataParam.VOID_FLAG = "2";
}

define.showone = function (data, callback) {
    _.Ajax('SearchUser', {
        Data: { USERID: data }
    }, function (data) {
        $.extend(define.dataParam, data.user);
        define.dataParam.USER_ROLE = data.userrole;
        callback && callback();
    });
}
define.otherMethods = {
    orgChange: function (value, selectedData) {
        define.dataParam.ORGID = value[value.length - 1];
    }
}

define.mountedInit = function () {
    _.Ajax('SearchInit', {
        Data: {}
    }, function (data) {
        Vue.set(define.screenParam, "ORGData", data.treeOrg.Obj);
    });
}
