define.beforeVue = function () {

    define.screenParam.colDef = [
        {
            title: "分店代码",
            key: 'ID', width: 150
        },
        {
            title: '分店名称',
            key: 'NAME', width: 250
        }];

    define.screenParam.dataDef = [];


    define.service = "XtglService";
    define.method = "GetBranchElement";
    define.methodList = "GetBranch";
}


define.getKey = 'ID';

define.newRecord = function () {
    define.dataParam.STATUS = "1";
}

define.IsValidSave = function (param) {
    if (!define.dataParam.NAME) {
        param.$Message.info("名称不能为空!");
        return false;
    }
    if (!define.dataParam.ORGID) {
        param.$Message.info("管理部门不能为空!");
        return false;
    }

    if (!define.dataParam.AREA_BUILD) {
        param.$Message.info("建筑面积不能为空!");
        return false;
    }
    return true;
}

