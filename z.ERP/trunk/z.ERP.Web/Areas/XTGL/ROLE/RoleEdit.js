editDetail.beforeVue = function () {

    editDetail.dataParam.colDef = [
        {
            title: "代码",
            key: 'ROLECODE', width: 100
        },
        {
            title: '名称',
            key: 'ROLENAME', width: 150
        }];
    editDetail.dataParam.dataDef = [];
    editDetail.branchid = false;
    editDetail.service = "UserService";
    editDetail.method = "GetRoleElement";
    editDetail.methodList = "GetRole";
    editDetail.Key = "ROLEID";
    editDetail.dataParam.ROLE_MENU = [];
    if (!editDetail.dataParam.ROLE_FEE) {
        editDetail.dataParam.ROLE_FEE = [{
            CHECKED: "false",
            TRIMID: "",
            NAME: "",
        }]
    }
    editDetail.dataParam.value = "1";
}

editDetail.newRecord = function () {
    editDetail.dataParam.VOID_FLAG = "2";
}

editDetail.showOne = function (data, callback) {
        _.Ajax('SearchRole', {
            Data: {ROLEID:data}
        }, function (data) {
            $.extend(editDetail.dataParam, data.role);
            editDetail.dataParam.BILLID = data.role.ROLEID;
            editDetail.dataParam.ROLE_MENU = data.menu.Obj;
            editDetail.dataParam.ROLE_FEE = data.fee;
            for (var i = 0; i < editDetail.dataParam.ROLE_FEE.length; i++) {
                if (editDetail.dataParam.ROLE_FEE[i].DISABLED == 0) {
                    editDetail.dataParam.ROLE_FEE[i].DISABLED = true;
                }
                else {
                    editDetail.dataParam.ROLE_FEE[i].DISABLED = false;
                }
                if (editDetail.dataParam.ROLE_FEE[i].CHECKED == 0) {
                    editDetail.dataParam.ROLE_FEE[i].CHECKED = false;
                }
                else {
                    editDetail.dataParam.ROLE_FEE[i].CHECKED = true;
                }
            }
            callback && callback();
        });    
}

editDetail.IsValidSave = function () {
    //editDetail.dataParam.FEESUBJECT = this.$refs.selectData.getSelection();
    var ss = editDetail.dataParam.ROLE_MENU;
    return true;
}
