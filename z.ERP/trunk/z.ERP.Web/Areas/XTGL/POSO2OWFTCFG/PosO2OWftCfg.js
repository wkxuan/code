define.beforeVue = function () {

    define.screenParam.colDef = [
    { title: '终端号', key: 'POSNO'},
    { title: '门店名称', key: 'BRANCHNAME'}
    ];

    define.screenParam.popParam = {};

    define.screenParam.dataDef = [];
    define.service = "XtglService";
    define.method = "GetPosO2OWftCfg";
    define.methodList = "GetPosO2OWftCfg";
    define.Key = 'POSNO';

    define.screenParam.showPopStation = false;
    define.screenParam.srcPopStation = __BaseUrl + "/" + "Pop/Pop/PopStationList/";

}

define.otherMethods = {
    SelStation: function () {
        define.screenParam.showPopStation = true;
        define.screenParam.popParam = { SqlCondition: " not exists(select 1 from POSO2OWFTCFG  where STATION.stationbh=POSO2OWFTCFG.posno)" };
    },

}

define.popCallBack = function (data) {

    if (define.screenParam.showPopStation) {
        define.screenParam.showPopStation = false;
        for (var i = 0; i < data.sj.length; i++) {
            define.dataParam.POSNO = data.sj[i].POSNO;
            define.dataParam.BRANCHNAME = data.sj[i].BRANCHNAME;
        }
    }
};

define.IsValidSave = function () {
    if (!define.dataParam.POSNO) {
        iview.Message.info("终端号不能为空!");
        return false;
    }
    if (!define.dataParam.URL) {
        iview.Message.info("服务地址不能为空!");
        return false;
    }

    if (!define.dataParam.PID) {
        iview.Message.info("商户号不能为空!");
        return false;
    }
    if (!define.dataParam.ENCRYPTION) {
        iview.Message.info("必须选择加密方式!");
        return false;
    }

    if (!define.dataParam.KEY) {
        iview.Message.info("密钥不能为空!");
        return false;
    }

    if (define.dataParam.ENCRYPTION == '2' && !define.dataParam.KEY_PUB) {
        iview.Message.info("加密方式为RSA时,公钥不能为空!");
        return false;
    }


    return true;
}