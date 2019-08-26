﻿define.beforeVue = function () {

    define.screenParam.colDef = [
        {
            title: '单元编码',
            key: 'SHOPID',width:120
        },
        {
            title: "代码",
            key: 'CODE', width: 130
        }, {
            title: "标题坐标",
            key: 'TITLEPOINTS', tooltip:true
        }];
    define.screenParam.dataDef = [];
    define.service = "DpglService";
    define.method = "SearchMAPSHOPDATA";
    define.methodList = "SearchMAPSHOPDATA";
    define.Key = 'SHOPID';
    define.screenParam.branchData = [];
    define.screenParam.regionData = [];
    define.screenParam.floorData = [];
    define.screenParam.showPopShop = false;
    define.screenParam.srcPopShop = __BaseUrl + "/" + "Pop/Pop/PopShopList/";
    define.screenParam.popParam = {};
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
    define.dataParam.SHOPID = "";
    define.dataParam.TITLEPOINTS = "";
    define.dataParam.POINTS = "";

}
//define.mountedInit = function () {
//    define.otherMethods.initBranch();
//};
define.otherMethods = {
    SelShop: function () {
        if (!define.dataParam.BRANCHID) {
            iview.Message.info("请选择门店!");
            return;
        } else if (!define.dataParam.REGIONID) {
            iview.Message.info("请选择区域!");
            return;
        }else if (!define.dataParam.FLOORID) {
            iview.Message.info("请选择楼层!");
            return;
        }; {
            define.screenParam.showPopShop = true;
            define.screenParam.popParam = {
                BRANCHID: define.dataParam.BRANCHID, REGIONID: define.dataParam.REGIONID,
                FLOORID: define.dataParam.FLOORID,
                SqlCondition: " not exists(select 1 from MAPSHOPDATA  where MAPSHOPDATA.SHOPID=A.SHOPID)"
            };
        }
    },
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

//接收子页面返回值
define.popCallBack = function (data) {
    if (define.screenParam.showPopShop) {
        define.screenParam.showPopShop = false;
        for (var i = 0; i < data.sj.length; i++) {
            define.dataParam.SHOPID = data.sj[i].SHOPID;
        }
    }
};


define.IsValidSave = function () {
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
    if (!define.dataParam.SHOPID) {
        iview.Message.info("请选择单元!");
        return false;
    };
    if (!define.dataParam.TITLEPOINTS) {
        iview.Message.info("请填写标题定位坐标!");
        return false;
    };
    if (!define.dataParam.POINTS) {
        iview.Message.info("请填写定位坐标!");
        return false;
    };

    return true;
}