defineDetail.beforeVue = function () {
    defineDetail.Key = "ID";
};

defineDetail.initDataParam = function () {
    defineDetail.dataParam.ID = null;
    defineDetail.dataParam.NAME = "";
    defineDetail.dataParam.YEAR = "";
    defineDetail.dataParam.CONTENT = "";
    defineDetail.dataParam.START_DATE = "";
    defineDetail.dataParam.END_DATE = "";
    defineDetail.dataParam.REPORTER = "";
    defineDetail.dataParam.REPORTER_NAME = "";
    defineDetail.dataParam.REPORTER_TIME = "";
    defineDetail.dataParam.VERIFY = "";
    defineDetail.dataParam.VERIFY_NAME = "";
    defineDetail.dataParam.VERIFY_TIME = "";
    defineDetail.dataParam.STATUS = 1;
};

defineDetail.mountedInit = function () {
    defineDetail.btnConfig = [{
        id: "add",
        authority: "11000101"
    }, {
        id: "edit",
        authority: "11000101",
        enabled: function (disabled, data) {
            if (disabled && data && data.ID && data.STATUS < 2) {
                return true;
            } else {
                return false;
            }
        }
    }, {
        id: "del",
        authority: "11000101",
        enabled: function (disabled, data) {
            return false;
        }
    }, {
        id: "save",
        authority: "11000101",
    }, {
        id: "abandon",
        authority: "11000101"
    },{
        id: "confirm",
        name: "审核",
        icon: "md-star",
        authority: "11000102",
        fun: function () {
            _.Ajax('Check', {
                DefineSave: defineDetail.dataParam
            }, function (data) {
                iview.Message.info("审核成功!");
                //     window.parent.defineNew.popCallBack(data);
                setTimeout(function () {
                    window.location.reload();
                }, 100);
            });
        },
        enabled: function (disabled, data) {
            if (disabled && data.ID && data.STATUS == 1) {
                return true;
            } else {
                return false;
            }
        },
        isNewAdd: true
    }];
};

defineDetail.showOne = function (data, callback) {
    _.Ajax('ShowOneData', {
        Data: { ID: data }
    }, function (data) {
        $.extend(defineDetail.dataParam, data.res);
    });
};

defineDetail.IsValidSave = function () {
    if (!defineDetail.dataParam.NAME) {
        iview.Message.info("主题名称不能为空!");
        return false;
    };
    if (!defineDetail.dataParam.YEAR) {
        iview.Message.info("年度不能为空!");
        return false;
    };
    if (!defineDetail.dataParam.START_DATE) {
        iview.Message.info("开始日期不能为空!");
        return false;
    };
    if (!defineDetail.dataParam.END_DATE) {
        iview.Message.info("结束日期不能为空!");
        return false;
    };
    if (new Date(defineDetail.dataParam.START_DATE).Format('yyyy-MM-dd') > new Date(defineDetail.dataParam.END_DATE).Format('yyyy-MM-dd')) {
        iview.Message.info(`结束日期不能小于开始日期!`);
        return false;
    };
    return true;
}