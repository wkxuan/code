define.beforeVue = function () {
    define.screenParam.colDef = [
        { title: '代码', key: 'ID', width: 150 },
        { title: '名称', key: 'NAME'},
    ]

    define.service = "XtglService";
    define.method = "GetFkfs";
    define.methodList = "GetFkfs";
    define.Key = 'ID';
}
