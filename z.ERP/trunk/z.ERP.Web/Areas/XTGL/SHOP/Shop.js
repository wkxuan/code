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
    define.searchParam.BRANCHID = 0;
    define.dataParam.BRANCHID = 0;
    define.screenParam.floorData = [];
    define.searchParam.FLOORID = 0;
    define.dataParam.FLOORID = 0;
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
        }
        else {

        }
    });
}
define.newRecord = function () {
    if (define.searchParam.BRANCHID == 0) {
        iview.Message.info("请请选择门店!");
        return;
    };
    if (define.searchParam.FLOORID == 0) {
        iview.Message.info("请请选择楼层!");
        return;
    };
    define.dataParam.TYPE = 1;
    define.dataParam.STATUS = 1;
    define.dataParam.RENT_STATUS = 1;
    define.dataParam.ORGIDCASCADER = [];
    define.dataParam.BRANCHID = define.searchParam.BRANCHID;
    //define.dataParam.FLOORID = define.searchParam.FLOORID;
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
        _.Ajax('GetFloor', {
            Data: { BRANCHID: value }
        }, function (data) {
            if (data.dt) {
                define.screenParam.floorData = [];
                for (var i = 0; i < data.dt.length; i++) {
                    define.screenParam.floorData.push({ value: data.dt[i].ID, label: data.dt[i].NAME })
                }
                define.dataParam.FLOORID = data.dt[0].ID;
            }
            else {

            }
        });
    },
    floorChange: function (value) {
        if (define.dataParam.FLOORID == 0) {
            define.searchParam.FLOORID = "";
        }
        else {
            define.searchParam.FLOORID = define.dataParam.FLOORID;
        }
        if (define.myve.disabled)
        {
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
            define.dataParam.ORGIDCASCADER = define.dataParam.ORGIDCASCADER.split(",");
        if (define.dataParam.CATEGORYIDCASCADER != null)
            define.dataParam.CATEGORYIDCASCADER = define.dataParam.CATEGORYIDCASCADER.split(",");
        if (define.dataParam.STATUS == 2) {
            define.myve.topbtnModVisible = define.isvisible(false);
            define.myve.topbtnChkVisible = define.isvisible(false);
        }
        else
        {
            define.myve.topbtnModVisible = define.isvisible(true);
            define.myve.topbtnChkVisible = define.isvisible(true);
        }
        callback && callback();
    });
}