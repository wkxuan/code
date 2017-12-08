define.beforeVue = function () {

    define.dataParam.CODE = '';

    define.screenParam.colPay = [
        { title: '支付方式CODE', key: 'CODE', width: 150 },
        { title: '支付方式名称', key: 'NAME', width: 250 }];

    define.screenParam.dataPay = [];


    define.screenParam.colPay1 = [
    { title: 'ID', key: 'ID', width: 150 },
    { title: '名称', key: 'NAME', width: 250 }];

    //define.screenParam.dataPay1 = [];

    //define.screenParam.sure = function () {
    //    define.dataParam.CODE = "1";
    //    define.dataParam.NAME = "就是这样";
    //    define.dataParam.TYPE = "1";
    //}

    //define.screenParam.table = function () {
    //    var itemList = [];
    //    itemList.push({ ID: 1, NAME: '和' });
    //    define.screenParam.dataPay1 = itemList;


    //    var itemList1 = [];
    //    itemList1.push({ CODE: 1, NAME: '和和和和和和' });
    //    define.screenParam.dataPay = itemList1;
    //}
}


define.search = function () {
    _.Search({
        Service: 'TestService',
        Method: 'GetPay',
        Data: {},
        Success: function (data) {
            define.screenParam.dataPay = data.rows;
        }
    })
}