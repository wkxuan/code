define.beforeVue = function () {
    define.screenParam.colDef = [
        {
            title: '楼层编码',
            key: 'FLOORID',width:150
        },
        {
            title: "坐标定位",
            key: 'POINTS', tooltip: true
        }];
    define.screenParam.dataDef = [];
    define.service = "DpglService";
    define.method = "SearchMAPFLOORDATA";
    define.methodList = "SearchMAPFLOORDATA";
    define.Key = 'FLOORID';
    define.screenParam.branchData = [];
    define.screenParam.regionData = [];
    define.screenParam.floorData = [];
    _.Ajax('GetBranch', {
        Data: { ID: "" }
    }, function (data) {
        if (data.dt) {
            define.screenParam.branchData = [];
            for (var i = 0; i < data.dt.length; i++) {
                define.screenParam.branchData.push({ value: data.dt[i].ID, label: data.dt[i].NAME })
            }
            define.searchParam.BRANCHID = data.dt[0].ID;
            define.dataParam.BRANCHID = data.dt[0].ID;
        }
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
                define.searchParam.REGIONID = data.dt[0].REGIONID;
                define.showlist();
            }
        });
    });
}
define.newRecord = function () {
    define.dataParam.BRANCHID = define.searchParam.BRANCHID;
    define.dataParam.REGIONID = define.searchParam.REGIONID;
    if (!define.dataParam.BRANCHID) {
        iview.Message.info("请选择门店!");
        return;
    }
    if (!define.dataParam.REGIONID) {
        iview.Message.info("请选择区域!");
        return;
    }
    define.dataParam.FLOORID = "";
    define.dataParam.POINTS = "";

}
define.otherMethods = {
    branchChange: function () {
        define.otherMethods.initRegion();
        define.searchParam.REGIONID = undefined;
        define.dataParam.FLOORID = undefined;
    },
    regionChange: function () {
        define.showlist();
        define.otherMethods.initFloor();
        define.dataParam.FLOORID = undefined;
    },
    floorChange: function (event) {
        _.Ajax('GetFloorMD', {
            Data: { BRANCHID: define.dataParam.BRANCHID, REGIONID: define.dataParam.REGIONID, FLOORID: event }
        }, function (data) {
            if (data.Item1) {
                iview.Message.info("该楼层已有数据，将启用编辑!");
                define.dataParam.POINTS = data.Item1.POINTS;
            } else {
                define.dataParam.POINTS = "";
            }
        });
    },
    initBranch: function () {
        _.Ajax('GetBranch', {
            Data: { ID: "" }
        }, function (data) {
            let dt = data.dt;
            if (dt && dt.length) {
                define.screenParam.branchData = [];
                for (var i = 0; i < dt.length; i++) {
                    define.screenParam.branchData.push({ value: dt[i].ID, label: dt[i].NAME })
                }
            }
        });
    },
    initRegion: function () {
        _.Ajax('GetRegion', {
            Data: { BRANCHID: define.searchParam.BRANCHID }
        }, function (data) {
            let dt = data.dt;
            if (dt && dt.length) {
                define.screenParam.regionData = [];
                for (var i = 0; i < dt.length; i++) {
                    define.screenParam.regionData.push({ value: dt[i].REGIONID, label: dt[i].NAME })
                }
            }
        });
    },
    initFloor: function () {
        _.Ajax('GetFloor', {
            Data: { REGIONID: define.searchParam.REGIONID }
        }, function (data) {
            let dt = data.dt;
            if (dt && dt.length) {
                define.screenParam.floorData = [];
                for (var i = 0; i < dt.length; i++) {
                    define.screenParam.floorData.push({ value: dt[i].ID, label: dt[i].NAME })
                }
            }
        });
    },
}

define.IsValidSave = function () {
    define.dataParam.BRANCHID = define.searchParam.BRANCHID;
    define.dataParam.REGIONID = define.searchParam.REGIONID;
    if (!define.dataParam.BRANCHID) {
        iview.Message.info("请选择门店!");
        return false;
    };
    if (!define.dataParam.REGIONID) {
        iview.Message.info("请选择区域!");
        return false;
    };
    if (!define.dataParam.FLOORID) {
        iview.Message.info("请选择楼层!");
        return false;
    };
    if (!define.dataParam.POINTS) {
        iview.Message.info("请填写坐标定位!");
        return false;
    };
    return true;
}