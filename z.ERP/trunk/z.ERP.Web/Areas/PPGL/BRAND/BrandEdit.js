editDetail.beforeVue = function () {   
    editDetail.service = "XtglService";
    editDetail.method = "GetBrandData";
    editDetail.branchid = false;
    editDetail.otherPanel = false;
    editDetail.Key = "ID";
    editDetail.screenParam.CATEGORYIDCASCADER = [];
    _.Ajax('SearchInit', {
        Data: {}
    }, function (data) {
        Vue.set(editDetail.screenParam, "CATEData", data.treeOrg.Obj);
    });
}


editDetail.showOne = function (data, callback) {
    _.Ajax('SearchElement', {
        Data: { ID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.main[0]);
        editDetail.dataParam.BILLID = data.main[0].ID;

        editDetail.dataParam.CATEGORYID = data.main[0].CATEGORYID;
        if (data.main[0].CATEGORYIDCASCADER != null) {
            editDetail.screenParam.CATEGORYIDCASCADER = data.main[0].CATEGORYIDCASCADER.split(",");
        } else {
            editDetail.screenParam.CATEGORYIDCASCADER = [];
        }
        callback && callback(data);

    });
};

//按钮初始化
editDetail.mountedInit = function () {
    editDetail.btnConfig = [{
        id: "add",
        authority: "10200201"
    }, {
        id: "edit",
        authority: "10200201",
        enabled: function (disabled, data) {
            if (!disabled && data.STATUS !=0) {
                return true;
            } else {
                return false;
            }
        },
    }, {
        id: "del",
        authority: "10200201"
    }, {
        id: "save",
        authority: "10200201"
    }, {
        id: "abandon",
        authority: "10200201"
    }, {
        id: "confirm",
        name: "审核",
        icon: "md-star",
        authority: "10200202",
        fun: function () {
            _.Ajax('ExecData', {
                Data: { ID: editDetail.dataParam.ID },
            }, function (data) {
                iview.Message.info("审核成功");
                setTimeout(function () {
                    window.location.reload();
                }, 100);
            });
        },
        enabled: function (disabled, data) {
            if (!disabled && data.STATUS < 2) {
                return true;
            } else {
                return false;
            }
        },
        isNewAdd: true
    }];
};


editDetail.clearKey = function () {
    editDetail.dataParam.ID = null;
    editDetail.screenParam.CATEGORYIDCASCADER = [];
    editDetail.dataParam.NAME = null;
    editDetail.dataParam.CATEGORYID = null;
    editDetail.dataParam.ADRESS = null;
    editDetail.dataParam.CONTACTPERSON = null;
    editDetail.dataParam.PHONENUM = null;
    editDetail.dataParam.WEIXIN = null;
    editDetail.dataParam.PIZ = null;
    editDetail.dataParam.QQ = null;
    editDetail.dataParam.STATUSMC = "未审核";
}


editDetail.otherMethods = {
    orgChange: function (value, selectedData) {
        editDetail.dataParam.CATEGORYID = value[value.length - 1];
    },
};


editDetail.IsValidSave = function () {


    if (!editDetail.dataParam.NAME) {
        iview.Message.info("请确认品牌名称!");
        return false;
    };


    if (!editDetail.dataParam.CATEGORYID) {
        iview.Message.info("请确认品牌业态!");
        return false;
    };

    return true;
}