defineDetail.beforeVue = function () {
    defineDetail.Key = "BRANCHID";
    defineDetail.screenParam.branchData = [];
};

defineDetail.clearKey = function () {

    defineDetail.dataParam.BRANCHID = null;
    defineDetail.dataParam.PRINTCOUNT = null;
    defineDetail.dataParam.HEAD = null;
    defineDetail.dataParam.TAIL = null;
    defineDetail.dataParam.ADQRCODE = null;
    defineDetail.dataParam.ADCONTENT = null;
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
    _.Ajax('GetTicketInfo', {
        Data: { BRANCHID: data }
    }, function (data) {
        $.extend(defineDetail.dataParam, data.dt[0]);     
    });
};

defineDetail.IsValidSave = function () {
    if (!defineDetail.dataParam.BRANCHID) {
        iview.Message.info("请选择门店!");
        return false;
    };

    if (!defineDetail.dataParam.PRINTCOUNT) {
        iview.Message.info("请填写打印次数");
        return false;
    };

    if (!defineDetail.dataParam.HEAD) {
        iview.Message.info("票头文字不能为空!");
        return false;
    };
    return true;
}