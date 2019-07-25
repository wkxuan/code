define.beforeVue = function () {
    define.screenParam.colDef = [
        {
            title: '门店代码',
            key: 'ID', width: 100,
        },
        {
            title: '门店名称',
            key: 'NAME', width: 280,
        }];

    define.screenParam.dataDef = [];
    define.windowParam = {
        terst: false
    }

    define.service = "XtglService";
    define.method = "GetBranchElement";
    define.methodList = "GetBranch";
    define.Key = 'ID';
}



define.newRecord = function () {
    define.dataParam.STATUS = "1";
}
