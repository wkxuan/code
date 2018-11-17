define.beforeVue = function () {

    define.screenParam.colDef = [
        {
            title: "代码",
            key: 'CODE', width: 100
        },
        {
            title: '名称',
            key: 'NAME', width: 200
        }];
    define.screenParam.dataDef = [];
    define.service = "DpglService";
    define.method = "SearchFloor";
    define.methodList = "SearchFloor";
    define.Key = 'ID';
    define.Data = [];
    define.screenParam.componentVisible = false;
    define.screenParam.branchData = [];
    define.screenParam.regionData = [];
    define.dataParam.ORGIDCASCADER = [];
    define.btnChkvisible = true;

    _.Ajax('GetBranch', {
        Data: { ID: "" }
    }, function (data) {
        if (data.dt) {
            define.screenParam.branchData = [];
            for (var i = 0; i < data.dt.length; i++) {
                define.screenParam.branchData.push({ value: data.dt[i].ID, label: data.dt[i].NAME })
            }
            define.searchParam.BRANCHID = data.dt[0].ID;
            define.dataParam.BRANCHID = define.searchParam.BRANCHID;
        }
        else {

        }
    });
    _.Ajax('GetRegion', {
        Data: { BRANCHID: define.dataParam.BRANCHID }
    }, function (data) {
        if (data.dt) {
            define.screenParam.regionData = [];
            define.dataParam.REGIONID = 0;
            define.searchParam.REGIONID = 0;
            for (var i = 0; i < data.dt.length; i++) {
                define.screenParam.regionData.push({ value: data.dt[i].REGIONID, label: data.dt[i].NAME })
            }
            define.dataParam.REGIONID = data.dt[0].REGIONID;
            define.searchParam.REGIONID = define.dataParam.REGIONID;
            define.showlist();
        }
        else {

        }
    });
}


define.newRecord = function () {
    if (define.searchParam.BRANCHID == 0) {
        iview.Message.info("请选择门店!");
     //   return;
    };
    if (define.searchParam.GEGIONID == 0) {
        iview.Message.info("请选择区域!");
     //   return;
    };
    define.dataParam.STATUS = 1;
    define.dataParam.ORGIDCASCADER = [];
    define.dataParam.BRANCHID = define.searchParam.BRANCHID;
    define.dataParam.REGIONID = define.searchParam.REGIONID;

    return true;
}

define.otherMethods = {
    branchChange: function (value) {
       // define.screenParam.regionData = [];
        define.screenParam.REGIONID = 0;
        define.dataParam.FLOORID = "";
        define.dataParam.CODE = "";
        define.dataParam.NAME = "";
        define.dataParam.ORGIDCASCADER = [];
        define.dataParam.AREA_BUILD = "";
        define.dataParam.AREA_USABLE = "";
        define.dataParam.AREA_RENTABLE = "";
        define.dataParam.STATUS = "";
        _.Ajax('GetRegion', {
            Data: { BRANCHID: value }
        }, function (Data) {
            if (Data.dt) {
                define.screenParam.regionData = [];
                define.dataParam.REGIONID = 0;
                define.searchParam.REGIONID = 0;
                for (var i = 0; i < Data.dt.length; i++) {
                    define.screenParam.regionData.push({ value: Data.dt[i].REGIONID, label: Data.dt[i].NAME })
                }
                define.dataParam.REGIONID = Data.dt[0].RGIONID;
                define.searchParam.REGIONID = define.dataParam.RGIONID;
                define.showlist();
            }
            else {

            }
        });
    },

    regionChange: function (value) {
        if (define.dataParam.REGIONID == 0) {
            define.searchParam.REGIONID = "";
        }
        else {
            define.searchParam.REGIONID = define.dataParam.REGIONID;
        }
        if (define.myve.disabled) {
            define.dataParam.FLOORID = "";
            define.dataParam.CODE = "";
            define.dataParam.NAME = "";
            define.dataParam.ORGIDCASCADER = [];
            define.dataParam.AREA_BUILD = "";
            define.dataParam.AREA_USABLE = "";
            define.dataParam.AREA_RENTABLE = "";
            define.dataParam.STATUS = "";
            define.showlist();
        }
    },

    orgChange: function (value, selectedData) {
        define.dataParam.ORGID = value[value.length - 1];
    },
    kindChange: function (value, selectedData) {
        editDetail.dataParam.KINDID = value[value.length - 1];
    },
    clear: function () {

    }
}

define.mountedInit = function () {
    _.Ajax('SearchInit', {
        Data: {}
    }, function (data) {
        Vue.set(define.screenParam, "ORGData", data.treeOrg.Obj);
    });
}

define.showone = function (data, callback) {
    _.Ajax('GetFloor', {
        Data: { ID: data }
    }, function (data) {
        $.extend(define.dataParam, data.floorelement);
        define.dataParam.ORGIDCASCADER = define.dataParam.ORGIDCASCADER.split(",");
        callback && callback();
    });
}

define.IsValidSave = function () {
    if (define.dataParam.BRANCHID == 0) {
        iview.Message.info("请选择门店!");
        return false;
    };
    if (define.dataParam.REGIONID == 0) {
        iview.Message.info("请选择区域!");
        return false;
    };
    if (!define.dataParam.CODE) {
        iview.Message.info("楼层代码不能为空!");
        return false;
    };
    if (!define.dataParam.NAME) {
        iview.Message.info("楼层名称不能为空!");
        return false;
    };
    if (!define.dataParam.ORGIDCASCADER) {
        iview.Message.info("请选择管理机构!");
        return false;
    };
    if (!define.dataParam.AREA_BUILD) {
        iview.Message.info("建筑面积不能为空!");
        return false;
    };

    return true;
}