
editDetail.beforeVue = function () {
    editDetail.defaultFooter = false;
    editDetail.branchid = false;
    editDetail.service = "UserService";
    editDetail.method = "GetRoleElement";

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
                    editDetail.dataParam.ORGIDCASCADER = [];
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
    let ytSaveData = editDetail.vueObj.$refs.ytTreeRef.getFilterCheckedNodes();
    if (ytSaveData) {
        for (let i = 0; i < ytSaveData.length; i++) {
            editDetail.dataParam.ROLE_YT.push({
                ROLEID: editDetail.dataParam.ROLEID,
                YTID: ytSaveData[i].value
            });
        };
    };
    
    //菜单权限数据
    editDetail.dataParam.ROLE_MENU = [];
    let moduleSaveData = editDetail.vueObj.$refs.moduleTreeRef.getCheckedNodes();
    if (moduleSaveData) {
        for (let i = 0; i < moduleSaveData.length; i++) {
            editDetail.dataParam.ROLE_MENU.push({
                MENUID: moduleSaveData[i].value,
                MODULECODE: moduleSaveData[i].code
            });
        };
    };

    //区域权限数据、楼层权限数据
    editDetail.dataParam.ROLE_REGION = [];
    editDetail.dataParam.ROLE_FLOOR = [];
    let regionData = editDetail.screenParam.regionTreeData;
    if (regionData) {
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
    };

    
    //门店权限数据
    editDetail.dataParam.ROLE_BRANCH = [];
    let branchSaveData = editDetail.vueObj.$refs.branchRef.getSelection();
    if (branchSaveData)
    {
        for (var i = 0; i < branchSaveData.length; i++) {
            editDetail.dataParam.ROLE_BRANCH.push({ BRANCHID: branchSaveData[i].BRANCHID });
        };
    }


    //费用项权限数据
    editDetail.dataParam.ROLE_FEE = [];
    let feeSaveData = editDetail.vueObj.$refs.feeRef.getSelection();
    if (feeSaveData) {
        for (var i = 0; i < feeSaveData.length; i++) {
            editDetail.dataParam.ROLE_FEE.push({ TRIMID: feeSaveData[i].TRIMID });
        };
    };

    //预警权限数据
    editDetail.dataParam.ROLE_ALERT = [];
    let alertSaveData = editDetail.vueObj.$refs.alertRef.getSelection();
    if (alertSaveData)
    {
        for (var i = 0; i < alertSaveData.length; i++) {
            editDetail.dataParam.ROLE_ALERT.push({ ALERTID: alertSaveData[i].ALERTID });
        };
    }

    return true;
}

editDetail.otherMethods = {
    onCheckChangeModule: function (checkArr, node) {
        this.setCurNodeChlCheck(node);
        this.setCurNodeParentCheck(node);
    },
    getNodeParentNode: function (node) {
        let data = editDetail.screenParam.userModule;
        return this.filterFunc(node, data);
    },
    filterFunc (node, data) {
        if (!node || !data || !data.length) {
            return;
        }
        for (let i = 0; i < data.length; i++) {
            if (node.parentId == data[i].code) {
                return data[i];
            }
            if (data[i].children && data[i].children.length) {
                let pnode = this.filterFunc(node, data[i].children);
                if (pnode) {
                    return pnode;
                }
            }
        }
    },
    //设置node的子节点checked状态
    setCurNodeParentCheck: function (node) {
        let pnode = this.getNodeParentNode(node);
        if (pnode && pnode.children && pnode.children.length) {
            let num = 0;
            for (let i = 0; i < pnode.children.length; i++) {
                if (pnode.children[i].checked) {
                    num++;
                }
            }
            if (num == pnode.children.length) {
                pnode.checked = true;
                pnode.indeterminate = false;
            } else if (num == 0) {
                if (pnode.code.length == 2 || pnode.code.length == 4) {
                    pnode.checked = false;
                    pnode.indeterminate = false;
                }
            } else {
                if (pnode.code.length == 2 || pnode.code.length == 4) {
                    pnode.checked = false;
                    pnode.indeterminate = true;
                } else {
                    pnode.checked = true;
                    pnode.indeterminate = false;
                }
            }
            if (this.getNodeParentNode(pnode)) {
                this.setCurNodeParentCheck(this.getNodeParentNode(pnode));
            }
        }
    },
    setCurNodeChlCheck: function (node) {
        if (node.children && node.children.length) {
            for (let i = 0; i < node.children.length; i++) {
                node.children[i].checked = node.checked;

                if (node.children[i]) {
                    this.setCurNodeChlCheck(node.children[i]);
                }
            }
        }
    },
    orgChange: function (value, selectedData) {
        editDetail.dataParam.ORGID = value[value.length - 1];
    },
    SelCrmRole: function () {
        _.Ajax('getCrmService', {
            Data: {}
        }, function (data) {
            if (!editDetail.dataParam.ROLECODE) {
                editDetail.popConfig.src = data;
            } else {
                editDetail.popConfig.src = data + "?personid=" + editDetail.dataParam.ROLECODE;
            }
            editDetail.popConfig.title = "CRM权限";
            editDetail.popConfig.height = 400;
            editDetail.popConfig.width = 800;
            editDetail.popConfig.open = true;
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
    editDetail.popConfig.open = false;
};
//按钮初始化
editDetail.mountedInit = function () {
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