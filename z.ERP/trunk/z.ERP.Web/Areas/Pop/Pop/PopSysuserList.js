search.beforeVue = function () {
    var col = [
        { title: '人员代码', key: 'USERCODE', width: 100 },
        { title: '人员名称', key: 'USERNAME', width: 200 },
        { title: '人员类型', key: 'USER_TYPEMC', width: 100 },
        { title: '所属机构', key: 'ORGNAME', width: 200 }
    ];
    search.screenParam.colDef = col.concat(search.colMul);
    search.service = "UserService";
    search.method = "GetUser";
}


//获取父页面参数
search.popInitParam = function (data) {
    if (data) {
        search.searchParam.USER_TYPE = data.USER_TYPE;
    }
}