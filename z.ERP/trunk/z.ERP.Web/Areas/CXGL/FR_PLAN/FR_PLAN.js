define.beforeVue = function () {

    define.screenParam.colDef = [
        {
            title: '编码',
            key: 'ID', width: 80
        }, {
            title: "方案名称",
            key: 'NAME', tooltip: true
        },
        {
            title: "满减方式",
            key: 'FRTYPEMC', width: 120
        },
        {
            title: "状态",
            key: 'STATUSMC', width: 100
        }];
    define.screenParam.dataDef = [];
    define.screenParam.itemDef = [
        { title: '序号', key: 'INX', width: 100 },
        { title: '满额', key: 'FULL', width: 200, cellType: "input", cellDataType: "number", },
        { title: '减额', key: 'CUT', width: 200, cellType: "input", cellDataType: "number", },
    ]
    define.service = "CxglService";
    define.method = "GetFRPLAN";
    define.methodList = "GetFRPLAN";
    define.Key = 'ID';
}
define.initDataParam = function () {
    define.dataParam.ID = "";
    define.dataParam.NAME = "";
    define.dataParam.STATUSMC = "";
    define.dataParam.FRTYPE = "";
    define.dataParam.LIMIT = "";
    define.dataParam.FR_PLAN_ITEM = [];
}

define.newRecord = function () {
    define.dataParam.ID = "";
    define.dataParam.NAME = "";
    define.dataParam.STATUSMC = "未使用";
    define.dataParam.FRTYPE = "";
    define.dataParam.LIMIT = "";
    define.dataParam.FR_PLAN_ITEM = [];
}

define.otherMethods = {
    addItem: function () {
        define.dataParam.FR_PLAN_ITEM.push({
            INX: define.dataParam.FR_PLAN_ITEM.length+1,
            FULL: "",
            CUT: "",
        });
    },
    delItem: function () {
        var selectton = this.$refs.refGroup.getSelection();
        if (selectton.length == 0) {
            iview.Message.info("请选中要删除的数据!");
        } else {
            for (var i = 0; i < selectton.length; i++) {
                for (var j = 0; j < define.dataParam.FR_PLAN_ITEM.length; j++) {
                    if (define.dataParam.FR_PLAN_ITEM[j].INX == selectton[i].INX) {
                        define.dataParam.FR_PLAN_ITEM.splice(j, 1);
                    }
                }
            }
            for (var i = 0; i < define.dataParam.FR_PLAN_ITEM.length; i++) {
                define.dataParam.FR_PLAN_ITEM[i].INX = i+1;
            }
        }
    }
}

define.showOne = function (data, callback) {
    _.Ajax('SearchFRPLANInfo', {
        Data: { ID: data }
    }, function (data) {
        $.extend(define.dataParam, data.FRPLANInfo);
        define.dataParam.FR_PLAN_ITEM = data.Item;
        callback && callback();
    });
}

define.IsValidSave = function () {
    if (!define.dataParam.NAME) {
        iview.Message.info("请确认规则名称!");
        return false;
    };
    if (!define.dataParam.FRTYPE) {
        iview.Message.info("请确认满减方式!");
        return false;
    };
    if (!define.dataParam.LIMIT) {
        iview.Message.info("请确认满减限额!");
        return false;
    };
    if (!define.dataParam.FR_PLAN_ITEM) {
        iview.Message.info("请确认满减方案明细!");
        return false;
    };
    return true;
}
define.IsValidMod = function () {
    if (define.dataParam.STATUS=="2") {
        iview.Message.info("数据已使用状态不能更改");
        return false;
    };
    return true;
}
define.mountedInit = function () {
    define.btnConfig = [{
        id: "search",
        authority: "11000300"
    }, {
        id: "add",
        authority: "11000301"
    }, {
        id: "edit",
        authority: "11000301"
    }, {
        id: "save",
        authority: "11000301"
    }, {
        id: "abandon",
        authority: "11000301"
    }];
}