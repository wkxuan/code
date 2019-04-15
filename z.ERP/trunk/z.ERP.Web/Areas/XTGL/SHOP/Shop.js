define.beforeVue = function () {

    define.screenParam.colDef = [
        {
            title: "代码",
            key: 'CODE', width: 150
        },
        {
            title: '名称',
            key: 'NAME', width: 200
        }];
    define.screenParam.dataDef = [];
    define.service = "DpglService";
    define.method = "SearchShop";
    define.methodList = "SearchShop";
    define.Key = 'SHOPID';
    define.Data = [];
    define.screenParam.componentVisible = false;
    define.screenParam.branchData = [];
    define.screenParam.regionData = [];
    define.screenParam.floorData = [];
    define.dataParam.ORGIDCASCADER = [];
    define.btnChkvisible = false;

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
            define.searchParam.REGIONID = data.dt[0].REGIONID;
        }
        else {

        }
    });
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
        else {

        }
    });
}
define.newRecord = function () {
    if (define.searchParam.BRANCHID == 0) {
        iview.Message.info("请选择门店!");
        //   return;
    };
    if (define.searchParam.REGIONID == 0) {
        iview.Message.info("请选择区域!");
        //  return;
    };
    if (define.searchParam.FLOORID == 0) {
        iview.Message.info("请选择楼层!");
        //  return;
    };

    //  define.dataParam.TYPE = 1;
    define.dataParam.STATUS = 1;
    define.dataParam.RENT_STATUS = 1;
    define.dataParam.ORGIDCASCADER = [];
    define.dataParam.BRANCHID = define.searchParam.BRANCHID;
    define.dataParam.REGIONID = define.searchParam.REGIONID;
    define.dataParam.FLOORID = define.searchParam.FLOORID;

    return true;
}
define.otherMethods = {
    branchChange: function (value) {
        //define.clear();
        define.screenParam.floorData = [];
        define.screenParam.FLOORID = 0;
        define.dataParam.SHOPID = "";
        define.dataParam.CODE = "";
        define.dataParam.NAME = "";
        define.dataParam.ORGIDCASCADER = [];
        define.dataParam.CATEGORYIDCASCADER = [];
        define.dataParam.TYPE = "";
        define.dataParam.AREA_BUILD = "";
        define.dataParam.AREA_USABLE = "";
        define.dataParam.AREA_RENTABLE = "";
        define.dataParam.AREA_STATUS = "";
        define.dataParam.RENT_STATUS = "";
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
                define.searchParam.REGIONID = Data.dt[0].RGIONID;
            }
            else {

            }
        });
        _.Ajax('GetFloor', {
            Data: { REGIONID: value }
        }, function (Data) {
            if (Data.dt) {
                define.screenParam.floorData = [];
                define.dataParam.FLOORID = 0;
                define.searchParam.FLOORID = 0;
                for (var i = 0; i < Data.dt.length; i++) {
                    define.screenParam.floorData.push({ value: Data.dt[i].ID, label: Data.dt[i].NAME })
                }
                define.dataParam.FLOORID = Data.dt[0].ID;
                define.searchParam.FLOORID = Data.dt[0].ID;
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
            define.dataParam.SHOPID = "";
            define.dataParam.CODE = "";
            define.dataParam.NAME = "";
            define.dataParam.ORGIDCASCADER = [];
            define.dataParam.CATEGORYIDCASCADER = [];
            define.dataParam.TYPE = "";
            define.dataParam.AREA_BUILD = "";
            define.dataParam.AREA_USABLE = "";
            define.dataParam.AREA_RENTABLE = "";
            define.dataParam.AREA_STATUS = "";
            define.dataParam.RENT_STATUS = "";
            _.Ajax('GetFloor', {
                Data: { REGIONID: value }
            }, function (Data) {
                if (Data.dt) {
                    define.screenParam.floorData = [];
                    define.dataParam.FLOORID = 0;
                    define.searchParam.FLOORID = 0;
                    for (var i = 0; i < Data.dt.length; i++) {
                        define.screenParam.floorData.push({ value: Data.dt[i].ID, label: Data.dt[i].NAME })
                    }
                    define.dataParam.FLOORID = Data.dt[0].ID;
                    define.searchParam.FLOORID = Data.dt[0].ID;
                    define.showlist();
                }
                else {

                }
            });
        }
    },
    floorChange: function (value) {
        if (define.dataParam.FLOORID == 0) {
            define.searchParam.FLOORID = "";
        }
        else {
            define.searchParam.FLOORID = define.dataParam.FLOORID;
        }
        if (define.myve.disabled) {
            define.dataParam.SHOPID = "";
            define.dataParam.CODE = "";
            define.dataParam.NAME = "";
            define.dataParam.ORGIDCASCADER = [];
            define.dataParam.CATEGORYIDCASCADER = [];
            define.dataParam.TYPE = "";
            define.dataParam.AREA_BUILD = "";
            define.dataParam.AREA_USABLE = "";
            define.dataParam.AREA_RENTABLE = "";
            define.dataParam.AREA_STATUS = "";
            define.dataParam.RENT_STATUS = "";
            define.showlist();
        }
    },
    orgChange: function (value, selectedData) {
        define.dataParam.ORGID = value[value.length - 1];
    },
    categoryChange: function (value, selectedData) {
        define.dataParam.CATEGORYID = value[value.length - 1];
    },
    clear: function () {

    }
}
define.mountedInit = function () {
    _.Ajax('SearchInit', {
        Data: {}
    }, function (data) {
        Vue.set(define.screenParam, "ORGData", data.treeOrg.Obj);
        Vue.set(define.screenParam, "CATEGORYData", data.treeCategory.Obj);
    });
}
define.showone = function (data, callback) {
    _.Ajax('GetShop', {
        Data: { SHOPID: data }
    }, function (data) {
        $.extend(define.dataParam, data.shopelement);
        if (define.dataParam.ORGIDCASCADER != null)
            define.dataParam.ORGIDCASCADER = define.dataParam.ORGIDCASCADER.toString().split(",");
        if (define.dataParam.CATEGORYIDCASCADER != null)
            define.dataParam.CATEGORYIDCASCADER = define.dataParam.CATEGORYIDCASCADER.toString().split(",");



        //if (define.dataParam.STATUS == 2) {
        //    define.myve.topbtnModVisible = define.isvisible(true);
        //    define.myve.topbtnChkVisible = define.isvisible(true);
        //}
        //else {
        //    define.myve.topbtnModVisible = define.isvisible(true);
        //    define.myve.topbtnChkVisible = define.isvisible(true);
        //}

        define.myve.topbtnModVisible = define.isvisible(true);
        define.myve.topbtnChkVisible = define.isvisible(false);
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
    if (define.dataParam.FLOORID == 0) {
        iview.Message.info("请选择楼层!");
        return false;
    };
    if (!define.dataParam.CODE) {
        iview.Message.info("单元代码不能为空!");
        return false;
    };
    if (!define.dataParam.NAME) {
        iview.Message.info("单元名称不能为空!");
        return false;
    };
    if (!define.dataParam.TYPE) {
        iview.Message.info("请选择单元类型!");
        return false;
    };
    if (!define.dataParam.ORGID) {
        iview.Message.info("请选择管理机构!");
        return false;
    };
    if (!define.dataParam.CATEGORYIDCASCADER) {
        iview.Message.info("请选择业态!");
        return false;
    };
    if (!define.dataParam.AREA_RENTABLE) {
        iview.Message.info("租赁面积不能为空!");
        return false;
    };

    return true;
}