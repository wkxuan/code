editDetail.beforeVue = function () {
    editDetail.others = false;
    editDetail.branchid = true;
    editDetail.otherPanel = false;
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
editDetail.mountedInit = function () {
    editDetail.btnConfig = [{
        id: "add",
        authority: "10700401"
    }, {
        id: "edit",
        authority: "10700401"
    }, {
        id: "del",
        authority: "10700401"
    }, {
        id: "save",
        authority: "10700401"
    }, {
        id: "abandon",
        authority: "10700401"
    }, {
        id: "confirm",
        name: "审核",
        icon: "md-star",
        authority: "10700402",
        fun: function () {
            _.Ajax('ExecData', {
                Data: { BILLID: editDetail.dataParam.BILLID },
            }, function (data) {
                iview.Message.info("审核成功");
                setTimeout(function () {
                    window.location.reload();
                }, 100);
            });
        },
        enabled: function (disabled, data) {
            if (!disabled && data.STATUS < 2) {
                return true;
            } else {
                return false;
            }
        },
        isNewAdd: true
    }];
};
editDetail.IsValidSave = function () {
    if (!editDetail.dataParam.BRANCHID) {
        iview.Message.info("请选择分店!");
        return false;
    };
    if (!editDetail.dataParam.MERCHANTID) {
        iview.Message.info("请选择商户!");
        return false;
    };
    if (!editDetail.dataParam.FKFSID) {
        iview.Message.info("请选择付款方式!");
        return false;
    };
    if (!editDetail.dataParam.FEE_ACCOUNT_ID) {
        iview.Message.info("请选收费单位!");
        return false;
    };
    if (!editDetail.dataParam.NIANYUE) {
        iview.Message.info("请确认债券发生月!");
        return false;
    };
    return true;
}