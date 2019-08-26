
editDetail.beforeVue = function () {
    editDetail.defaultFooter = false;
    editDetail.branchid = false;
    editDetail.service = "UserService";
    editDetail.method = "GetRoleElement";
  
    editDetail.screenParam.showPopCrmRole = false;
    editDetail.screenParam.srcPopCrmRole = null;
    editDetail.screenParam.popParam = {};
    editDetail.screenParam.ORGData = [];

    editDetail.screenParam.userModule = [];
    editDetail.screenParam.ytTreeData = [];
    editDetail.screenParam.regionTreeData = [];
    editDetail.screenParam.fee = [];    
    editDetail.screenParam.branch = [];
    editDetail.screenParam.Alert = [];
    
    editDetail.screenParam.colDef_Menufee = [
        { type: 'selection', width: 60, align: 'center' },
        { title: '费用项目名称', key: 'NAME' }
    ];
    editDetail.screenParam.colDef_BRANCH = [
        { type: 'selection', width: 60, align: 'center' },
        { title: '门店名称', key: 'NAME' }
    ];
    editDetail.screenParam.colDef_Alert = [
        { type: 'selection', width: 60, align: 'center' },
        { title: '预警项目', key: 'NAME' }
    ];
};

editDetail.newRecord = function () {
    editDetail.dataParam.VOID_FLAG = "2";
};

editDetail.clearKey = function () {
    editDetail.dataParam.ROLECODE = null;
    editDetail.dataParam.ROLENAME = null;
    editDetail.dataParam.ORGIDCASCADER = [];
    editDetail.dataParam.VOID_FLAG = "2";
    this.otherMethods.initdata();
};

editDetail.showOne = function (data, callback) {
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

            editDetail.screenParam.userModule = data.module;
            editDetail.screenParam.regionTreeData = data.regionTree;
            editDetail.screenParam.ytTreeData = data.ytTree;

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
            for (var j = 0; j < editDetail.screenParam.branch.length; j++) {
                Vue.set(editDetail.screenParam.branch[j], '_checked', false);

                for (var i = 0; i < data.branch.length; i++) {
                    if (data.branch[i].BRANCHID == editDetail.screenParam.branch[j].BRANCHID) {
                        Vue.set(editDetail.screenParam.branch[j], '_checked', true);
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
        };
    });
    callback && callback();
}

editDetail.IsValidSave = function () {
    if (!editDetail.dataParam.ROLECODE) {
        iview.Message.info("角色代码不能为空!");
        return false;
    }
    if (!editDetail.dataParam.ROLENAME) {
        iview.Message.info("角色名称不能为空!");
        return false;
    }
    if (!editDetail.dataParam.ORGIDCASCADER) {
        iview.Message.info("所属机构不能为空!");
        return false;
    }

    //业态权限数据
    editDetail.dataParam.ROLE_YT = [];
    let ytSaveData = editDetail.veObj.$refs.ytTreeRef.getFilterCheckedNodes();
    for (let i = 0; i < ytSaveData.length; i++) {
        editDetail.dataParam.ROLE_YT.push({
            ROLEID: editDetail.dataParam.ROLEID,
            YTID: ytSaveData[i].value
        });
    };
    
    //菜单权限数据
    editDetail.dataParam.ROLE_MENU = [];
    let moduleSaveData = editDetail.veObj.$refs.moduleTreeRef.getCheckedNodes();
    for (let i = 0; i < moduleSaveData.length; i++) {
        editDetail.dataParam.ROLE_MENU.push({
            MENUID: moduleSaveData[i].value,
            MODULECODE: moduleSaveData[i].code
        });
    };

    //区域权限数据、楼层权限数据
    editDetail.dataParam.ROLE_REGION = [];
    editDetail.dataParam.ROLE_FLOOR = [];
    let regionData = editDetail.screenParam.regionTreeData;
    for (let i = 0; i < regionData.length; i++) {
        if (regionData[i].checked || regionData[i].indeterminate) {
            editDetail.dataParam.ROLE_REGION.push({
                ROLEID: editDetail.dataParam.ROLEID,
                REGIONID: regionData[i].value
            });

            let chl = regionData[i].children;
            if (chl && chl.length) {
                for (let j = 0; j < chl.length; j++) {
                    if (chl[j].checked) {
                        editDetail.dataParam.ROLE_FLOOR.push({
                            ROLEID: editDetail.dataParam.ROLEID,
                            FLOORID: chl[j].value
                        });
                    }
                }
            }
        }  
    }
    
    //门店权限数据
    editDetail.dataParam.ROLE_BRANCH = [];
    let branchSaveData = editDetail.veObj.$refs.branchRef.getSelection();
    for (var i = 0; i < branchSaveData.length; i++) {
        editDetail.dataParam.ROLE_BRANCH.push({ BRANCHID: branchSaveData[i].BRANCHID });
    };

    //费用项权限数据
    editDetail.dataParam.ROLE_FEE = [];
    let feeSaveData = editDetail.veObj.$refs.feeRef.getSelection();
    for (var i = 0; i < feeSaveData.length; i++) {
        editDetail.dataParam.ROLE_FEE.push({ TRIMID: feeSaveData[i].TRIMID });
    };

    //预警权限数据
    editDetail.dataParam.ROLE_ALERT = [];
    let alertSaveData = editDetail.veObj.$refs.alertRef.getSelection();
    for (var i = 0; i < alertSaveData.length; i++) {
        editDetail.dataParam.ROLE_ALERT.push({ ALERTID: alertSaveData[i].ALERTID });
    };

    debugger
    return true;
}

editDetail.otherMethods = {
    orgChange: function (value, selectedData) {
        editDetail.dataParam.ORGID = value[value.length - 1];
    },
    SelCrmRole: function () {
        _.Ajax('getCrmService', {
            Data: {}
        }, function (data) {
            if (!editDetail.dataParam.ROLECODE) {
                editDetail.screenParam.srcPopCrmRole = data;
            } else {
                editDetail.screenParam.srcPopCrmRole = data + "?personid=" + editDetail.dataParam.ROLECODE;
            }
            editDetail.screenParam.showPopCrmRole = true;
        });      
    },
    initdata: function () {
        _.Ajax('SearchInit', {
            Data: {}
        }, function (data) {
            Vue.set(editDetail.screenParam, "userModule", data.module);
            Vue.set(editDetail.screenParam, "regionTreeData", data.regionTree);
            Vue.set(editDetail.screenParam, "ytTreeData", data.ytTree);
            Vue.set(editDetail.screenParam, "fee", data.fee);
            Vue.set(editDetail.screenParam, "branch", data.branch);
            Vue.set(editDetail.screenParam, "Alert", data.alert);
        });
    }
}
//接收子页面返回值
editDetail.popCallBack = function (data) {
    editDetail.screenParam.showPopCrmRole = false;
};
//按钮初始化
editDetail.mountedInit = function () {
    _.Ajax('SearchTreeOrg', {
        Data: {}
    }, function (data) {
        Vue.set(editDetail.screenParam, "ORGData", data.Item1.Obj);
    });
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
            return false;
        }
    }, {
        id: "save",
        authority: "10100701"
    }, {
        id: "abandon",
        authority: "10100701"
    }]
};
//取消保存后方法，原数据回复
editDetail.afterAbandon = function () {
    editDetail.showOne(editDetail.dataParam.BILLID);
}