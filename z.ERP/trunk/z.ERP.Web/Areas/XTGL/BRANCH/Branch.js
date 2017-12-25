define.beforeVue = function () {
    define.screenParam.colDef = [
        {
            title: '分店代码',
            key: 'ID', width: 150,
        },
        {
            title: '分店名称',
            key: 'NAME', width: 250,
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

