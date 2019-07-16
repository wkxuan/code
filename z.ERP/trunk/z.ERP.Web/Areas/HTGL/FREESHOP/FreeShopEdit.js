editDetail.beforeVue = function () {
    //初始化弹窗所要传递参数
    editDetail.screenParam.showPopContract = false;
    editDetail.screenParam.srcPopContract = __BaseUrl + "/" + "Pop/Pop/PopContractList/";

    editDetail.screenParam.popParam = {};

    editDetail.screenParam.colDef = [
        { title: "商铺代码", key: "CODE", width: 150, },
        { title: '业态代码', key: 'CATEGORYCODE', width: 100 },
        { title: '业态名称', key: 'CATEGORYNAME', width: 100 },
    ]
}

editDetail.showOne = function (data, callback) {
    _.Ajax('ShowOneEdit', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.freeShop[0]);
        editDetail.dataParam.FREESHOPITEM = data.freeShopItem[0];
        callback && callback(data);
    });
}

editDetail.mountedInit = function () {
    editDetail.btnConfig = [{
        id: "add",
        authority: "10600301"
    }, {
        id: "edit",
        authority: "10600301"
    }, {
        id: "del",
        authority: "10600301"
    }, {
        id: "save",
        authority: "10600301"
    }, {
        id: "abandon",
        authority: "10600301"
    }, {
        id: "confirm",
        name: "审核",
        icon: "md-star",
        authority: "10600302",
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
            if (!disabled && data.STATUS == 1) {
                return true;
            } else {
                return false;
            }
        },
        isNewAdd: true
    }, {
        id: "stop",
        name: "终止",
        icon: "md-close",
        authority: "10600302",
        fun: function () {
            _.Ajax('StopData', {
                Data: { BILLID: editDetail.dataParam.BILLID },
            }, function (data) {
                iview.Message.info("终止成功");
                setTimeout(function () {
                    window.location.reload();
                }, 100);
            });
        },
        enabled: function (disabled, data) {
            if (!disabled && data.STATUS == 2) {
                return true;
            } else {
                return false;
            }
        },
        isNewAdd: true
    }];
};

///html中绑定方法
editDetail.otherMethods = {
    srchContract: function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info("请先选择门店!");
            return;
        }
        editDetail.screenParam.showPopContract = true;
        editDetail.screenParam.popParam = { YXHTBJ: 1, FREESHOPBJ: 1, BRANCHID: editDetail.dataParam.BRANCHID };
    },
}

///接收弹窗返回参数
editDetail.popCallBack = function (data) {
    if (editDetail.screenParam.showPopContract) {
        editDetail.dataParam.CONTRACTID = data.sj[0].CONTRACTID;
        editDetail.screenParam.showPopContract = false;
        _.Ajax('GetContractList', {
            Data: { CONTRACTID: editDetail.dataParam.CONTRACTID }
        }, function (data) {
            if (data.contract.length > 0) {
                editDetail.dataParam.MERCHANTID = data.contract[0].MERCHANTID;
                editDetail.dataParam.SHMC = data.contract[0].SHMC;
                editDetail.dataParam.STYLE = data.contract[0].STYLE;
                editDetail.dataParam.STYLEMC = data.contract[0].STYLEMC;
                editDetail.dataParam.FREESHOPITEM = data.shop;
            }
            else {
                editDetail.dataParam.MERCHANTID = null;
                editDetail.dataParam.SHMC = null;
            }
        })
    }
};
editDetail.clearKey = function () {
    editDetail.dataParam.BILLID = null;
    editDetail.dataParam.BRANCHID = null;
    editDetail.dataParam.CONTRACTID = null;
    editDetail.dataParam.FREEDATE = null;
    editDetail.dataParam.MERCHANTID = null;
    editDetail.dataParam.SHMC = null;
    editDetail.dataParam.STATUSMC = null;
    editDetail.dataParam.DESCRIPTION = null;
    editDetail.dataParam.FREESHOPITEM = [];
}
editDetail.IsValidSave = function () {
    if (!editDetail.dataParam.BRANCHID) {
        iview.Message.info("请选择门店!");
        return false;
    }
    if (!editDetail.dataParam.CONTRACTID) {
        iview.Message.info("请选择租约!");
        return false;
    };
    if (!editDetail.dataParam.FREEDATE) {
        iview.Message.info("请确认退铺日期!");
        return false;
    };
    if (editDetail.dataParam.FREESHOPITEM.length == 0) {
        iview.Message.info("请确认店铺信息!");
        return false;
    };

    return true;
}