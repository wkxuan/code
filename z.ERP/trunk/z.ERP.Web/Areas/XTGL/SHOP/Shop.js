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
    define.service = "XtglService";
    define.method = "GetShopElement";
    define.methodList = "GetShop";
    define.Key = 'SHOPID';
    define.Data = [];
    define.screenParam.componentVisible = false;
    define.screenParam.branchData = [];
    define.searchParam.BRANCHID = 0;
    define.screenParam.floorData = [];
    define.searchParam.FLOORID = 0;
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
    if (define.searchParam.BRANCHID==0) {
        iview.Message.info("请请选择门店!");
        return;
    };
    if (define.searchParam.FLOORID == 0) {
        iview.Message.info("请请选择楼层!");
        return;
    };
    define.dataParam.TYPE = 1;
    define.dataParam.STATUS = 2;
    define.dataParam.RENT_STATUS = 1;
    define.dataParam.BRANCHID = define.searchParam.BRANCHID;
    define.dataParam.FLOORID = define.searchParam.FLOORID;
}
define.otherMethods = {
    branchChange: function (value) {
        define.screenParam.floorData = [];
        define.screenParam.FLOORID = 0;
        _.Ajax('GetFloor', {
            Data: { BRANCHID: value }
        }, function (data) {
            if (data.dt) {
                define.screenParam.floorData = [];
                for (var i = 0; i < data.dt.length;i++){
                    define.screenParam.floorData.push({value:data.dt[i].ID,label:data.dt[i].NAME})
                }
                define.searchParam.FLOORID = data.dt[0].ID;
            }
            else {

            }
        });
    },
    floorChange: function (value) {
        define.showlist();
    }
}