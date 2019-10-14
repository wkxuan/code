define.beforeVue = function () {
    define.screenParam.colDef = [
        {
            title: '支付方式代码',
            key: 'PAYID', width: 150,
        },
        {
            title: '支付方式名称',
            key: 'NAME'
        }];

    define.service = "XtglService";
    define.method = "GetPayElement";
    define.methodList = "GetPay";
    define.Key = 'PAYID';

    define.dataParam.PAYID = "";
    define.dataParam.VOID_FLAG = "";
    define.dataParam.NAME = "";
    define.dataParam.TYPE = "";
    define.dataParam.FK = "";
    define.dataParam.JF = "";
    define.dataParam.ZLFS = "";
    define.dataParam.FLAG = "";

}

define.newRecord = function () {
    define.myve.dataParam.VOID_FLAG = 1;
}


define.IsValidSave = function () {
    if (!define.myve.dataParam.NAME) {
        iview.Message.info("名称不能为空!");
        return false;
    }
    if (!define.myve.dataParam.TYPE) {
        iview.Message.info("类型不能为空!");
        return false;
    }

    if (!define.myve.dataParam.FK) {
        iview.Message.info("返款标记不能为空!");
        return false;
    }
    if (!define.myve.dataParam.JF) {
        iview.Message.info("积分标记不能为空!");
        return false;
    }

    if (define.myve.dataParam.ZLFS == null || define.myve.dataParam.ZLFS == undefined) {
        iview.Message.info("找零方式不能为空!");
        return false;
    }

    if (!define.myve.dataParam.FLAG) {
        iview.Message.info("显示序号不能为空!");
        return false;
    }
    if (isNaN(define.myve.dataParam.FLAG)) {
        iview.Message.info("显示序号必须为数字!");
        return false;
    }

    if (define.myve.dataParam.TYPE == "3" && !define.myve.dataParam.COUPONID)
    {
        iview.Message.info("类型是优惠券时必须选择对应优惠券!");
        return false;
    }

    return true;
}

