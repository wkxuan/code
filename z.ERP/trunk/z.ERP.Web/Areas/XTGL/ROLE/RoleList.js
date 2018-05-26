search.beforeVue = function () {
    search.searchParam.BILLID = "";
    var col = [
        { title: "角色编码", key: 'ROLEID', width: 100 },
        { title: '角色代码', key: 'ROLECODE', width: 100 },
        { title: '角色名称', key: 'ROLENAME', width: 200 }, 
        { title: '所属机构', key: 'ORGNAME', width: 200 }
        //{ title: '编辑人', key: 'REPORTER_NAME', width: 200 },
        //{ title: '编辑时间', key: 'REPORTER_TIME', width: 200 },
        //{ title: '审核人', key: 'VERIFY_NAME', width: 200 },
        //{ title: '审核时间', key: 'VERIFY_TIME', width: 200 },
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
//浏览双击跳转页面
search.browseHref = function (row, index) {
    //_.OpenPage("XTGL/ROLE/RoleDetail/" + row.ROLEID, function (data) {
    //});
}
//添加跳转页面
search.addHref = function (row) {
    _.OpenPage("XTGL/ROLE/RoleEdit/-1", function (data) {
    });
}

search.modHref = function (row, index) {
    _.OpenPage("XTGL/ROLE/RoleEdit/" + row.ROLEID, function (data) {
   });
}
