popShow.beforeVue = function () {
    popShow.screenParam.SHOPCODE = "";
    popShow.screenParam.STATUSMC = "";
    popShow.screenParam.RENTAREA = "";
    popShow.screenParam.CATEGORYNAME = "";
    popShow.screenParam.BRANDNAME = "";
    popShow.screenParam.MERCHANTNAME = "";
    popShow.screenParam.CONTRACTID = "";
    popShow.screenParam.CONT_S_E = "";
    popShow.screenParam.OPERATERULE = "";
    popShow.screenParam.FEERULE = "";
    popShow.screenParam.RENT = "";
    popShow.screenParam.RENEFFECT = "";
    popShow.screenParam.AMOUNT = "";
    popShow.screenParam.AMOUNTEFFECT = "";
    popShow.screenParam.DISCRIPTION = "";
}


//获取父页面参数
popShow.popInitParam = function (data) {
    if (data) {
        popShow.screenParam.SHOPCODE = data.SHOPCODE;
        popShow.screenParam.STATUSMC = data.STATUSMC;
        popShow.screenParam.RENTAREA = data.RENTAREA;
        popShow.screenParam.CATEGORYNAME = data.CATEGORYNAME;
        popShow.screenParam.BRANDNAME = data.BRANDNAME;
        popShow.screenParam.MERCHANTNAME = data.MERCHANTNAME;
        popShow.screenParam.CONTRACTID = data.CONTRACTID;
        popShow.screenParam.CONT_S_E = data.CONT_S_E;
        popShow.screenParam.OPERATERULENAME = data.OPERATERULENAME;
        popShow.screenParam.QZRQ = data.QZRQ;
        popShow.screenParam.RENT = data.RENT;
        popShow.screenParam.RENEFFECT = data.RENEFFECT;
        popShow.screenParam.AMOUNT = data.AMOUNT;
        popShow.screenParam.AMOUNTEFFECT = data.AMOUNTEFFECT;
        popShow.screenParam.DISCRIPTION = data.DISCRIPTION;
    }
}
