define.beforeVue = function () {
    define.service = "XtglService";
    define.method = "TreeOrgData";
    define.methodList = "TreeOrgList";
    define.Key = 'ORGCODE';
}

define.newRecord = function () {
    define.dataParam.VOID_FLAG = 1;
    define.dataParam.LEVEL_LAST = 1;
}

define.initDataParam = function () {
    define.dataParam.ORGCODE = null;
    define.dataParam.ORGNAME = null;
    define.dataParam.ORG_TYPE = null;
    define.dataParam.LEVEL_LAST = null;
    define.dataParam.VOID_FLAG = null;
}

define.IsValidXj = function () {
    if (define.vueObj.dataParam.LEVEL_LAST == 2) {
        iview.Message.info("该节点已是末级节点不能再添加下级节点！");
        return false;
    }
    return true;
}
define.IsValidDel = function () {
    if (define.vueObj.dataParam.LEVEL_LAST == 1) {
        iview.Message.info("该节点是非末级节点不能删除！");
        return false;
    }
    return true;
}
