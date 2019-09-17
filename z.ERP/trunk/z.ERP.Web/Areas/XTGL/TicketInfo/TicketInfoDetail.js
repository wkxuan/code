defineDetail.beforeVue = function () {
    defineDetail.Key = "BRANCHID";
    defineDetail.screenParam.branchData = [];
};

defineDetail.clearKey = function () {

    defineDetail.dataParam.BRANCHID = null;
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
    if (!defineDetail.dataParam.HEAD) {
        iview.Message.info("票头文字不能为空!");
        return false;
    };
    if (!defineDetail.dataParam.TAIL) {
        iview.Message.info("票尾文字不能为空!");
        return false;
    };
    if (!defineDetail.dataParam.ADQRCODE) {
        iview.Message.info("二维码广告位不能为空!");
        return false;
    };
    if (!defineDetail.dataParam.ADCONTENT) {
        iview.Message.info("文字广告位不能为空!");
        return false;
    };

    return true;
}