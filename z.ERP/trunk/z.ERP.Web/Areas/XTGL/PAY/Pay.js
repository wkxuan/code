define.beforeVue = function () {
    define.dataParam.colPay = [
        { title: '支付方式CODE', key: 'CODE', width: 150 },
        { title: '支付方式名称', key: 'NAME', width: 250 }];

    define.dataParam.dataPay = [];
}


define.search = function () {
    _.Search({
        Service: 'TestService',
        Method: 'GetPay',
        Data: {},
        Success: function (data) {
            define.dataParam.dataPay = data.rows;
        }
    })
}