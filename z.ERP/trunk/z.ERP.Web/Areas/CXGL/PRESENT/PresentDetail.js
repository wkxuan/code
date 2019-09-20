defineDetail.beforeVue = function () {
    defineDetail.Key = "ID";
    defineDetail.screenParam.idData = [];
    defineDetail.dataParam.STATUSMC = "未使用";
};

defineDetail.clearKey = function () {

    defineDetail.dataParam.ID = [];
    defineDetail.dataParam.BRANCHID = null;
    defineDetail.dataParam.NAME = null;
    defineDetail.dataParam.PRICE = null;
    defineDetail.dataParam.STATUSMC = "未使用"
};


defineDetail.mountedInit = function () {
    defineDetail.btnConfig = [{
        id: "add",
        //authority: "10102001"
    }, {
        id: "edit",
        enabled: function (disabled, data) {
            if (!disabled && data && data.STATUS < 2) {
                return true;
            } else {
                return false;
            }
        }
        //authority: "10102001"
    }, {
        id: "del",
        enabled: function (disabled, data) {
            if (!disabled && data && data.STATUS < 2) {
                return true;
            } else {
                return false;
            }
        }
        //authority: "10102001"

    }, {
        id: "save",
        //authority: "10102001"
    }, {
        id: "abandon",
        //authority: "10102001"
    }];
};
defineDetail.showOne = function (data, callback) {
    _.Ajax('GetPresent', {
        Data: { ID: data }
    }, function (data) {
        $.extend(defineDetail.dataParam, data.dt[0]);     
    });
};

defineDetail.IsValidSave = function () {
    if (!defineDetail.dataParam.BRANCHID) {
        iview.Message.info("请选择门店!");
        return false;
    };
    if (!defineDetail.dataParam.NAME) {
        iview.Message.info("赠品名称不能为空!");
        return false;
    };
    if (!defineDetail.dataParam.PRICE) {
        iview.Message.info("价值不能为空!");
        return false;
    };
    //if (define.dataParam.STATUS == "2") {
    //    iview.Message.info("数据已使用状态不能更改");
    //    return false;
    //};
    return true;
}