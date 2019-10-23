define.beforeVue = function () {

    define.screenParam.colDef = [
        { title: '终端号', key: 'POSNO' },
        { title: '门店名称', key: 'BRANCHNAME' }
    ];
    define.service = "XtglService";
    define.method = "GetPOSUMSCONFIG";
    define.methodList = "GetPOSUMSCONFIG";
    define.Key = 'POSNO';
}

define.otherMethods = {
    SelStation: function () {
        define.screenParam.popParam = { SqlCondition: " not exists(select 1 from posumsconfig  where STATION.stationbh=posumsconfig.posno)" };
        define.popConfig.title = "选择终端号";
        define.popConfig.src = __BaseUrl + "/Pop/Pop/PopStationList/";
        define.popConfig.open = true;
    },
    branchChange: function () {
        define.showlist();
    }
}
define.initDataParam = function () {
    define.dataParam.POSNO = "";
    define.dataParam.BRANCHNAME = "";
    define.dataParam.IP= "";
    define.dataParam.IP_BAK = "";
    define.dataParam.PORT = "";
    define.dataParam.CFX_MCHTID = "";
    define.dataParam.CFX_TERMID = "";
    define.dataParam.CFXMPAY_MCHTNAME = "";
    define.dataParam.CFXMPAY_MCHTID = "";
    define.dataParam.CFXMPAY_TERMID = "";
}
define.popCallBack = function (data) {
    define.popConfig.open = false;
    if (define.popConfig.title == "选择终端号") {
        for (var i = 0; i < data.sj.length; i++) {
            define.dataParam.POSNO = data.sj[i].POSNO;
            define.dataParam.BRANCHNAME = data.sj[i].BRANCHNAME;
        };
    }
};


define.IsValidSave = function () {
    if (!define.dataParam.POSNO) {
        iview.Message.info("终端号不能为空!");
        return false;
    }
    if (!define.dataParam.IP) {
        iview.Message.info("IP地址不能为空!");
        return false;
    }

    if (!define.dataParam.IP_BAK) {
        iview.Message.info("备用IP地址不能为空!");
        return false;
    }
    if (!define.dataParam.PORT) {
        iview.Message.info("端口号不能未空!");
        return false;
    }

    if (!define.dataParam.CFX_MCHTID) {
        iview.Message.info("银联实体卡商户号不能为空!");
        return false;
    }

    if (!define.dataParam.CFX_TERMID) {
        iview.Message.info("银联实体卡终端号不能为空!");
        return false;
    }
    if (!define.dataParam.CFXMPAY_MCHTNAME) {
        iview.Message.info("银行扫码商户名称不能为空!");
        return false;
    }

    if (!define.dataParam.CFXMPAY_MCHTID) {
        iview.Message.info("银行扫码商户号不能为空!");
        return false;
    }
    if (!define.dataParam.CFXMPAY_TERMID) {
        iview.Message.info("银行扫码终端号不能为空!");
        return false;
    }
    return true;
}