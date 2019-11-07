﻿defineDetail.beforeVue = function () {
    defineDetail.Key = "ID";
    defineDetail.screenParam.branchData = [];
    defineDetail.screenParam.regionData = [];
};

defineDetail.initDataParam = function () {
    defineDetail.dataParam.STATUS = 1;
    defineDetail.dataParam.ID = "";
    defineDetail.dataParam.CODE = "";
    defineDetail.dataParam.NAME = "";
    defineDetail.dataParam.ORGID = "";
    defineDetail.dataParam.ORGIDCASCADER = [];
    defineDetail.dataParam.AREA_BUILD = "";
    defineDetail.dataParam.AREA_USABLE = "";
    defineDetail.dataParam.AREA_RENTABLE = "";
    defineDetail.dataParam.BRANCHID = "";
    defineDetail.dataParam.REGIONID = "";
};

defineDetail.mountedInit = function () {
    defineDetail.otherMethods.initBranch();
    defineDetail.btnConfig = [{
        id: "add",
        authority: ""
    }, {
        id: "edit",
        authority: ""
    }, {
        id: "del",
        authority: ""
    }, {
        id: "save",
        authority: ""
    }, {
        id: "abandon",
        authority: ""
    }];
};

defineDetail.otherMethods = {
    orgChange: function (value, selectedData) {
        defineDetail.dataParam.ORGID = value[value.length - 1];
    },
    branchChange: function () {
        defineDetail.otherMethods.initRegion();
    },
    initBranch: function () {
        _.Ajax('GetBranch', {
            Data: { ID: "" }
        }, function (data) {
            let dt = data.dt;
            if (dt && dt.length) {
                defineDetail.screenParam.branchData = [];
                for (var i = 0; i < dt.length; i++) {
                    defineDetail.screenParam.branchData.push({ value: dt[i].ID, label: dt[i].NAME })
                }
            }
        });
    },
    initRegion: function () {
        _.Ajax('GetRegion', {
            Data: { BRANCHID: defineDetail.dataParam.BRANCHID }
        }, function (data) {
            let dt = data.dt;
            if (dt && dt.length) {
                defineDetail.screenParam.regionData = [];
                for (var i = 0; i < dt.length; i++) {
                    defineDetail.screenParam.regionData.push({ value: dt[i].REGIONID, label: dt[i].NAME })
                }
            }
        });
    }
};

defineDetail.showOne = function (data, callback) {
    _.Ajax('GetFloor', {
        Data: { ID: data }
    }, function (data) {
        $.extend(defineDetail.dataParam, data.floorelement);
        if (defineDetail.dataParam.ORGIDCASCADER != null)
            defineDetail.dataParam.ORGIDCASCADER = defineDetail.dataParam.ORGIDCASCADER.toString().split(",");
        defineDetail.otherMethods.initRegion();
        callback && callback();
    });
};

defineDetail.newRecord = function () {
    defineDetail.dataParam.STATUS = 1;
    defineDetail.dataParam.ORGIDCASCADER = [];
}

defineDetail.IsValidSave = function () {
    if (!defineDetail.dataParam.BRANCHID) {
        iview.Message.info("请选择门店!");
        return false;
    };
    if (!defineDetail.dataParam.REGIONID) {
        iview.Message.info("请选择区域!");
        return false;
    };
    if (!defineDetail.dataParam.CODE) {
        iview.Message.info("楼层代码不能为空!");
        return false;
    };
    if (!defineDetail.dataParam.NAME) {
        iview.Message.info("楼层名称不能为空!");
        return false;
    };
    if (!defineDetail.dataParam.ORGIDCASCADER) {
        iview.Message.info("请选择管理机构!");
        return false;
    };
    if (!defineDetail.dataParam.AREA_BUILD) {
        iview.Message.info("建筑面积不能为空!");
        return false;
    };

    return true;
}