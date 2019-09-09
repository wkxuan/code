define.beforeVue = function () {
    define.service = "XtglService";
    define.method = "TreeCategoryData";
    define.methodList = "TreeCategoryList";
    define.Key = 'CATEGORYCODE';
    define.dataParam.COLOR = " ";

    define.dataParam.CATEGORYCODE = null;
    define.dataParam.CATEGORYNAME = null;
    define.dataParam.LEVEL_LAST = null;
}
define.IsValidXj = function () {
    if (define.dataParam.LEVEL_LAST == 2) {
        iview.Message.info("该节点已是末级节点不能再添加下级节点！");
        return false;
    }
    return true;
}
define.IsValidDel = function () {
    if (define.dataParam.LEVEL_LAST == 1) {
        iview.Message.info("该节点是非末级节点不能删除！");
        return false;
    }
    return true;
}

