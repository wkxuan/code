define.beforeVue = function () {

    define.screenParam.colDef = [
        {
            title: "收费项目代码",
            key: 'TRIMID', width: 150
        },
        {
            title: '收费项目名称',
            key: 'NAME', width: 250
        }];

    define.screenParam.dataDef = [];
}


define.search = function () {
    _.Search({
        Service: 'TestService',
        Method: 'GetFeeSubject',
        Data: {},
        Success: function (data) {
            define.screenParam.dataDef = data.rows;
        }
    })
}

define.newRecord = function () {
    define.dataParam.VOID_FLAG = "1";
}
