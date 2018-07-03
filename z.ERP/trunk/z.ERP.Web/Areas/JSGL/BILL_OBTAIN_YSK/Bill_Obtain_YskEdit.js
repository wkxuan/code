editDetail.beforeVue = function () {
    editDetail.others = false;
    editDetail.branchid = true;
    editDetail.Key = 'BILLID';
    //预收款收款
    editDetail.dataParam.TYPE = 1;

    editDetail.screenParam.ParentMerchant = {};
}
editDetail.showOne = function (data, callback) {
    _.Ajax('SearchBill_Obtain_Ysk', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.billObtainYsk);
        callback && callback(data);
    });
}

editDetail.otherMethods = {
    //点击商户弹窗
    Merchant: function () {
        Vue.set(editDetail.screenParam, "PopMerchant", true);
    },
    //商户弹窗返回
    MerchantBack: function (val) {
        Vue.set(editDetail.screenParam, "PopMerchant", false);
        editDetail.dataParam.MERCHANTID = val.sj[0].MERCHANTID;
        editDetail.dataParam.MERNAME = val.sj[0].NAME;
    },
}