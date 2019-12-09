defineDetail.beforeVue = function () {
    defineDetail.Key = "ID";
};

defineDetail.initDataParam = function () {
    defineDetail.dataParam.BRANCHID = "";
    defineDetail.dataParam.ID = "";
    defineDetail.dataParam.START_DATE = "";
    defineDetail.dataParam.END_DATE = "";
    defineDetail.dataParam.CENT = "";
    defineDetail.dataParam.MONEY = "";
    defineDetail.dataParam.STATUS = "1";
};

defineDetail.mountedInit = function () {
    defineDetail.btnConfig = [{
        id: "add",
        authority: ""
    }, {
        id: "edit",
        authority: "",
        enabled: function (disabled, data) {
            if (disabled && Number(data.STATUS) < 2) {
                return true;
            } else {
                return false;
            }
        }
    }, {
        id: "del",
        authority: "",
    }, {
        id: "save",
        authority: ""
    }, {
        id: "abandon",
        authority: ""
    }, {
        id: "begin",
        authority: "",
        isNewAdd: true,
        name: "启动",
        icon: "md-add",
        fun: function () {
            _.Ajax('Begin', {
                Data: defineDetail.dataParam
            }, function (data) {
                iview.Message.info("启动成功");

                if (window.parent.search != undefined) {
                    window.parent.search.popCallBack(data);
                }
            });
        },
        enabled: function (disabled, data) {
            if (disabled && data.STATUS == 1) {
                return true;
            } else {
                return false;
            }
        }
    }, {
        id: "stop",
        authority: "",
        isNewAdd: true,
        name: "终止",
        icon: "md-add",
        fun: function () {
            _.Ajax('Stop', {
                Data: defineDetail.dataParam
            }, function (data) {
                iview.Message.info("终止成功");

                if (window.parent.search != undefined) {
                    window.parent.search.popCallBack(data);
                }
            });
        },
        enabled: function (disabled, data) {
            if (disabled && data.STATUS == 2) {
                return true;
            } else {
                return false;
            }
        }
    }];
};

defineDetail.showOne = function (data, callback) {
    _.Ajax('SearchRedemptionRules', {
        Data: { ID: data }
    }, function (data) {
        $.extend(defineDetail.dataParam, data.res[0]);
    });
};

defineDetail.IsValidSave = function () {
    if (!defineDetail.dataParam.BRANCHID) {
        iview.Message.info("请选择门店!");
        return false;
    };
    if (!defineDetail.dataParam.START_DATE) {
        iview.Message.info("请选择开始日期!");
        return false;
    };
    if (!defineDetail.dataParam.END_DATE) {
        iview.Message.info("请选结束日期!");
        return false;
    };
    if (new Date(defineDetail.dataParam.START_DATE) > new Date(defineDetail.dataParam.END_DATE)) {
        iview.Message.info("结束日期不能小于开始日期!");
        return false;
    }
    if (!defineDetail.dataParam.CENT) {
        iview.Message.info("请确定积分!");
        return false;
    };
    if (!defineDetail.dataParam.MONEY) {
        iview.Message.info("请确定金额!");
        return false;
    };
    return true;
}