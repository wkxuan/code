define.beforeVue = function () {
    define.service = "XtglService";
    define.method = "GetStaion";
    define.methodList = "GetStaion";
    define.Key = "STATIONBH";

    define.screenParam.colDef = [
    { title: "POS终端号", key: 'STATIONBH' },
    { title: "IP地址", key: 'IP' },
    { title: "门店", key: 'BRANCHNAME' }
    ];

    define.screenParam.payColDef = [
       { type: 'selection', width: 60, align: 'center' },
       { title: '代码', key: 'PAYID', width: 80 },
       { title: '名称', key: 'NAME' }
    ];
}

define.initDataParam = function () {
    define.dataParam.IP = null;
    define.dataParam.SHOPID = null;
    define.dataParam.SHOPCODE = null;
    define.dataParam.TYPE = null;
    define.dataParam.NETWORK_NODE_ADDRESS = null;
    define.dataParam.STATIONBH = null;
    define.dataParam.STATION_PAY = [];
    define.screenParam.STATION_PAY_DATA = [];
}

define.newRecord = function () {
    for (var j = 0; j < define.screenParam.STATION_PAY_DATA.length; j++) {
        Vue.set(define.screenParam.STATION_PAY_DATA[j], '_checked', false);
    }
}

define.mountedInit = function () {
    _.Ajax('GetBranch', {
        Data: { ID: "" }
    }, function (data) {
        if (data.dt) {
            define.screenParam.branchData = [];
            for (var i = 0; i < data.dt.length; i++) {
                define.screenParam.branchData.push({ value: data.dt[i].ID, label: data.dt[i].NAME })
            }
            define.searchParam.BRANCHID = data.dt[0].ID;
            Vue.set(define.dataParam, "BRANCHID", define.searchParam.BRANCHID);
            define.showlist();
        }
    });

    _.Ajax('GetStaionPayList', {
    }, function (data) {
        for (var i = 0; i < data.pay.length; i++) {
            define.screenParam.STATION_PAY_DATA.push({
                _checked: false,
                PAYID: data.pay[i]["PAYID"],
                NAME: data.pay[i]["NAME"],
            });
        }
    });
}

define.showOne = function (data, callback) {
    _.Ajax('SearchStation', {
        Data: { STATIONBH: data }
    }, function (data) {
        $.extend(define.dataParam, data.Station);
        let localData = [];
        for (var j = 0; j < define.screenParam.STATION_PAY_DATA.length; j++) {
            Vue.set(define.screenParam.STATION_PAY_DATA[j], '_checked', false);
            for (var i = 0; i < data.Pay.length; i++) {
                if (data.Pay[i].PAYID == define.screenParam.STATION_PAY_DATA[j].PAYID) {
                    Vue.set(define.screenParam.STATION_PAY_DATA[j], '_checked', true);
                    localData.push({ PAYID: data.Pay[i].PAYID });
                };
            };
            //赋值防止左边列表变化后直接保存
            Vue.set(define.dataParam, 'STATION_PAY', localData);
        };
        callback && callback();
    });
}

define.cancelAfter = function () {
    define.showOne(define.backData.STATIONBH);
}
define.otherMethods = {
    branchChange: function (value) {
        define.dataParam.BRANCHID = define.searchParam.BRANCHID;
        define.showlist();
    },
    SelShop: function () {
        define.dataParam.BRANCHID = define.searchParam.BRANCHID;
        if (!define.dataParam.BRANCHID) {
            iview.Message.info("门店不能为空!");
            return;
        }
        define.screenParam.popParam = { BRANCHID: define.dataParam.BRANCHID };
        define.popConfig.title = "选择店铺";
        define.popConfig.src = __BaseUrl + "/Pop/Pop/PopShopList/";
        define.popConfig.open = true;
    },
    mackeyup: function () {
        define.dataParam.NETWORK_NODE_ADDRESS = define.dataParam.NETWORK_NODE_ADDRESS.toUpperCase()
    }
}

//接收子页面返回值
define.popCallBack = function (data) {
    define.popConfig.open = false;
    if (define.popConfig.title == "选择店铺") {
        for (let i = 0; i < data.sj.length; i++) {
            define.dataParam.SHOPID = data.sj[i].SHOPID;
            define.dataParam.SHOPCODE = data.sj[i].SHOPCODE;
        };
    }
};

define.IsValidSave = function () {
    define.dataParam.BRANCHID = define.searchParam.BRANCHID;
    if (!define.dataParam.BRANCHID) {
        iview.Message.info("门店不能为空!");
        return false;
    }
    if (!define.dataParam.STATIONBH) {
        iview.Message.info("终端号不能为空!");
        return false;
    }
    if (!define.dataParam.TYPE) {
        iview.Message.info("类型不能为空!");
        return false;
    }
    if (define.dataParam.TYPE == "2" && !define.dataParam.SHOPID) {
        iview.Message.info("类型为店铺时，店铺不能为空!");
        return false;
    };
    define.dataParam.STATION_PAY = [];
    let list = define.vueObj.$refs.payRef.getSelection();
    if (list) {
        for (var i = 0; i < list.length; i++) {
            define.dataParam.STATION_PAY.push({ PAYID: list[i].PAYID });
        };
    };
    return true;
};