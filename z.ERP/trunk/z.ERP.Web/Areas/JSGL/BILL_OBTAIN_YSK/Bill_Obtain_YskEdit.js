editDetail.beforeVue = function () {
    editDetail.others = false;
    editDetail.branchid = true;
    editDetail.Key = 'BILLID';
    //预收款收款
    editDetail.dataParam.TYPE = 1;
    editDetail.dataParam.popParam = {};
    editDetail.dataParam.showPopMerchant = false;
    editDetail.dataParam.srcPopMerchant = __BaseUrl + "/" + "Pop/Pop/PopMerchantList/";
}
editDetail.showOne = function (data, callback) {
    _.Ajax('SearchBill_Obtain_Ysk', {
        Data: { BILLID: data }
    }, function (data) {
        debugger
        $.extend(editDetail.dataParam, data.billObtainYsk);
        Vue.set(editDetail.dataParam, data.billObtainYsk);
        editDetail.dataParam.BILLID = data.billObtainYsk.BILLID;
        callback && callback(data);
    });
}

editDetail.otherMethods = {
    SelMerchant: function () {
        editDetail.dataParam.showPopMerchant = true;
    }
}

editDetail.popCallBack = function (data) {

    if (editDetail.dataParam.showPopMerchant) {
        editDetail.dataParam.showPopMerchant = false;
        for (var i = 0; i < data.sj.length; i++) {
            editDetail.dataParam.MERCHANTID = data.sj[i].MERCHANTID;
            editDetail.dataParam.MERCHANTNAME = data.sj[i].NAME;
        }
    }
};