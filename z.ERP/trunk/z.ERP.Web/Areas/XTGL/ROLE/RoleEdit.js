
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

editDetail.clearKey = function () {
    editDetail.dataParam.ROLECODE = null;
    editDetail.dataParam.ROLENAME = null;
    editDetail.dataParam.ORGIDCASCADER = [];
    editDetail.dataParam.VOID_FLAG = "2";
};

editDetail.newRecord = function () {
    this.otherMethods.initdata();
};

editDetail.showOne = function (data, callback) {
    this.otherMethods.initdata(function () {
        _.Ajax('SearchInit', {
            Data: { ROLEID: data }
        }, function (data) {
            if (data.role != null && data.role.ROLEID != null) {
                $.extend(editDetail.dataParam, data.role);
                editDetail.dataParam.VOID_FLAG = data.role.VOID_FLAG + "";  //控件接收string类型
                editDetail.dataParam.BILLID = data.role.ROLEID;

                if (editDetail.dataParam.ORGIDCASCADER != null) {
                    editDetail.dataParam.ORGIDCASCADER = editDetail.dataParam.ORGIDCASCADER.split(",")
                } else {
                    editDetail.dataParam.ORGIDCASCADER = null;
                }
            };
            editDetail.screenParam.userModule = data.module;
            editDetail.screenParam.regionTreeData = data.regionTree;
            editDetail.screenParam.ytTreeData = data.ytTree;
            editDetail.screenParam.fee = data.fee;
            editDetail.screenParam.branch = data.branch;
            editDetail.screenParam.alert = data.alert;
        });
    });
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
    initdata: function (func) {
        _.Ajax('SearchInit', {
            Data: { }
        }, function (data) {
            editDetail.screenParam.userModule = data.module;
            editDetail.screenParam.regionTreeData = data.regionTree;
            editDetail.screenParam.ytTreeData = data.ytTree;
            editDetail.screenParam.fee = data.fee;
            editDetail.screenParam.branch = data.branch;
            editDetail.screenParam.alert = data.alert;
            if (typeof func == "function") {
                func();
            }
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

    this.otherMethods.initdata();

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
    if (editDetail.dataParam.BILLID) {
        editDetail.showOne(editDetail.dataParam.BILLID);
    } else {
        this.otherMethods.initdata();
    }
}