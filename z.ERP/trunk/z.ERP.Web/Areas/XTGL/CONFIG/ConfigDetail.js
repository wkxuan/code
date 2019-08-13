defineDetail.beforeVue = function () {
    defineDetail.Key = "ID";
};

defineDetail.clearKey = function () {
    defineDetail.dataParam.ID = null;
    defineDetail.dataParam.DEF_VAL = null;
    defineDetail.dataParam.CUR_VAL = null;
    defineDetail.dataParam.MAX_VAL = null;
    defineDetail.dataParam.MIN_VAL = null;
    defineDetail.dataParam.DESCRIPTION = null;
};

defineDetail.mountedInit = function () {
    defineDetail.btnConfig = [{
        id: "add",
        enabled: function () {
            return false;
        },
        authority: ""
    }, {
        id: "edit",
        authority: ""
    }, {
        id: "del",
        enabled: function () {
            return false;
        },
        authority: ""
    }, {
        id: "save",
        authority: ""
    }, {
        id: "abandon",
        authority: ""
    }];
};

defineDetail.showOne = function (data) {
    _.Search({
        Service: "XtglService",
        Method: "GetConfig",
        Data: { ID: data },
        Success: function (data) {
            $.extend(defineDetail.dataParam, data.rows[0]);
        }
    });
};

defineDetail.IsValidSave = function () {
    //if (!defineDetail.dataParam.DEF_VAL) {
    //    iview.Message.info("缺省值不能为空!!");
    //    return false;
    //};
    if (defineDetail.dataParam.CUR_VAL == null) {
        iview.Message.info("当前值不能为空!!");
        return false;
    };
    //if (!defineDetail.dataParam.MAX_VAL) {
    //    iview.Message.info("最大值不能为空!!");
    //    return false;
    //};
    //if (!defineDetail.dataParam.MIN_VAL) {
    //    iview.Message.info("最小值不能为空!");
    //    return false;
    //};
    //if (!defineDetail.dataParam.DESCRIPTION) {
    //    iview.Message.info("描述不能为空!");
    //    return false;
    //};
    return true;
};