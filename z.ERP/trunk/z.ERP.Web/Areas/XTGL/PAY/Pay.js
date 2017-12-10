define.beforeVue = function () {

    define.dataParam.CODE = '';

    define.screenParam.colDef = [
        { title: '支付方式代码', key: 'CODE', width: 150 },
        { title: '支付方式名称', key: 'NAME', width: 250 }];

    define.screenParam.dataDef = [];

    define.screenParam.cs = function () {
        define.dataParam.CODE = "1";
        define.dataParam.NAME = "就是这样";
        define.dataParam.TYPE = "1";
    }
}


define.search = function () {
    _.Search({
        Service: 'TestService',
        Method: 'GetPay',
        Data: {},
        Success: function (data) {
            define.screenParam.dataDef = data.rows;
        }
    })
}
