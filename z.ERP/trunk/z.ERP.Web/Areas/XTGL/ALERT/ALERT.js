define.beforeVue = function () {
    define.screenParam.colDef = [
        { title: '代码', key: 'ID', width: 150 },
        { title: '名称', key: 'MC', width: 150 },
    ]
    define.screenParam.dataDef = [];

    define.service = "XtglService";
    define.method = "GetAlert";
    define.methodList = "GetAlert";
    define.Key = 'ID';
}
