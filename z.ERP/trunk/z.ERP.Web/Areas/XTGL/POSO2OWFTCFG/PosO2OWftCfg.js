define.beforeVue = function () {

    define.screenParam.colDef = [
    { title: '终端号', key: 'POSNO', width: 400 }
    ];

    define.screenParam.dataDef = [];
    define.service = "XtglService";
    define.method = "GetPosO2OWftCfg";
    define.methodList = "GetPosO2OWftCfg";
    define.Key = 'POSNO';

}

define.IsValidSave = function () {
    if (!define.dataParam.POSNO) {
        iview.Message.info("终端号为空!");
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