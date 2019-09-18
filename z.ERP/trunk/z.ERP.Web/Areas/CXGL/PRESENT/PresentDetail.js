defineDetail.beforeVue = function () {
    defineDetail.Key = "ID";
    defineDetail.screenParam.idData = [];
};

defineDetail.clearKey = function () {

    defineDetail.dataParam.BRANCHID = null;
    defineDetail.dataParam.ID = null;
    defineDetail.dataParam.NAME = null;
    defineDetail.dataParam.PRICE = null;
    defineDetail.dataParam.STATUS = null;
};

defineDetail.mountedInit = function () {
    defineDetail.btnConfig = [{
        id: "add",
        //authority: "10102001"
    }, {
        id: "edit",
        //authority: "10102001"
    }, {
        id: "del",
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
        iview.Message.info("名称不能为空!");
        return false;
    };
    if (!defineDetail.dataParam.PRICE) {
        iview.Message.info("价值不能为空!");
        return false;
    };
    return true;
}