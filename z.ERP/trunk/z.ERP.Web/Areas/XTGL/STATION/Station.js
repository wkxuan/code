define.beforeVue = function () {
    define.dataParam.IP = "";
    define.screenParam.colDef = [
    { title: "POS终端号", key: 'STATIONBH'},
    { title: "IP地址", key: 'IP' },
    { title: "门店", key: 'BRANCHNAME' }
    ];

    define.screenParam.payColDef = [
       { type: 'selection', width: 60, align: 'center' },
       { title: '代码', key: 'PAYID', width: 80 },
       { title: '名称', key: 'NAME', width: 160 }
    ];
    define.dataParam.STATION_PAY = [];
    define.screenParam.dataDef = [];
    define.screenParam.STATION_PAY_DATA = [];
    define.service = "XtglService";
    define.method = "GetStaionElement";
    define.methodList = "GetStaion";
    define.Key = "STATIONBH";

    define.screenParam.showPopShop = false;
    define.screenParam.srcPopShop = __BaseUrl + "/" + "Pop/Pop/PopShopList/";
    define.screenParam.popParam = {};

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


    //以下三个表格事件
    define.screenParam.selectData = function (selection, row) {
        define.checkSTATION_PAY(selection);
    };

    define.screenParam.selectDataAll = function (selection) {
        define.checkSTATION_PAY(selection);
    };
    define.screenParam.selectCancel = function (selection) {
        define.checkSTATION_PAY(selection);
    };

    define.checkSTATION_PAY = function (selection) {
        define.dataParam.STATION_PAY = [];
        let localData = [];
        for (var i = 0; i < selection.length; i++) {
            localData.push({ PAYID: selection[i].PAYID });
        };
        Vue.set(define.dataParam, 'STATION_PAY', localData);
    }

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
}

define.showone = function (data, callback) {
    _.Ajax('SearchStation', {
        Data: { STATIONBH: data }
    }, function (data) {
        $.extend(define.dataParam, data.Pay);
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
           // Vue.set(define.dataParam, 'STATION', localData)
        };
        callback && callback();
    });
}

define.otherMethods = {
    branchChange: function (value) {
        define.dataParam.BRANCHID = define.searchParam.BRANCHID;
        define.showlist();
    },
    SelShop: function () {
        if (!define.dataParam.BRANCHID) {
            iview.Message.info("门店不能为空!");
            exit;
        }        
        define.screenParam.showPopShop = true;
        define.screenParam.popParam = { BRANCHID: define.dataParam.BRANCHID };
    },
    atoA: function () {
        define.dataParam.NETWORK_NODE_ADDRESS = define.dataParam.NETWORK_NODE_ADDRESS.toUpperCase()
    }
}

//接收子页面返回值
define.popCallBack = function (data) {
    if (define.screenParam.showPopShop) {
       define.screenParam.showPopShop = false;
        for (var i = 0; i < data.sj.length; i++) {
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
    if (!define.dataParam.IP) {
        iview.Message.info("IP地址不能为空!");
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

    return true;
};