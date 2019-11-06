defineDetail.beforeVue = function () {
    defineDetail.Key = "ID";
};

defineDetail.initDataParam = function () {
    defineDetail.dataParam.ID = null;
    defineDetail.dataParam.NAME = "";
    defineDetail.dataParam.YEAR = "";
    defineDetail.dataParam.CONTENT = null;
    defineDetail.dataParam.START_DATE = null;
    defineDetail.dataParam.END_DATE = null;
    defineDetail.dataParam.REPORTER = null;
    defineDetail.dataParam.REPORTER_NAME = null;
    defineDetail.dataParam.REPORTER_TIME = null;
    defineDetail.dataParam.VERIFY = null;
    defineDetail.dataParam.VERIFY_NAME = null;
    defineDetail.dataParam.VERIFY_TIME = null;
    defineDetail.dataParam.STATUS = 1;
};

defineDetail.mountedInit = function () {
    defineDetail.btnConfig = [{
        id: "add",
        authority: "104004"
    }, {
        id: "edit",
        authority: "104004",
        enabled: function (disabled, data) {
            if (disabled && data && data.ID && data.STATUS < 2) {
                return true;
            } else {
                return false;
            }
        }
    }, {
        id: "del",
        authority: "104004",
        enabled: function (disabled, data) {
            return false;
        }
    }, {
        id: "save",
        authority: "104004",
    }, {
        id: "abandon",
        authority: "104004"
    },{
        id: "confirm",
        name: "审核",
        icon: "md-star",
        authority: "104004",
        fun: function () {
            _.Ajax('Check', {
                DefineSave: defineDetail.dataParam
            }, function (data) {
                iview.Message.info("审核成功!");
                window.parent.defineNew.popCallBack(data);
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