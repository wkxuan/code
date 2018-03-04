define.beforeVue = function () {

    define.screenParam.colDef = [
        {
            title: "代码",
            key: 'CODE', width: 150
        },
        {
            title: '名称',
            key: 'NAME', width: 250
        }];

    define.screenParam.dataDef = [];
    define.service = "XtglService";
    define.method = "GetShopElement";
    define.methodList = "GetShop";
    define.Key = 'SHOPID';
}


define.newRecord = function () {
    define.dataParam.TYPE = "1";
}
