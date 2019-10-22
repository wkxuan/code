define.beforeVue = function () {
    define.screenParam.colDef = [
        { title: "代码", key: "ID", width: 120 },
        { title: "名称", key: "NAME" },
    ];
    define.service = "XtglService";
    define.method = "GetLateFeeRuleElement";
    define.methodList = "GetLateFeeRule";
    define.Key = "ID";
}

define.initDataParam = function () {
    define.dataParam.ID = "";
    define.dataParam.NAME = "";
    define.dataParam.RATIO = "";
    define.dataParam.DAYS = "";
    define.dataParam.AMOUNTS = "";
}

define.IsValidSave = function () {
    if (!define.dataParam.NAME) {
        iview.Message.info("名称不能为空!");
        return false;
    }
    if (!define.dataParam.RATIO) {
        iview.Message.info("滞纳金比例不能为空!");
        return false;
    }
    if (parseFloat(define.dataParam.RATIO) < 0 || parseFloat(define.dataParam.RATIO) / 100 > 1) {
        iview.Message.info("滞纳金比例超出0~1的范围!");
        return false;
    }
    if (!define.dataParam.DAYS) {
        iview.Message.info("宽限天数不能为空!");
        return false;
    }
    if (!define.dataParam.AMOUNTS) {
        iview.Message.info("宽限金额不能为空!");
        return false;
    }
    return true;
};