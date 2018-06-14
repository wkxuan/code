editDetail.beforeVue = function () {
    editDetail.branchid = false;
    editDetail.service = "UserService";
    editDetail.method = "GetRoleElement";
    editDetail.Key = "ROLEID";
    editDetail.dataParam.USERMODULEBUTTON = [];
    editDetail.isChange = false;

    editDetail.screenParam.colDef_Menu = [
        { type: 'selection', width: 60, align: 'center' },
        { title: '菜单名称', key: 'MODULENAME', width: 350 }
    ];
    editDetail.screenParam.colDef_Menubutton = [
        { type: 'selection', width: 60, align: 'center' },
        { title: '按钮名称', key: 'MODULENAME', width: 350 }
    ];
    editDetail.screenParam.colDef_Menufee = [
        { type: 'selection', width: 60, align: 'center' },
        { title: '费用项目名称', key: 'NAME', width: 350 }
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


    editDetail.screenParam.selectDatabutton = function (selection, row) {
        editDetail.checkSysUserGroupMenuButton(selection)
    };

    editDetail.screenParam.selectDataAllbutton = function (selection) {
        editDetail.checkSysUserGroupMenuButton(selection)
    };
    editDetail.screenParam.selectCancelbutton = function (selection) {
        editDetail.checkSysUserGroupMenuButton(selection)
    };


    editDetail.screenParam.selectDatafee=function (selection, row) {
        editDetail.checkfee(selection);
    };

    editDetail.screenParam.selectDataAllfee= function (selection) {
        editDetail.checkfee(selection);
    };
    editDetail.screenParam.selectCancelfee = function (selection) {
        editDetail.checkfee(selection);
    };


    editDetail.screenParam.usermoduleclick = function (selection) {
        editDetail.isChange = true;
        _.Search({
            Service: "UserService",
            Method: "Get_UserModule_Button",
            Data: { MODULECODE: selection.MODULECODE },
            Success: function (data) {
                Vue.set(editDetail.screenParam, 'BUTTON', data.rows);
            }
        })
    }
}


editDetail.checkSysUserGroupMenu = function (selection) {
    editDetail.dataParam.USERMODULE = [];
    var localData = [];
    for (var i = 0; i < selection.length; i++) {
        localData.push({ MENUID: selection[i].MENUID });
    };
    Vue.set(editDetail.dataParam, 'USERMODULE', localData);
}

editDetail.checkSysUserGroupMenuButton = function (selection) {

    editDetail.screenParam.USERMODULEBUTTON = [];
    var localData = [];
    for (var i = 0; i < selection.length; i++) {
        localData.push({ MENUID: selection[i].MENUID });
    };
    Vue.set(editDetail.screenParam, 'USERMODULEBUTTON', localData);

    //每次找出当前选中与当前未选中的
    //未选中的重最终的数据中移除
    //选中的若没有就插入

    var all = editDetail.screenParam.BUTTON.concat(editDetail.screenParam.USERMODULEBUTTON);

    var noCheckData = [];
    for (i = 0; i < editDetail.screenParam.BUTTON.length; i++) {  //总数据
        if //(
            (editDetail.screenParam.USERMODULEBUTTON[i].length == 0){
            //||
            //(editDetail.screenParam.USERMODULEBUTTON[i].MENUID != editDetail.screenParam.BUTTON[i].MENUID)) { //选中的数据!=总数据里面的数据
            noCheckData.push(editDetail.screenParam.BUTTON[i].MENUID);
        }
    };

    if (editDetail.dataParam.USERMODULEBUTTON.length == 0) {
        editDetail.dataParam.USERMODULEBUTTON.push(editDetail.screenParam.USERMODULEBUTTON);
    }
    else {
        for (i = 0; i < noCheckData.length; i++) {
            for (j = 0; j < editDetail.dataParam.USERMODULEBUTTON.length; j++) {
                if (editDetail.dataParam.USERMODULEBUTTON[j].MENUID == noCheckData[i].MENUID) {
                    editDetail.dataParam.USERMODULEBUTTON.remove(noCheckData[i].MENUID);
                }
            }
        };

        for (i = 0; i < editDetail.screenParam.USERMODULEBUTTON.length; i++) {
            for (j = 0; j < editDetail.dataParam.USERMODULEBUTTON.length; j++) {
                if (editDetail.dataParam.USERMODULEBUTTON[j].MENUID != editDetail.screenParam.USERMODULEBUTTON[i].MENUID) {
                    editDetail.dataParam.USERMODULEBUTTON.push(editDetail.screenParam.USERMODULEBUTTON[i].MENUID);
                }
            }
        };
    }

    console.log(editDetail.screenParam.USERMODULEBUTTON);
    console.log(editDetail.dataParam.USERMODULEBUTTON);
}

editDetail.newRecord = function () {
    editDetail.dataParam.VOID_FLAG = "1";
}

editDetail.clearKey = function () {
    editDetail.dataParam.ROLECODE = null;
    editDetail.dataParam.ROLENAME = null;
    editDetail.dataParam.ORGIDCASCADER = null;
    editDetail.dataParam.VOID_FLAG = "1";
    editDetail.showOne(-1);
}
editDetail.showOne = function (data, callback) {
        _.Ajax('SearchRole', {
            Data: {ROLEID:data}
        }, function (data) {
            if (data.role != null) {
                $.extend(editDetail.dataParam, data.role);
                editDetail.dataParam.BILLID = data.role.ROLEID;
            };
            Vue.set(editDetail.screenParam, "USERMODULE", data.module);
            Vue.set(editDetail.screenParam, "fee", data.fee);
            callback && callback();
        });    
}


editDetail.IsValidSave = function () {

    return true;
}

editDetail.otherMethods = {
    orgChange: function (value, selectedData) {
         editDetail.dataParam.ORGID = value[value.length - 1];
    }
}



editDetail.checkfee = function (selection) {
    editDetail.dataParam.fee = [];
    var localData = [];
    for (var i = 0; i < selection.length; i++) {
        localData.push({ TRIMID: selection[i].TRIMID });
    };
    Vue.set(editDetail.dataParam, 'FEE', localData);
}

editDetail.mountedInit = function () {
    _.Ajax('SearchInit', {
        Data: {}
    }, function (data) {
        Vue.set(editDetail.screenParam, "ORGData", data.treeOrg.Obj);
    });
}





