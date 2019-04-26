define.beforeVue = function () {

    define.screenParam.colDef = [
    { title: '终端号', key: 'POSNO', width: 400 }
    ];

    define.screenParam.dataDef = [];
    define.service = "XtglService";
    define.method = "GetPOSUMSCONFIG";
    define.methodList = "GetPOSUMSCONFIG";
    define.Key = 'POSNO';

}

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