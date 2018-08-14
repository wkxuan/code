define.beforeVue = function () {

    define.screenParam.colDef = [
        {
            title: "代码",
            key: 'USERCODE', width: 100
        },
        {
            title: '名称',
            key: 'USERNAME', width: 200
        }];
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
            key: 'ORGNAME', width: 200
        },
      {
          title: '操作',
          key: 'action',
          width: 80,
          align: 'center',
          render: function (h, params) {
              return h('div',
                  [
                  h('Button', {
                      props: { type: 'primary', size: 'small', disabled: false },

                      style: { marginRight: '50px' },
                      on: {
                          click: function (event) {
                              define.dataParam.USER_ROLE.splice(params.index, 1);
                          }
                      },
                  }, '删除')
                  ]);
          }
      }];
    define.screenParam.dataDef = [];
    define.screenParam.componentVisible = false;
    define.dataParam.ORGIDCASCADER = [];
    define.dataParam.USER_ROLE = [];
    define.service = "UserService";
    define.method = "GetUserElement";
    define.methodList = "GetUser";
    define.Key = "USERID";
    define.screenParam.showPopRole = false;
    define.screenParam.srcPopRole = __BaseUrl + "/" + "Pop/Pop/PopRoleList/";
    define.screenParam.showPopShop = false;
    define.screenParam.srcPopShop = __BaseUrl + "/" + "Pop/Pop/PopShopList/";
    define.screenParam.popParam = {};
}

define.newRecord = function () {
    define.dataParam.USER_FLAG = "1";
    define.dataParam.VOID_FLAG = "2";
    define.dataParam.USER_ROLE = [];
    define.dataParam.ORGIDCASCADER = [];
}

define.showone = function (data, callback) {
    _.Ajax('SearchUser', {
        Data: { USERID: data }
    }, function (data) {
        $.extend(define.dataParam, data.user);
        define.dataParam.ORGIDCASCADER = define.dataParam.ORGIDCASCADER.split(",");
        define.dataParam.USER_ROLE = data.userrole;
        callback && callback();
    });
}
define.otherMethods = {
    orgChange: function (value, selectedData) {
        define.dataParam.ORGID = value[value.length - 1];
    },
    SelRole: function () {
        define.screenParam.showPopRole = true;
    },
    SelShop:function () {
        define.screenParam.showPopShop = true;
    }
}

define.mountedInit = function () {
    _.Ajax('SearchInit', {
        Data: {}
    }, function (data) {
        Vue.set(define.screenParam, "ORGData", data.treeOrg.Obj);
    });
}

//接收子页面返回值
define.popCallBack = function (data) {
    if (define.screenParam.showPopRole)
    {
        define.screenParam.showPopRole = false;
        for (var i = 0; i < data.sj.length; i++) {
            define.dataParam.USER_ROLE.push(data.sj[i]);
        };
    }

    else if (define.screenParam.showPopShop)
    {
        define.screenParam.showPopShop = false;
        for (var i = 0; i < data.sj.length; i++) {
            define.dataParam.SHOPID = data.sj[i].SHOPID;
            define.dataParam.SHOPCODE = data.sj[i].SHOPCODE;
        };
    }

};


define.IsValidSave = function () {
    if (!define.dataParam.USERCODE)
    {
        iview.Message.info("用户代码不能为空!");
        return false;
    }

    if (!define.dataParam.USERNAME)
    {
        iview.Message.info("用户名称不能为空!");
        return false;
    }

    if (!define.dataParam.USER_TYPE)
    {
        iview.Message.info("用户类型不能为空!");
        return false;
    }

    if (!define.dataParam.ORGID) {
        iview.Message.info("所属机构不能为空!");
        return false;
    }


    if (define.dataParam.USER_TYPE == "2" && !define.dataParam.SHOPID)
    {
        iview.Message.info("营业员必须选择店铺!");
        return false;
    }

    return true;
};  
