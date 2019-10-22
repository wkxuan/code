define.beforeVue = function () {
    define.screenParam.colDef = [
        {
            title: "代码",
            key: 'CODE', width: 175
        },
        {
            title: '名称',
            key: 'NAME'
        }];
    define.screenParam.branchData = [];
    define.service = "DpglService";
    define.method = "SearchRegion";
    define.methodList = "SearchRegion";
    define.Key = 'REGIONID';
}

define.initDataParam = function () {
    define.dataParam.REGIONID = "";
    define.dataParam.NAME = "";
    define.dataParam.CODE = "";
    define.dataParam.ORGIDCASCADER = [];
    define.dataParam.AREA_BUILD = "";
    define.dataParam.AREA_USABLE = "";
    define.dataParam.AREA_RENTABLE = "";
    define.dataParam.STATUS = "";
    define.dataParam.ORGID = "";
}

define.newRecord = function () {
    if (!define.searchParam.BRANCHID) {
        iview.Message.info("请请选择门店!");
        return;
    };
    define.dataParam.STATUS = "1";
    define.dataParam.ORGIDCASCADER = [];   
}

define.otherMethods = {
    branchChange: function (value) {
        define.dataParam.BRANCHID = define.searchParam.BRANCHID;
        define.initDataParam();
        define.showlist();
    },
    orgChange: function (value, selectedData) {
        define.dataParam.ORGID = value[value.length - 1];
    }
}

define.mountedInit = function () {
    _.Ajax('SearchInit', {
        Data: {}
    }, function (data) {
        Vue.set(define.screenParam, "ORGData", data.treeOrg.Obj);
    });

    _.Ajax('GetBranch', {
        Data: { ID: "" }
    }, function (data) {
        if (data.dt) {
            define.screenParam.branchData = [];
            for (var i = 0; i < data.dt.length; i++) {
                define.screenParam.branchData.push({ value: data.dt[i].ID, label: data.dt[i].NAME })
            }
            define.searchParam.BRANCHID = data.dt[0].ID;
            define.showlist();
        }
    });
}

define.showOne = function (data, callback) {
    _.Ajax('GetRegion', {
        Data: { REGIONID: data }
    }, function (data) {
        $.extend(define.dataParam, data.regionelement);
        define.dataParam.ORGIDCASCADER = define.dataParam.ORGIDCASCADER.toString().split(",");
        define.dataParam.STATUS = data.regionelement.STATUS+""; //控件接受string类型
        callback && callback();
    });
}

define.IsValidSave = function () {
    if (!define.dataParam.CODE) {
        iview.Message.info("区域代码不能为空!");
        return false;
    }
    if (!define.dataParam.NAME) {
        iview.Message.info("区域名称不能为空!");
        return false;
    }
    if (!define.dataParam.ORGID) {
        iview.Message.info("管理机构不能为空!");
        return false;
    }
    if (!define.dataParam.AREA_BUILD) {
        iview.Message.info("建筑面积不能为空!");
        return false;
    }
    return true;
}