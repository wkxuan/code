define.beforeVue = function () {

    define.screenParam.colDef = [
        {
            title: '编码',
            key: 'ID', width: 100
        },
        {
            title: "名称",
            key: 'NAME', 
        }, {
            title: "布局颜色",
            key: 'COLOR', tooltip: true
        }];
    define.screenParam.dataDef = [];
    define.service = "DpglService";
    define.method = "SearchPUBLICDATA";
    define.methodList = "SearchPUBLICDATA";
    define.Key = 'ID';
    define.dataParam.COLOR = "#FFFFFF";
}
define.newRecord = function () {
    define.dataParam.ID = "";
    define.dataParam.NAME = "";
    define.dataParam.IMAGEURL = "";
    define.dataParam.COLOR = "#FFFFFF";

}

define.otherMethods = {
   
}


define.IsValidSave = function () {
    if (!define.dataParam.NAME) {
        iview.Message.info("请名称!");
        return false;
    };
    if (!define.dataParam.IMAGEURL) {
        iview.Message.info("请选择图标!");
        return false;
    };
    if (!define.dataParam.COLOR) {
        iview.Message.info("请选择颜色!");
        return false;
    };
    return true;
}