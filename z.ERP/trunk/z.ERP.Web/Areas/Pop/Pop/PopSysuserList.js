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
//获取父页面参数
search.popInitParam = function (data) {
    if (data) {
        search.searchParam.USER_TYPE = data.USER_TYPE;
    }
}