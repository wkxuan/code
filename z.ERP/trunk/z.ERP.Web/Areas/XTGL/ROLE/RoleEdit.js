var spins;
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

    editDetail.screenParam.colDef_Menufee = [
        { type: 'selection', width: 60, align: 'center' },
        { title: '费用项目名称', key: 'NAME', width: 150 }
    ];
    editDetail.screenParam.colDef_Menuregion = [
        { type: 'selection', width: 60, align: 'center' },
        { title: '区域名称', key: 'NAME', width: 150 }
    ];

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
            editDetail.otherMethods.spin();
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
    for (var i = 0; i < editDetail.screenParam.USERMODULE[0].children.length; i++) { //循环erp级菜单
        var itemdata = editDetail.screenParam.USERMODULE[0].children[i].children;
        InsertTreeMenu(itemdata);
    };
    for (var i = 0; i < editDetail.screenParam.USERMODULE[1].children.length; i++) { //循环crm菜单
        var itemdata = editDetail.screenParam.USERMODULE[1].children[i].children;
        InsertTreeMenu(itemdata);
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
    spin:function(){
        spins = document.getElementById("spin");
        spins.parentNode.removeChild(spins);
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
        Vue.set(editDetail.screenParam, "USERMODULE", data.module);
        Vue.set(editDetail.screenParam, "fee", data.fee);
        Vue.set(editDetail.screenParam, "ytTreeData", data.ytTree);
        Vue.set(editDetail.screenParam, "region", data.region);
        if (editDetail.Id == "" || editDetail.Id == undefined) {
            editDetail.otherMethods.spin();
        }
    });
}

editDetail.billchange = function () {
}
//接收子页面返回值
editDetail.popCallBack = function (data) {
    editDetail.screenParam.showPopCrmRole = false;
};




