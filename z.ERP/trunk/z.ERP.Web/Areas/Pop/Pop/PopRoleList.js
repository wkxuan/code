search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "角色编码", key: 'ROLEID', width: 100 },
        { title: '角色代码', key: 'ROLECODE', width: 100 },
        { title: '角色名称', key: 'ROLENAME', width: 200 },
        { title: '所属机构', key: 'ORGNAME', width: 200 }
    ];
    search.service = "UserService";
    search.method = "GetRole";
}
search.initSearchParam = function () {
    search.searchParam.ROLECODE = "";
    search.searchParam.ROLENAME = "";
}
