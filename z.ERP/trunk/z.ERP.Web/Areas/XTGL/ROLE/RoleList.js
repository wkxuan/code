search.beforeVue = function () {
    var col = [
        { title: "角色编码", key: 'ROLEID', width: 100 },
        { title: '角色代码', key: 'ROLECODE', width: 100 },
        { title: '角色名称', key: 'ROLENAME', width: 200 }, 
        { title: '所属机构', key: 'ORGNAME', width: 200 }
    ];
    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "UserService";
    search.method = "GetRole";
}
search.mountedInit = function () {
    _.Ajax('SearchInit', {
        Data: {}
    }, function (data) {
        Vue.set(search.searchParam, "ORGData", data.treeOrg.Obj);
    });
}
search.otherMethods = {
    orgChange: function (value, selectedData) {
        Vue.set(search.searchParam, "ORGCODE", selectedData[selectedData.length - 1].code);
    }
}

search.addHref = function (row) {
    _.OpenPage({
        id: 02000701,
        title: '添加新角色',
        url: "XTGL/ROLE/RoleEdit/-1"
    });
}



search.modHref = function (row, index) {
    _.OpenPage({
        id: 02000702,
        title: '修改角色',
        url: "XTGL/ROLE/RoleEdit/" + row.ROLEID
    });
}
