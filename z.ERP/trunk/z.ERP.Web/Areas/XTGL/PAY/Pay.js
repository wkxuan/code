define.beforeVue = function () {
    colPay = [
        { title: '支付方式CODE', key: 'CODE', width: 150 },
        { title: '支付方式名称', key: 'NAME', width: 250 }];

    dataPay = [];
}

define.afterSave = function (data) {
    dataPay = data.row;
}