editDetail.beforeVue = function () {
    editDetail.branchid = false;
    editDetail.service = "UserService";
    editDetail.method = "GetRoleElement";
    editDetail.methodList = "GetRole";
    editDetail.Key = "ROLEID";
    editDetail.screenParam.ROLE_MENU = [];
    if (!editDetail.screenParam.ROLE_FEE) {
        editDetail.screenParam.ROLE_FEE = [{
            CHECKED: "false",
            TRIMID: "",
            NAME: "",
        }]
    }
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
            //菜单
            editDetail.screenParam.ROLE_MENU = data.menu.Obj;
            //收费项目距
            editDetail.screenParam.ROLE_FEE = data.fee;
            for (var i = 0; i < editDetail.screenParam.ROLE_FEE.length; i++) {
                if (editDetail.screenParam.ROLE_FEE[i].DISABLED == 0) {
                    editDetail.screenParam.ROLE_FEE[i].DISABLED = true;
                }
                else {
                    editDetail.screenParam.ROLE_FEE[i].DISABLED = false;
                }
                if (editDetail.screenParam.ROLE_FEE[i].CHECKED == 0) {
                    editDetail.screenParam.ROLE_FEE[i].CHECKED = false;
                }
                else {
                    editDetail.screenParam.ROLE_FEE[i].CHECKED = true;
                }
            }
            callback && callback();
        });    
}

editDetail.IsValidSave = function () {
    editDetail.dataParam.ROLE_MENU = [];
    editDetail.dataParam.ROLE_FEE = [];
    function GetTreeMenuSelectedData(node) {
        if (node.length > 0)
        {
            for (var i = 0; i < node.length; i++)
            {
                if(node[i].checked)
                editDetail.dataParam.ROLE_MENU.push({ "MODULECODE": node[i].code,"MENUID":1 });
                if (node[i].children.length > 0 && node[i].checked)
                {
                    GetTreeMenuSelectedData(node[i].children)
                }
            }
        }
    }

    GetTreeMenuSelectedData(editDetail.screenParam.ROLE_MENU);    
    for (var i = 0; i < editDetail.screenParam.ROLE_FEE.length; i++)
    {
        if (editDetail.screenParam.ROLE_FEE[i].CHECKED)
        {
            editDetail.dataParam.ROLE_FEE.push({ "TRIMID": editDetail.screenParam.ROLE_FEE[i].TRIMID });
        }
    }
    return true;
}



