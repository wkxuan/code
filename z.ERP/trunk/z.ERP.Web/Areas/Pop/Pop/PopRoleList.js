search.beforeVue = function () {
    var col = [
        { title: "角色编码", key: 'ROLEID', width: 100 },
        { title: '角色代码', key: 'ROLECODE', width: 100 },
        { title: '角色名称', key: 'ROLENAME', width: 200 },
        { title: '所属机构', key: 'ORGNAME', width: 200 }
    ];
    search.screenParam.colDef = col.concat(search.colMul);
    search.service = "UserService";
    search.method = "GetRole";
}

