defineDetail.beforeVue = function () {
    defineDetail.Key = "SHOPID";
    defineDetail.screenParam.branchData = [];
    defineDetail.screenParam.regionData = [];
    defineDetail.screenParam.floorData = [];
};

defineDetail.clearKey = function () {
    defineDetail.dataParam.STATUS = 1;
    defineDetail.dataParam.RENT_STATUS = 1;
    defineDetail.dataParam.SHOPID = null;
    defineDetail.dataParam.CODE = null;
    defineDetail.dataParam.NAME = null;
    defineDetail.dataParam.ORGIDCASCADER = [];
    defineDetail.dataParam.CATEGORYIDCASCADER = [];
    defineDetail.dataParam.TYPE = null;
    defineDetail.dataParam.AREA_BUILD = null;
    defineDetail.dataParam.AREA_USABLE = null;
    defineDetail.dataParam.AREA_RENTABLE = null;
    defineDetail.dataParam.AREA_STATUS = null;
    defineDetail.dataParam.BRANCHID = null;
    defineDetail.dataParam.REGIONID = null;
    defineDetail.dataParam.FLOORID = null;
};

defineDetail.mountedInit = function () {
    _.Ajax('SearchInit', {
        Data: {}
    }, function (data) {
        Vue.set(defineDetail.screenParam, "ORGData", data.treeOrg.Obj);
        Vue.set(defineDetail.screenParam, "CATEGORYData", data.treeCategory.Obj);
    });
    defineDetail.otherMethods.initBranch();

    defineDetail.btnConfig = [{
        id: "add",
        authority: "104004"
    }, {
        id: "edit",
        authority: "104004"
    }, {
        id: "del",
        authority: "104004"
    }, {
        id: "save",
        authority: "104004"
    }, {
        id: "abandon",
        authority: "104004"
    }];
};

defineDetail.otherMethods = {
    orgChange: function (value, selectedData) {
        defineDetail.dataParam.ORGID = value[value.length - 1];
    },
    categoryChange: function (value, selectedData) {
        defineDetail.dataParam.CATEGORYID = value[value.length - 1];
    },
    branchChange: function () {
        defineDetail.otherMethods.initRegion();
    },
    regionChange: function () {
        defineDetail.otherMethods.initFloor();
    },
    floorChange: function () {},
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
    },
    initFloor: function () {
        _.Ajax('GetFloor', {
            Data: { REGIONID: defineDetail.dataParam.REGIONID }
        }, function (data) {
            let dt = data.dt;
            if (dt && dt.length) {
                defineDetail.screenParam.floorData = [];
                for (var i = 0; i < dt.length; i++) {
                    defineDetail.screenParam.floorData.push({ value: dt[i].ID, label: dt[i].NAME })
                }
            }
        });
    },
};

defineDetail.showOne = function (data, callback) {
    _.Ajax('GetShop', {
        Data: { SHOPID: data }
    }, function (data) {
        $.extend(defineDetail.dataParam, data.shopelement);
        if (defineDetail.dataParam.ORGIDCASCADER != null)
            defineDetail.dataParam.ORGIDCASCADER = defineDetail.dataParam.ORGIDCASCADER.toString().split(",");
        if (defineDetail.dataParam.CATEGORYIDCASCADER != null)
            defineDetail.dataParam.CATEGORYIDCASCADER = defineDetail.dataParam.CATEGORYIDCASCADER.toString().split(",");
        defineDetail.otherMethods.initRegion();
        defineDetail.otherMethods.initFloor();
        callback && callback();
    });
};

defineDetail.newRecord = function () {
    defineDetail.dataParam.STATUS = 1;
    defineDetail.dataParam.RENT_STATUS = 1;
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
    if (!defineDetail.dataParam.FLOORID) {
        iview.Message.info("请选择楼层!");
        return false;
    };
    if (!defineDetail.dataParam.CODE) {
        iview.Message.info("单元代码不能为空!");
        return false;
    };
    if (!defineDetail.dataParam.NAME) {
        iview.Message.info("单元名称不能为空!");
        return false;
    };
    if (!defineDetail.dataParam.TYPE) {
        iview.Message.info("请选择单元类型!");
        return false;
    };
    if (!defineDetail.dataParam.ORGID) {
        iview.Message.info("请选择管理机构!");
        return false;
    };
    if (!defineDetail.dataParam.CATEGORYIDCASCADER) {
        iview.Message.info("请选择业态!");
        return false;
    };
    if (!defineDetail.dataParam.AREA_RENTABLE) {
        iview.Message.info("租赁面积不能为空!");
        return false;
    };

    return true;
}