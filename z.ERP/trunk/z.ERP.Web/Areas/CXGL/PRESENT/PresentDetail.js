defineDetail.beforeVue = function () {
    defineDetail.Key = "ID";
    defineDetail.screenParam.idData = [];
    defineDetail.dataParam.STATUSMC = "未使用";
};

defineDetail.initDataParam = function () {
    defineDetail.dataParam.ID = "";
    defineDetail.dataParam.BRANCHID = "";
    defineDetail.dataParam.NAME = "";
    defineDetail.dataParam.PRICE = "";
    defineDetail.dataParam.STATUSMC = "未使用"
};

defineDetail.mountedInit = function () {
    defineDetail.btnConfig = [{
        id: "add",
        authority: "11000201"
    }, {
        id: "edit",
        authority: "11000201"
    }, {
        id: "del",
        authority: "11000201"
    }, {
        id: "save",
        authority: "11000201"
    }, {
        id: "abandon",
        authority: "11000201"
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