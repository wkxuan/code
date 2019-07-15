
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
    editDetail.screenParam.localMenu = [];
    editDetail.screenParam.BRANCH = editDetail.screenParam.BRANCH || [];
    editDetail.screenParam.Alert = editDetail.screenParam.Alert || [];

    editDetail.screenParam.colDef_Menufee = [
        { type: 'selection', width: 60, align: 'center' },
        { title: '费用项目名称', key: 'NAME' }
    ];
    editDetail.screenParam.colDef_Menuregion = [
        { type: 'selection', width: 60, align: 'center' },
        { title: '区域名称', key: 'NAME' }
    ];
    editDetail.screenParam.colDef_BRANCH = [
        { type: 'selection', width: 60, align: 'center' },
        { title: '门店名称', key: 'NAME' }
    ];
    editDetail.screenParam.colDef_Alert = [
        { type: 'selection', width: 60, align: 'center' },
        { title: '预警项目', key: 'NAME' }
    ];
    //门店
    editDetail.screenParam.selectDataAlert = function (selection, row) {
        editDetail.checkAlert(selection);
    };

    editDetail.screenParam.selectDataAllAlert = function (selection) {
        editDetail.checkAlert(selection);
    };
    editDetail.screenParam.selectCancelAlert = function (selection) {
        editDetail.checkAlert(selection);
    };
    editDetail.screenParam.selectAllCancelAlert = function (selection) {
        editDetail.checkAlert(selection);
    }
    //门店
    editDetail.screenParam.selectDataBRANCH = function (selection, row) {
        editDetail.checkBRANCH(selection);
    };

    editDetail.screenParam.selectDataAllBRANCH = function (selection) {
        editDetail.checkBRANCH(selection);
    };
    editDetail.screenParam.selectCancelBRANCH = function (selection) {
        editDetail.checkBRANCH(selection);
    };
    editDetail.screenParam.selectAllCancelBRANCH = function (selection) {
        editDetail.checkBRANCH(selection);
    }
    //费用
    editDetail.screenParam.selectDatafee = function (selection, row) {
        editDetail.checkfee(selection);
    };
    editDetail.screenParam.selectDataAllfee = function (selection) {
        editDetail.checkfee(selection);
    };
    editDetail.screenParam.selectCancelfee = function (selection) {
        editDetail.checkfee(selection);
    };
    editDetail.screenParam.selectAllCancelfee = function (selection) {
        editDetail.checkfee(selection);
    }
    //区域
    editDetail.screenParam.selectDataregion = function (selection, row) {
        editDetail.checkregion(selection);
    };

    editDetail.screenParam.selectDataAllregion = function (selection) {
        editDetail.checkregion(selection);
    };
    editDetail.screenParam.selectCancelregion = function (selection) {
        editDetail.checkregion(selection);
    };
    editDetail.screenParam.selectAllCancelregion = function (selection) {
        editDetail.checkregion(selection);
    }
};

editDetail.newRecord = function () {
    editDetail.dataParam.VOID_FLAG = "2";
};

editDetail.clearKey = function () {
    editDetail.dataParam.ROLECODE = null;
    editDetail.dataParam.ROLENAME = null;
    editDetail.dataParam.ORGIDCASCADER = [];
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
        Vue.set(editDetail.screenParam, "BRANCH", datainit.branch);
        Vue.set(editDetail.screenParam, "Alert", datainit.alert);

        _.Ajax('SearchRole', {
            Data: { ROLEID: data }
        }, function (data) {
            if (data.role != null) {
                $.extend(editDetail.dataParam, data.role);
                editDetail.dataParam.VOID_FLAG = data.role.VOID_FLAG + "";  //控件接收string类型
                editDetail.dataParam.BILLID = data.role.ROLEID;
                if (editDetail.dataParam.ORGIDCASCADER != null) {
                    editDetail.dataParam.ORGIDCASCADER = editDetail.dataParam.ORGIDCASCADER.split(",")
                } else {
                    editDetail.dataParam.ORGIDCASCADER = null;
                }
                editDetail.screenParam.USERMODULE = data.module;

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
                //门店
                var localBRANCH = [];
                for (var j = 0; j < editDetail.screenParam.BRANCH.length; j++) {
                    Vue.set(editDetail.screenParam.BRANCH[j], '_checked', false);

                    for (var i = 0; i < data.branch.length; i++) {
                        if (data.branch[i].BRANCHID == editDetail.screenParam.BRANCH[j].BRANCHID) {
                            Vue.set(editDetail.screenParam.BRANCH[j], '_checked', true);
                            localBRANCH.push({
                                BRANCHID: data.branch[i].BRANCHID
                            });
                        }
                    }
                    Vue.set(editDetail.dataParam, 'ROLE_BRANCH', localBRANCH);
                };
                //预警
                var localALERT = [];
                for (var j = 0; j < editDetail.screenParam.Alert.length; j++) {
                    Vue.set(editDetail.screenParam.Alert[j], '_checked', false);

                    for (var i = 0; i < data.alert.length; i++) {
                        if (data.alert[i].ALERTID == editDetail.screenParam.Alert[j].ALERTID) {
                            Vue.set(editDetail.screenParam.Alert[j], '_checked', true);
                            localALERT.push({
                                ALERTID: data.alert[i].ALERTID
                            });
                        }
                    }
                    Vue.set(editDetail.dataParam, 'ROLE_ALERT', localALERT);
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
    //业态数据
    editDetail.screenParam.localYt = [];
    for (var j = 0; j < editDetail.screenParam.ytTreeData.length; j++) {
        var itemdata = editDetail.screenParam.ytTreeData[j].children;
        InsertTree(itemdata);
    };
    //菜单权限数据
    editDetail.screenParam.localMenu = [];
    for (var j = 0; j < editDetail.screenParam.USERMODULE.length; j++) { 
        for (var i = 0; i < editDetail.screenParam.USERMODULE[j].children.length; i++) { //循环菜单
            var itemdata = editDetail.screenParam.USERMODULE[j].children[i].children;
            InsertTreeMenu(itemdata);
        };
    };

    Vue.set(editDetail.dataParam, 'ROLE_MENU', editDetail.screenParam.localMenu);
    return true;
}
//菜单权限结合
function InsertTreeMenu(treeData) {
    if (treeData.length > 0) {
        for (var i = 0; i < treeData.length; i++) {
            if (treeData[i].checked) {
                editDetail.screenParam.localMenu.push({
                    MENUID: treeData[i].value,
                    MODULECODE: treeData[i].code,
                });
                
                InsertTreeMenu(treeData[i].children)
            }
            else if (treeData[i].children.length > 0) {
                InsertTreeMenu(treeData[i].children)
            }
        }
    }
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
    },
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
//门店选中方法
editDetail.checkBRANCH = function (selection) {
    editDetail.dataParam.ROLE_BRANCH = [];
    var localData = [];
    for (var i = 0; i < selection.length; i++) {
        localData.push({ BRANCHID: selection[i].BRANCHID });
    };
    Vue.set(editDetail.dataParam, 'ROLE_BRANCH', localData);
}
//预警选中方法
editDetail.checkAlert = function (selection) {
    editDetail.dataParam.ROLE_ALERT = [];
    var localData = [];
    for (var i = 0; i < selection.length; i++) {
        localData.push({ ALERTID: selection[i].ALERTID });
    };
    Vue.set(editDetail.dataParam, 'ROLE_ALERT', localData);
}
editDetail.mountedInit = function () {

    _.Ajax('SearchInit', {
        Data: {}
    }, function (data) {
        Vue.set(editDetail.screenParam, "ORGData", data.treeOrg.Obj);
        Vue.set(editDetail.screenParam, "USERMODULE", data.module);
        Vue.set(editDetail.screenParam, "fee", data.fee);
        Vue.set(editDetail.screenParam, "ytTreeData", data.ytTree);
        Vue.set(editDetail.screenParam, "region", data.region);
        Vue.set(editDetail.screenParam, "BRANCH", data.branch);
        Vue.set(editDetail.screenParam, "Alert", data.alert);
    });
}

editDetail.billchange = function () {
}
//接收子页面返回值
editDetail.popCallBack = function (data) {
    editDetail.screenParam.showPopCrmRole = false;
};
//按钮初始化
editDetail.mountedInit = function () {
    editDetail.btnConfig = [{
        id: "add",
        authority: "10100701"
    }, {
        id: "edit",
        authority: "10100701",
        enabled: function (disabled, data) {
                            if (!disabled && data.BILLID != null) {
                                return true;
                            } else {
                                return false;
                            }
                        }
    }, {
        id: "del",
        authority: "10100702",
        enabled: function (disabled, data) {
        if (!disabled && data.BILLID != null) {
            return true;
        } else {
            return false;
        }
    }
    }, {
        id: "save",
        authority: "10100701"
    },{
        id: "abandon",
        authority: "10100701"
    }]
};

//取消保存后方法，原数据回复
editDetail.afterAbandon = function () {
    editDetail.showOne(editDetail.dataParam.BILLID);
}


