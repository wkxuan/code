define.beforeVue = function () {
    define.service = "XtglService";
    define.method = "TreeGoodsKindData";
    define.methodList = "TreeGoodsKindList";
    define.Key = 'CODE';
}
define.initDataParam = function () {
    define.dataParam.CODE = null;
    define.dataParam.NAME = null;
    define.dataParam.LAST_BJ = null;
}
define.newRecord = function () {
    define.dataParam.LAST_BJ = 1;
}
define.IsValidXj = function () {
    if (define.vueObj.dataParam.LAST_BJ == 2) {
        iview.Message.info("该节点已是末级节点不能再添加下级节点！");
        return false;
    }
    return true;
}
define.IsValidDel = function () {
    if (define.vueObj.dataParam.LAST_BJ == 1) {
        iview.Message.info("该节点是非末级节点不能删除！");
        return false;
    }
    return true;
}


