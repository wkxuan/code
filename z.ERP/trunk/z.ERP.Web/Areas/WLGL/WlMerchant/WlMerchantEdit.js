editDetail.beforeVue = function () {

    editDetail.others = false;
    editDetail.branchid = false;
    editDetail.otherPanel = false;
    editDetail.service = "WyglService";
    editDetail.method = "GetWlMerchant";
    editDetail.Key = 'MERCHANTID';
    editDetail.dataParam.STATUS = "1";
};

editDetail.showOne = function (data, callback) {
    _.Ajax('SearchWlMerchant', {
        Data: { MERCHANTID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.merchant);
        editDetail.dataParam.BILLID = data.merchant.MERCHANTID;
        callback && callback(data);
    });
}


editDetail.clearKey = function () {
    editDetail.dataParam.MERCHANTID = null;
    editDetail.dataParam.NAME = null;
    editDetail.dataParam.SH = null;
    editDetail.dataParam.BANK_NAME = null;
    editDetail.dataParam.BANK = null;
    editDetail.dataParam.ADRESS = null;
    editDetail.dataParam.CONTACTPERSON = null;
    editDetail.dataParam.PHONE = null;
    editDetail.dataParam.PIZ = null;
    editDetail.dataParam.WEIXIN = null;
    editDetail.dataParam.QQ = null;
}


editDetail.IsValidSave = function () {
    if (!editDetail.dataParam.NAME) {
        iview.Message.info("请输入供货商名称!");
        return false;
    };
    return true;
}
//按钮初始化
editDetail.mountedInit = function () {
    editDetail.btnConfig = [{
        id: "add",
        authority: "10900101"
    }, {
        id: "edit",
        authority: "10900101"
    }, {
        id: "del",
        authority: "10900101"
    }, {
        id: "save",
        authority: "10900101"
    }, {
        id: "abandon",
        authority: "10900101"
    }, {
        id: "confirm",
        name: "审核",
        icon: "md-star",
        authority: "10900102",
        fun: function () {
            _.Ajax('ExecData', {
                Data: { MERCHANTID: editDetail.dataParam.MERCHANTID },
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