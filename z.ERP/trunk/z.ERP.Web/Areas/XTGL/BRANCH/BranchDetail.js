defineDetail.beforeVue = function () {
    defineDetail.Key = "ID";
};

defineDetail.clearKey = function () {
    defineDetail.dataParam.ID = null;
    defineDetail.dataParam.NAME = null;
    defineDetail.dataParam.ORGID = null;
    defineDetail.dataParam.AREA_BUILD = null;
    defineDetail.dataParam.AREA_USABLE = null;
    defineDetail.dataParam.AREA_RENTABLE = null;
    defineDetail.dataParam.STATUS = 1;
    defineDetail.dataParam.PRINTNAME = null;
    defineDetail.dataParam.CONTACT = null;
    defineDetail.dataParam.CONTACT_NUM = null;
    defineDetail.dataParam.BANK = null;
    defineDetail.dataParam.ACCOUNT = null;
    defineDetail.dataParam.ADDRESS = null;
};

defineDetail.mountedInit = function () {
    defineDetail.btnConfig = [{
        id: "add",
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

defineDetail.showOne = function (id) {
    _.Search({
        Service: "XtglService",
        Method: "GetBranch",
        Data: { ID: id },
        Success: function (data) {
            $.extend(defineDetail.dataParam, data.rows[0]);
        }
    });
};

defineDetail.IsValidSave = function () {
    if (!defineDetail.dataParam.NAME) {
        iview.Message.info("门店名称不能为空!!");
        return false;
    };
    if (!defineDetail.dataParam.ORGID) {
        iview.Message.info("管理部门不能为空!!");
        return false;
    };
    if (!defineDetail.dataParam.AREA_BUILD) {
        iview.Message.info("建筑面积不能为空!!");
        return false;
    };
    if (!defineDetail.dataParam.STATUS) {
        iview.Message.info("停用标记不能为空!");
        return false;
    };
    return true;
};