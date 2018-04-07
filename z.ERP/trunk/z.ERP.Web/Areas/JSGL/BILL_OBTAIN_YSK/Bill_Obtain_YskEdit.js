editDetail.beforeVue = function () {
    editDetail.others = false;
    editDetail.branchid = true;
    editDetail.Key = 'BILLID';
    //预收款收款
    editDetail.dataParam.TYPE = 1;
}
editDetail.showOne = function (data, callback) {
    _.Ajax('SearchBill_Obtain_Ysk', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.billObtainYsk);
        callback && callback(data);
    });
}