search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: '人员代码', key: 'USERCODE' },
        { title: '人员名称', key: 'USERNAME'},
        { title: '人员类型', key: 'USER_TYPEMC' },
        { title: '所属机构', key: 'ORGNAME' }
    ];
    search.service = "UserService";
    search.method = "GetUser";
}
search.initSearchParam = function () {
    search.searchParam.USERCODE = "";
    search.searchParam.USERNAME = "";
}