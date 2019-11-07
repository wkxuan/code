﻿define.beforeVue = function () {

    define.screenParam.colDef = [
        {
            title: "代码",
            key: 'USERCODE'
        },
        {
            title: '名称',
            key: 'USERNAME'
        },
        {
            title: '所属机构',
            key: 'ORGNAME'
        }
    ];
    define.screenParam.colUserRoleDef = [
        {
            title: "角色代码",
            key: 'ROLECODE', width: 100
        },
        {
            title: '角色名称',
            key: 'ROLENAME', width: 200
        },
        {
            title: '所属机构',
            key: 'ORGNAME'
        }];

    define.service = "UserService";
    define.method = "GetUserElement";
    define.methodList = "GetUser";
    define.Key = "USERID";
}

define.initDataParam = function () {
    define.dataParam.USERCODE = "";
    define.dataParam.USERNAME = "";
    define.dataParam.SHOPCODE = "";
    define.dataParam.SHOPID = "";
    define.dataParam.USER_TYPE = "";
    define.dataParam.PASSWORD = "";
    define.dataParam.USER_FLAG = null;
    define.dataParam.VOID_FLAG = null;
    define.dataParam.USER_ROLE = [];
    define.dataParam.ORGIDCASCADER = [];
}

define.newRecord = function () {
    define.dataParam.USER_FLAG = 1;
    define.dataParam.VOID_FLAG = 2;
}

define.showOne = function (data, callback) {
    _.Ajax('SearchUser', {
        Data: { USERID: data }
    }, function (data) {
        $.extend(define.dataParam, data.user);
        define.dataParam.ORGIDCASCADER = define.dataParam.ORGIDCASCADER.toString().split(",");
        define.dataParam.USER_ROLE = data.userrole;
        callback && callback();
    });
}

define.otherMethods = {
    orgChange: function (value, selectedData) {
        define.dataParam.ORGID = value[value.length - 1];
    },
    SelRole: function () {
        define.screenParam.popParam = {};
        define.popConfig.title = "选择角色";
        define.popConfig.src = __BaseUrl + "/Pop/Pop/PopRoleList/";
        define.popConfig.open = true;
    },
    DelRole: function () {
        let selection = this.$refs.roleRef.getSelection();
        if (selection.length == 0) {
            iview.Message.info("请选中要删除的角色!");
        } else {
            for (let i = 0; i < selection.length; i++) {
                let temp = define.dataParam.USER_ROLE;
                for (let j = 0; j < temp.length; j++) {
                    if (temp[j].ROLECODE == selection[i].ROLECODE) {
                        temp.splice(j, 1);
                        break;
                    }
                }
            }
        }
    },
    SelShop: function () {
        define.screenParam.popParam = {};
        define.popConfig.title = "选择店铺";
        define.popConfig.src = __BaseUrl + "/Pop/Pop/PopShopList/";
        define.popConfig.open = true;
    }
}

define.mountedInit = function () {
    define.btnConfig = [{
        id: "search",
        authority: ""
    }, {
        id: "add",
        authority: ""
    }, {
        id: "edit",
        authority: ""
    },{
        id: "save",
        authority: ""
    }, {
        id: "abandon",
        authority: ""
    }];
}

//接收子页面返回值
define.popCallBack = function (data) {
    define.popConfig.open = false;
    if (define.popConfig.title == "选择角色") {
        let role = define.dataParam.USER_ROLE;
        for (let i = 0; i < data.sj.length; i++) {
            if (role.filter(function (item) { return (data.sj[i].ROLECODE == item.ROLECODE) }).length == 0) {
                role.push(data.sj[i]);
            }
        };
    }
    if (define.popConfig.title == "选择店铺") {
        for (var i = 0; i < data.sj.length; i++) {
            define.dataParam.SHOPID = data.sj[i].SHOPID;
            define.dataParam.SHOPCODE = data.sj[i].SHOPCODE;
        };
    }
};

define.IsValidSave = function () {
    if (!define.dataParam.USERCODE) {
        iview.Message.info("用户代码不能为空!");
        return false;
    }
    if (!define.dataParam.USERNAME) {
        iview.Message.info("用户名称不能为空!");
        return false;
    }

    if (!define.dataParam.USER_TYPE) {
        iview.Message.info("用户类型不能为空!");
        return false;
    }

    if (!define.dataParam.ORGID) {
        iview.Message.info("所属机构不能为空!");
        return false;
    }

    if (define.dataParam.USER_TYPE == "2" && !define.dataParam.SHOPID) {
        iview.Message.info("营业员必须选择店铺!");
        return false;
    }

    return true;
};
