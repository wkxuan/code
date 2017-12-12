define.beforeVue = function () {

    define.dataParam.CODE = '';

    define.screenParam.colDef = [
        {
            title: '支付方式代码',
            key: 'CODE', width: 150,
            filters: [
                { label: '过滤', value: define.screenParam.SrchParam }
            ],
            filterMultiple: false,
            filterMethod (value, row) {
                return row.CODE.indexOf(define.screenParam.SrchParam) > -1;
            }
        },
        {
            title: '支付方式名称',
            key: 'NAME', width: 250,

            filters: [
                { label: '过滤', value: define.screenParam.SrchParam }
            ],
            filterMultiple: false,
            filterMethod (value, row) {
                return row.NAME.indexOf(define.screenParam.SrchParam) > -1;
            }
        }];

    define.screenParam.dataDef = [];
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

define.IsValidSave = function (param) {
    if (!define.dataParam.NAME) {
        param.$Message.info("名称不能为空!");
        return false;
    }
    return true;
}
