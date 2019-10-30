define.beforeVue = function () {
    define.screenParam.colDef = [
        {
            title: "支付编码",
            key: 'PAYID'
        }, {
            title: "支付方式",
            key: 'PAYNAME'
        }];

    define.service = "XtglService";
    define.method = "GetPay_Charges";
    define.methodList = "GetPay_Charges";
    define.Key = 'PAYID';
    define.screenParam.PAYData = [];
    _.Ajax('GetPAY', {        
    }, function (data) {
        if (data.dt) {
            debugger
            define.screenParam.PAYData = [];
            for (var i = 0; i < data.dt.length; i++) {
                define.screenParam.PAYData.push({ value: data.dt[i].PAYID, label: data.dt[i].NAME })
            }
        }
    });
};
define.initDataParam = function () {
    define.dataParam.PAYID = null;
    define.dataParam.FLOOR = null;
    define.dataParam.CEILING = null;
    define.dataParam.RATE = null;
}
define.newRecord = function () {
    define.dataParam.PAYID = null;
    define.dataParam.FLOOR = null;
    define.dataParam.CEILING = null;
    define.dataParam.RATE = null;
}
define.mountedInit = function () {
    _.Ajax('GetBranch', {
        Data: { ID: "" }
    }, function (data) {
        if (data.dt) {
            define.screenParam.branchData = [];
            for (var i = 0; i < data.dt.length; i++) {
                define.screenParam.branchData.push({ value: data.dt[i].ID, label: data.dt[i].NAME })
            }
            define.searchParam.BRANCHID = data.dt[0].ID;
            Vue.set(define.dataParam, "BRANCHID", define.searchParam.BRANCHID);
            define.showlist();
        }
    });
}

define.showOne = function (data, callback) {
    _.Ajax('GetPay_ChargesOne', {
        Data: { PAYID: data, BRANCHID: define.searchParam.BRANCHID }
    }, function (data) {
        $.extend(define.dataParam, data[0]);
        callback && callback();
    });
}

define.otherMethods = {
    branchChange: function (value) {
        define.dataParam.BRANCHID = define.searchParam.BRANCHID;
        define.initDataParam();
        define.showlist();
    },
    payChange: function (data) {
        _.Ajax('GetPay_ChargesOne', {
            Data: { PAYID: data, BRANCHID: define.searchParam.BRANCHID }
        }, function (data) {
            if (data.length == 1) {
                iview.Message.warning("该收款方式已设置，将启用编辑!");
                $.extend(define.dataParam, data[0]);
            } else {
                define.dataParam.FLOOR = null;
                define.dataParam.CEILING = null;
                define.dataParam.RATE = null;
            }
        });
    }
};

define.IsValidSave = function () {
    define.dataParam.BRANCHID = define.searchParam.BRANCHID;
    if (!define.dataParam.BRANCHID) {
        iview.Message.info("请选择门店!");
        return false;
    };
    if (!define.dataParam.PAYID) {
        iview.Message.info("请选择支付方式!");
        return false;
    };
    if (!define.dataParam.FLOOR) {
        iview.Message.info("请确认单笔最低!");
        return false;
    };
    if (!define.dataParam.CEILING) {
        iview.Message.info("请确认单笔封顶!");
        return false;
    };
    if (!define.dataParam.RATE) {
        iview.Message.info("请确认比率!");
        return false;
    };
    if (Number(define.dataParam.RATE) < 0 || Number(define.dataParam.RATE) > 1000) {
        iview.Message.info("比率范围0~1000之间!");
    };
    return true;
}