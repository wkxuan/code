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
    define.service = "DpglService";
    define.method = "SearchRegion";
    define.methodList = "SearchRegion";
    define.Key = 'REGIONID';

    define.searchParam.BRANCHID = "";
}

define.initDataParam = function () {
    define.dataParam.REGIONID = "";
    define.dataParam.NAME = "";
    define.dataParam.CODE = "";
    define.dataParam.ORGID = "";
    define.dataParam.ORGIDCASCADER = [];
    define.dataParam.AREA_BUILD = "";
    define.dataParam.AREA_USABLE = "";
    define.dataParam.AREA_RENTABLE = "";
    define.dataParam.STATUS = "";
}

define.newRecord = function () {
    define.dataParam.STATUS = "1";
    define.dataParam.ORGIDCASCADER = [];   
}

define.otherMethods = {
    branchChange: function (value) {
        define.initDataParam();
        define.showlist();
    },
    orgChange: function (value, selectedData) {
        define.dataParam.ORGID = value[value.length - 1];
    }
}

define.mountedInit = function () { }

define.showOne = function (data, callback) {
    _.Ajax('GetRegion', {
        Data: { REGIONID: data }
    }, function (data) {
        $.extend(define.dataParam, data.regionelement);
        define.dataParam.ORGIDCASCADER = define.dataParam.ORGIDCASCADER.toString().split(",");
        define.dataParam.STATUS = data.regionelement.STATUS + ""; 
        callback && callback();
    });
}

define.IsValidSave = function () {
    if (!define.searchParam.BRANCHID) {
        iview.Message.info("请选择门店!");
        return false;
    };
    define.dataParam.BRANCHID = define.searchParam.BRANCHID;
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