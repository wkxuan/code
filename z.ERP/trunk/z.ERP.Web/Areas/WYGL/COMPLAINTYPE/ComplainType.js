define.beforeVue = function () {
    define.screenParam.colDef = [
        { title: '代码', key: 'ID', width: 150 },
        { title: '名称', key: 'NAME' },
    ]

    define.service = "WyglService";
    define.method = "GetComplainType";
    define.methodList = "GetComplainType";
    define.Key = 'ID';
}
