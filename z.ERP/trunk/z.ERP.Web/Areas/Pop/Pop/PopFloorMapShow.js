popShow.beforeVue = function () {
    //var col = [
    //    { title: '人员代码', key: 'USERCODE', width: 100 },
    //    { title: '人员名称', key: 'USERNAME', width: 200 },
    //    { title: '人员类型', key: 'USER_TYPEMC', width: 100 },
    //    { title: '所属机构', key: 'ORGNAME', width: 200 }
    //];
    //popShow.screenParam.colDef = col.concat(popShow.colMul);
    popShow.screenParam.USERCODE = "";
    popShow.screenParam.USERNAME = "";
    popShow.service = "UserService";
    popShow.method = "GetUser";
}


//获取父页面参数
popShow.popInitParam = function (data) {
    if (data) {
        //popShow.screenParam.USERCODE = data.USERCODE;
    }
}