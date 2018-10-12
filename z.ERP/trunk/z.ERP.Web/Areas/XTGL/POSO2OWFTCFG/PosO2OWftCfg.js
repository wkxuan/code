define.beforeVue = function () {

    define.screenParam.colDef = [
    { title: '终端号', key: 'POSNO', width: 400 }
    ];

    define.screenParam.dataDef = [];
    define.service = "XtglService";
    define.method = "GetPosO2OWftCfg";
    define.methodList = "GetPosO2OWftCfg";
    define.Key = 'POSNO';

}