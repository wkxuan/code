define.beforeVue = function () {

    define.screenParam.colDef = [
        {
            title: '编码',
            key: 'ID', width: 120
        },
        {
            title: "名称",
            key: 'NAME', width: 130
        }, {
            title: "标志坐标",
            key: 'IMAGEPOINTS', tooltip: true
        }];
    define.screenParam.dataDef = [];
    define.service = "DpglService";
    define.method = "SearchMAPPUBLICDATA";
    define.methodList = "SearchMAPPUBLICDATA";
    define.Key = 'ID';
    define.screenParam.branchData = [];
    define.screenParam.regionData = [];
    define.screenParam.floorData = [];
    define.dataParam.PUBLICDATAID = "";
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
            }

            _.Ajax('GetFloor', {
                Data: { REGIONID: define.dataParam.REGIONID }
            }, function (data) {
                if (data.dt) {
                    define.screenParam.floorData = [];
                    define.dataParam.FLOORID = 0;
                    define.searchParam.FLOORID = 0;
                    for (var i = 0; i < data.dt.length; i++) {
                        define.screenParam.floorData.push({ value: data.dt[i].ID, label: data.dt[i].NAME })
                    }
                    define.dataParam.FLOORID = data.dt[0].ID;
                    define.searchParam.FLOORID = data.dt[0].ID;
                    define.showlist();
                }
            });
        });
    });

}
define.newRecord = function () {
    define.dataParam.BRANCHID = define.searchParam.BRANCHID;
    define.dataParam.REGIONID = define.searchParam.REGIONID;
    define.dataParam.FLOORID = define.searchParam.FLOORID;
    define.dataParam.ID = null;
    define.dataParam.PUBLICDATAID = null;
    define.dataParam.IMAGEPOINTS = null;
    define.dataParam.POINTS = null;
    if (!define.dataParam.BRANCHID) {
        iview.Message.info("请选择门店!");
        return;
    }
    if (!define.dataParam.REGIONID) {
        iview.Message.info("请选择区域!");
        return;
    }
    if (!define.dataParam.FLOORID) {
        iview.Message.info("请选择楼层!");
        return;
    };

}
//define.mountedInit = function () {
//    define.otherMethods.initBranch();
//};
define.otherMethods = {
    branchChange: function () {
        define.otherMethods.initRegion();
        define.searchParam.REGIONID = undefined;
        define.searchParam.FLOORID = undefined;
    },
    regionChange: function () {
        define.otherMethods.initFloor();
        define.searchParam.FLOORID = undefined;
    },
    floorChange: function () {
        define.showlist();
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
    if (!define.dataParam.FLOORID) {
        iview.Message.info("请选择楼层!");
        return false;
    };
    if (!define.dataParam.PUBLICDATAID) {
        iview.Message.info("请选择模型类型!");
        return false;
    };
    if (!define.dataParam.IMAGEPOINTS) {
        iview.Message.info("请填写图标定位坐标!");
        return false;
    };
    if (!define.dataParam.POINTS) {
        iview.Message.info("请填写定位坐标!");
        return false;
    };

    return true;
}