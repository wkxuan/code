define.beforeVue = function () {

    define.screenParam.colDef = [
        {
            title: "代码",
            key: 'FILECODE', width: 150
        },
        {
            title: '名称',
            key: 'FILENAME', width: 250
        }];

    define.screenParam.dataDef = [];

    define.service = "XtglService";
    define.method = "GetEnergyFilesElement";
    define.methodList = "GetEnergyFiles";
    define.getKey = 'FILEID';
}



