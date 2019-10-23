define.beforeVue = function () {
    define.screenParam.colDef = [
        { title: '代码', key: 'ID', width: 150 },
        { title: '名称', key: 'NAME' },
    ]

    define.service = "WyglService";
    define.method = "GetComplainDept";
    define.methodList = "GetComplainDept";
    define.Key = 'ID';
}
define.initDataParam = function () {
    define.dataParam.ID = "";
    define.dataParam.NAME = "";
}