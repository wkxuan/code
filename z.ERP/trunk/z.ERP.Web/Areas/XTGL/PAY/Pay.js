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

}

define.newRecord = function () {
    define.dataParam.VOID_FLAG = "1";
}


define.IsValidSave = function () {
    if (!define.dataParam.NAME) {
        iview.Message.info("名称不能为空!");
        return false;
    }
    if (!define.dataParam.TYPE) {
        iview.Message.info("类型不能为空!");
        return false;
    }

    if (!define.dataParam.FK) {
        iview.Message.info("返款标记不能为空!");
        return false;
    }
    if (!define.dataParam.JF) {
        iview.Message.info("积分标记不能为空!");
        return false;
    }

    if (define.dataParam.ZLFS==null) {
        iview.Message.info("找零方式不能为空!");
        return false;
    }

    if (!define.dataParam.FLAG) {
        iview.Message.info("显示序号不能为空!");
        return false;
    }
    if (isNaN(define.dataParam.FLAG)) {
        iview.Message.info("显示序号必须为数字!");
        return false;
    }

    if (define.dataParam.TYPE == "3" && !define.dataParam.COUPONID)
    {
        iview.Message.info("类型是优惠券时必须选择对应优惠券!");
        return false;
    }

    return true;
}

