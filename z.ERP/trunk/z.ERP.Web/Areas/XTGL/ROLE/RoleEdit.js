editDetail.beforeVue = function () {
    editDetail.others = false;
    editDetail.branchid = false;
    editDetail.service = "UserService";
    editDetail.method = "GetRoleElement";
    editDetail.screenParam.USERMODULE = editDetail.screenParam.USERMODULE || [];
    editDetail.screenParam.fee = editDetail.screenParam.fee || [];
    editDetail.screenParam.showPopCrmRole = false;
    editDetail.screenParam.srcPopCrmRole = "http://113.133.162.90:8002/PopupPage/defczgqx.aspx?personid=-5";
    editDetail.screenParam.popParam = {};
    editDetail.screenParam.ytTreeData = editDetail.screenParam.ytTreeData || [];
    editDetail.screenParam.region = editDetail.screenParam.region || [];
    editDetail.screenParam.localYt = [];
    editDetail.screenParam.colDef_Menu = [
        { type: 'selection', width: 60, align: 'center' },
        { title: '菜单名称', key: 'MENUNAME', width: 150 },
        { title: '按钮名称', key: 'BUTONNAME', width: 190 }
    ];

    editDetail.screenParam.colDef_Menufee = [
        { type: 'selection', width: 60, align: 'center' },
        { title: '费用项目名称', key: 'NAME', width: 150 }
    ];
    editDetail.screenParam.colDef_Menuregion = [
        { type: 'selection', width: 60, align: 'center' },
        { title: '区域名称', key: 'NAME', width: 150 }
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

    editDetail.screenParam.selectDataregion = function (selection, row) {
        editDetail.checkregion(selection);
    };

    editDetail.screenParam.selectDataAllregion = function (selection) {
        editDetail.checkregion(selection);
    };
    editDetail.screenParam.selectCancelregion = function (selection) {
        editDetail.checkregion(selection);
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
        Vue.set(editDetail.screenParam, "ytTreeData", datainit.ytTree);
        Vue.set(editDetail.screenParam, "region", datainit.region);


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

                var localRegion = [];
                for (var j = 0; j < editDetail.screenParam.region.length; j++) {
                    Vue.set(editDetail.screenParam.region[j], '_checked', false);

                    for (var i = 0; i < data.region.length; i++) {
                        if (data.region[i].REGIONID == editDetail.screenParam.region[j].REGIONID) {
                            Vue.set(editDetail.screenParam.region[j], '_checked', true);
                            localRegion.push({
                                REGIONID: data.region[i].REGIONID
                            });
                        }
                    }
                    Vue.set(editDetail.dataParam, 'ROLE_REGION', localRegion);
                };
               
                editDetail.screenParam.ytTreeData = data.ytTreeData;                
            };
        });
    });
    callback && callback();
}

editDetail.IsValidSave = function () {
    editDetail.screenParam.localYt = [];
    for (var j = 0; j < editDetail.screenParam.ytTreeData.length; j++) {
        var itemdata = editDetail.screenParam.ytTreeData[j].children;
        InsertTree(itemdata);
    };
    return true;
}

function InsertTree(treeData) {
    if (treeData.length>0)
    {
        for (var i = 0; i < treeData.length; i++)
        {
            if (treeData[i].checked) {
                editDetail.screenParam.localYt.push({ YTID: treeData[i].value });
                Vue.set(editDetail.dataParam, 'ROLE_YT', editDetail.screenParam.localYt);
            }
            else if (treeData[i].children.length>0)
            {
                InsertTree(treeData[i].children)
            }
        }
    }
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
        editDetail.screenParam.srcPopCrmRole = "http://113.133.162.90:8002/PopupPage/defczgqx.aspx?personid=" + editDetail.dataParam.ROLECODE;
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

editDetail.checkregion = function (selection) {
    editDetail.dataParam.ROLE_REGION = [];
    var localData = [];
    for (var i = 0; i < selection.length; i++) {
        localData.push({ REGIONID: selection[i].REGIONID });
    };
    Vue.set(editDetail.dataParam, 'ROLE_REGION', localData);
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




