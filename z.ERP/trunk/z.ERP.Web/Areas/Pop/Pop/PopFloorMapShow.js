popShow.beforeVue = function () {
    popShow.screenParam.SHOPCODE = "";
    popShow.screenParam.USERCODE = "";
    popShow.screenParam.USERNAME = "";
    popShow.service = "UserService";
    popShow.method = "GetUser";
}


//获取父页面参数
popShow.popInitParam = function (data) {
    if (data) {
        popShow.screenParam.SHOPCODE = data.SHOPCODE;
        popShow.screenParam.USERCODE = data.USERCODE;;
        popShow.screenParam.USERNAME = data.USERNAME;;
    }
}
