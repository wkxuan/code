editDetail.beforeVue = function () {
    editDetail.others = false;
    editDetail.branchid = false;
    editDetail.service = "UserService";
    editDetail.method = "GetRoleElement";
    editDetail.screenParam.USERMODULE = editDetail.screenParam.USERMODULE || [];
    editDetail.screenParam.fee = editDetail.screenParam.fee || [];
    editDetail.screenParam.showPopCrmRole = false;
    editDetail.screenParam.srcPopCrmRole = "http://47.93.116.29:8002/PopupPage/defczgqx.aspx?personid=-5";
    editDetail.screenParam.popParam = {};

    editDetail.screenParam.colDef_Menu = [
        { type: 'selection', width: 60, align: 'center' },
        { title: '菜单名称', key: 'MENUNAME', width: 150 },
        { title: '按钮名称', key: 'BUTONNAME', width: 190 }
    ];

    editDetail.screenParam.colDef_Menufee = [
        { type: 'selection', width: 60, align: 'center' },
        { title: '费用项目名称', key: 'NAME', width: 150 }
    ];

    editDetail.screenParam.selectData = function (selection, row) {
        editDetail.checkSysUserGroupMenu(selection);
    };

    editDetail.screenParam.selectDataAll = function (selection) {
        editDetail.checkSysUserGroupMenu(selection);
    };
    editDetail.screenParam.selectCancel = function (selection) {
        editDetail.checkSysUserGroupMenu(selection)
    };


    editDetail.screenParam.selectDatafee = function (selection, row) {
        editDetail.checkfee(selection);
    };

    editDetail.screenParam.selectDataAllfee = function (selection) {
        editDetail.checkfee(selection);
    };
    editDetail.screenParam.selectCancelfee = function (selection) {
        editDetail.checkfee(selection);
    };

};
editDetail.checkSysUserGroupMenu = function (selection) {
    editDetail.dataParam.ROLE_MENU = [];
    var localData = [];
    for (var i = 0; i < selection.length; i++) {
        localData.push({
            MENUID: selection[i].MENUID,
            MODULECODE: selection[i].MODULECODE
        });
    };
    Vue.set(editDetail.dataParam, 'ROLE_MENU', localData);
}
editDetail.newRecord = function () {
    editDetail.dataParam.VOID_FLAG = "2";
};

editDetail.clearKey = function () {
    editDetail.dataParam.ROLECODE = null;
    editDetail.dataParam.ROLENAME = null;
    editDetail.dataParam.ORGIDCASCADER = null;
    editDetail.dataParam.VOID_FLAG = "2";
    editDetail.showOne(-1);
};

editDetail.showOne = function (data, callback) {


    _.Ajax('SearchInit', {
        Data: {}
    }, function (datainit) {
        Vue.set(editDetail.screenParam, "ORGData", datainit.treeOrg.Obj);
        Vue.set(editDetail.screenParam, "USERMODULE", datainit.module);
        Vue.set(editDetail.screenParam, "fee", datainit.fee);


        _.Ajax('SearchRole', {
            Data: { ROLEID: data }
        }, function (data) {
            if (data.role != null) {
                $.extend(editDetail.dataParam, data.role);
                editDetail.dataParam.BILLID = data.role.ROLEID;
                editDetail.dataParam.ORGIDCASCADER = editDetail.dataParam.ORGIDCASCADER.split(",");
                var localMenu = [];
                for (var j = 0; j < editDetail.screenParam.USERMODULE.length; j++) {
                    Vue.set(editDetail.screenParam.USERMODULE[j], '_checked', false);

                    for (var i = 0; i < data.module.length; i++) {
                        if ((data.module[i].MENUID == editDetail.screenParam.USERMODULE[j].MENUID) && (
                           data.module[i].MODULECODE == editDetail.screenParam.USERMODULE[j].MODULECODE)) {
                            Vue.set(editDetail.screenParam.USERMODULE[j], '_checked', true);
                            localMenu.push({
                                MENUID: data.module[i].MENUID,
                                MODULECODE: data.module[i].MODULECODE
                            });
                        }
                    }
                    Vue.set(editDetail.dataParam, 'ROLE_MENU', localMenu);
                };


                var localFee = [];
                for (var j = 0; j < editDetail.screenParam.fee.length; j++) {
                    Vue.set(editDetail.screenParam.fee[j], '_checked', false);

                    for (var i = 0; i < data.fee.length; i++) {
                        if (data.fee[i].TRIMID == editDetail.screenParam.fee[j].TRIMID) {
                            Vue.set(editDetail.screenParam.fee[j], '_checked', true);
                            localFee.push({
                                TRIMID: data.fee[i].TRIMID
                            });
                        }
                    }
                    Vue.set(editDetail.dataParam, 'ROLE_FEE', localFee);
                };
            };
        });
    });
    callback && callback();
}

editDetail.IsValidSave = function () {
    return true;
}

editDetail.otherMethods = {
    orgChange: function (value, selectedData) {
         editDetail.dataParam.ORGID = value[value.length - 1];
    },
    SelCrmRole: function () {
        if(editDetail.dataParam.ROLECODE=="")
        {
            iview.Message.info("请输入角色代码!");
            return;
        }
        editDetail.screenParam.srcPopCrmRole = "http://47.93.116.29:8002/PopupPage/defczgqx.aspx?personid=" + editDetail.dataParam.ROLECODE;
        editDetail.screenParam.showPopCrmRole = true;
    }
}

editDetail.checkfee = function (selection) {
    editDetail.dataParam.ROLE_FEE = [];
    var localData = [];
    for (var i = 0; i < selection.length; i++) {
        localData.push({ TRIMID: selection[i].TRIMID });
    };
    Vue.set(editDetail.dataParam, 'ROLE_FEE', localData);
}

editDetail.mountedInit = function () {

    _.Ajax('SearchInit', {
        Data: {}
    }, function (data) {
        Vue.set(editDetail.screenParam, "ORGData", data.treeOrg.Obj);
    });
}

editDetail.billchange = function () {
}
//接收子页面返回值
editDetail.popCallBack = function (data) {
    editDetail.screenParam.showPopCrmRole = false;
};




