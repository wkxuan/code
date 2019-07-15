define.beforeVue = function () {

    define.screenParam.colDef = [
    { title: '单位编号', key: 'ID', width: 90 },
    { title: '单位名称', key: 'NAME' },
    { title: "分店", key: 'BRANCHNAME' }

    ];

    define.screenParam.dataDef = [];
    define.service = "XtglService";
    define.method = "GetFeeAccount";
    define.methodList = "GetFeeAccount";
    define.Key = 'ID';

}

define.IsValidSave = function () {
    if (!define.dataParam.BRANCHID) {
        iview.Message.info("分店不能为空!");
        return false;
    }
    if (!define.dataParam.NAME) {
        iview.Message.info("单位名称不能为空!");
        return false;
    }
    return true;
}