define.beforeVue = function () {
    define.screenParam.dataDef = [];
    define.service = "XtglService";
    define.method = "TreeOrgData";
    define.methodList = "TreeOrgList";
    define.Key = 'ORGCODE';
}

define.newRecord = function () {
    define.dataParam.VOID_FLAG = 1;
    define.dataParam.LEVEL_LAST = 1;
}

define.IsValidXj = function () {
    if (define.dataParam.LEVEL_LAST == 2) {
        iview.Message.info("当前级次已经是末级不能添加下级!");
        return false;
    };
    return true;
};

