define.beforeVue = function () {

    define.screenParam.colDef = [
        {
            title: '支付方式代码',
            key: 'CODE', width: 150,
            filters: [
                { label: '过滤', value: define.screenParam.SrchParam }
            ],
            filterMultiple: false,
            filterMethod: function (value, row) {
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
            filterMethod: function (value, row) {
                return row.NAME.indexOf(define.screenParam.SrchParam) > -1;
            }
        }];

    define.screenParam.dataDef = [];
}


define.search = function () {
    _.Search({
        Service: 'XtglService',
        Method: 'GetPay',
        Data: {},
        Success: function (data) {
            define.screenParam.dataDef = data.rows;
        }
    })
}

define.newRecord = function () {
    define.dataParam.VOID_FLAG = "1";
}

define.IsValidSave = function (param) {
    if (!define.dataParam.NAME) {
        param.$Message.info("名称不能为空!");
        return false;
    }
    if (!define.dataParam.TYPE) {
        param.$Message.info("类型不能为空!");
        return false;
    }

    if (!define.dataParam.FK) {
        param.$Message.info("返款标记不能为空!");
        return false;
    }
    if (!define.dataParam.JF) {
        param.$Message.info("积分标记不能为空!");
        return false;
    }

    if (!define.dataParam.ZLFS) {
        param.$Message.info("找零方式不能为空!");
        return false;
    }

    if (!define.dataParam.FLAG) {
        param.$Message.info("显示顺序不能为空!");
        return false;
    }
    return true;
}
