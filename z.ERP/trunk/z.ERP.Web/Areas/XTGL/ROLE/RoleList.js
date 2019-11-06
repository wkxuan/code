search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "角色编码", key: 'ROLEID', width: 100 },
        { title: '角色代码', key: 'ROLECODE', width: 100 },
        { title: '角色名称', key: 'ROLENAME', width: 200 }, 
        { title: '所属机构', key: 'ORGNAME', width: 200 },
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
                _.OpenPage({
                    id: 020007,
                    title: '角色信息',
                    url: "XTGL/ROLE/RoleEdit/" + row.ROLEID
                });
            }
        }
    ];
    
    search.service = "UserService";
    search.method = "GetRole";
}

search.mountedInit = function () { }

search.newCondition = function () {
    search.searchParam.ROLECODE = "";
    search.searchParam.ROLENAME = "";
    search.searchParam.ORGCODE = "";
    search.searchParam.ORGData = [];
}

search.otherMethods = {
    orgChange: function (value, selectedData) {
        Vue.set(search.searchParam, "ORGCODE", selectedData[selectedData.length - 1].code);
    }
}

search.addHref = function (row) {
    _.OpenPage({
        id: 02000701,
        title: '角色信息',
        url: "XTGL/ROLE/RoleEdit/"
    });
}
